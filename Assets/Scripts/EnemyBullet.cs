using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 5f;

    private Rigidbody2D rgbd;

    private void Awake()
    {
        rgbd = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(float direction)
    {
        rgbd = GetComponent<Rigidbody2D>();
        rgbd.linearVelocity = new Vector2(direction * speed, 0f);
        SpriteRenderer rend = GetComponent<SpriteRenderer>();

        if (direction < 0)
        {
            rend.flipX = true;
        }
        else
        {
            rend.flipX = false;
        }
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            
            
        }

        if (other.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }
        
    }

}
