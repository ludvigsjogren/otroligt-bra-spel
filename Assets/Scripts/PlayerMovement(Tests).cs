using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementRanged : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [Range(0.1f, 1f)]
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference shoot;
    private float moveDirection;

    [SerializeField] private float movespeed = 1f;
    [SerializeField] private float jumpforce = 200f;
    [SerializeField] private Transform leftFoot, rightFoot;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float raycastDistance = 0.25f;
    [SerializeField] private AudioClip[] jumpSounds;
    [SerializeField] private ParticleSystem jumpParticleSystem;
    bool canMove = true;

    private AudioSource audioSource;
    private Rigidbody2D rgbd;
    private SpriteRenderer rend;
    private Animator anim;

    

    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        jump.action.started += Jump;
        shoot.action.started += Shoot;
    }

    void Update()
    {
        moveDirection = move.action.ReadValue<float>();

        anim.SetFloat("MoveSpeed",Mathf.Abs(rgbd.linearVelocity.x));
        anim.SetFloat("VerticalSpeed", rgbd.linearVelocity.y);
        anim.SetBool("IsGrounded", CheckIsGrounded());

        if (moveDirection < 0f)
        {
            FlipSprite(true);
        }

        if(moveDirection > 0f)
        {
            FlipSprite(false);
        }

        //if (Input.GetKey(0))
        //{
            //Shoot();
        //}
    }

    private void FixedUpdate()
    {
        if(!canMove)
        {
            return;
        }
        rgbd.linearVelocity = new Vector2(moveDirection * movespeed * Time.deltaTime, rgbd.linearVelocity.y);
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
    }


    private void OnDisable()
    {
        jump.action.started -= Jump;
    }

    
    private void FlipSprite(bool direction)
    {
        rend.flipX = direction;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (CheckIsGrounded() == true)
        {
            rgbd.AddForce(new Vector2(0, jumpforce));
            jumpParticleSystem.Play();
            int randomJumpSound = Random.Range(0, jumpSounds.Length);
            audioSource.PlayOneShot(jumpSounds[randomJumpSound]);
        }
    }

    private bool CheckIsGrounded()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(leftFoot.position, Vector2.down, raycastDistance, whatIsGround);
        RaycastHit2D righthit = Physics2D.Raycast(rightFoot.position, Vector2.down, raycastDistance, whatIsGround);

        Debug.DrawRay(leftFoot.position, Vector2.down * raycastDistance, Color.blue, 0.25f);
        Debug.DrawRay(rightFoot.position, Vector2.down * raycastDistance, Color.blue, 0.25f);

        if (leftHit.collider != null && leftHit || righthit.collider != null && righthit)
        {
            return true;
        }
        else
        {
            return false;
        }
      
    }

    public void TakeKnockback(float knockbackForce, float upwardsForce)
    {
        canMove = false;
        rgbd.AddForce(new Vector2(knockbackForce, upwardsForce));
        Invoke(nameof(CanMoveAgain), 0.25f);
    }

    private void CanMoveAgain()
    {
        canMove = true;
    }

}
