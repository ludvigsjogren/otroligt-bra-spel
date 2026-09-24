using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference jump;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        jump.action.started += Jump;
    }

    // Update is called once per frame
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
    }

    private void FixedUpdate()
    {
        if(!canMove)
        {
            return;
        }
        rgbd.linearVelocity = new Vector2(moveDirection * movespeed * Time.deltaTime, rgbd.linearVelocity.y);
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
