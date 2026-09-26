using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private InputActionReference fire;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float bulletSpeed = 10f;

    private SpriteRenderer rend;

    private float shootPointX;
    private float shootPointY;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        shootPointX = shootPoint.localPosition.x;
        shootPointY = shootPoint.localPosition.y;
        fire.action.started += Shoot;
    }

    private void Update()
    {
        if (rend.flipX)
        {
            shootPoint.localPosition = new Vector3(-Mathf.Abs(shootPointX),shootPointY);
        }
        else
        {
            shootPoint.localPosition = new Vector3(Mathf.Abs(shootPointX), shootPointY);
        }
    }

    private void OnDisable()
    {
        fire.action.started -= Shoot;
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        float direction = rend.flipX ? -1f : 1f;
        bulletRb.linearVelocity = new Vector2(direction * bulletSpeed, 0f);
        SpriteRenderer bulletRenderer = bullet.GetComponent<SpriteRenderer>();

        if (direction < 0)
        {
            bulletRenderer.flipX = true;
        }
        else
        {
            bulletRenderer.flipX = false;
        }
    }
}