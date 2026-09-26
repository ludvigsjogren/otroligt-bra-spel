using UnityEngine;

public class EnemyMovementRanged : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float bounciness = 100f;
    [SerializeField] private int damageGiven = 1;

    //Knockback
    [SerializeField] private float knockbackForce = 100f;
    [SerializeField] private float upwardsForce = 5f;

    //Shooting
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootInterval = 2f;

    private SpriteRenderer rend;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();

        InvokeRepeating(nameof(Shoot), 1f, shootInterval);
    }

    private void Update()
    {
        if (moveSpeed < 0)
        {
            rend.flipX = true;
        }
        if (moveSpeed > 0)
        {
            rend.flipX = false;
        }

    }
    //Shooting
    private void Shoot()
    {
        if (bulletPrefab == null || shootPoint == null)
        {
            Debug.LogError("Enemy saknar Bullet Prefab eller Shoot Point!", gameObject);
            return;
        }
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
        if (enemyBullet == null)
        {
            Debug.LogError("Bullet Prefab saknar EnemyBullet-script!", bullet);
        }
        float direction = moveSpeed > 0 ? 1f : -1f;
        enemyBullet.SetDirection(direction);
    }

    void FixedUpdate()
    {
        transform.Translate(new Vector2(moveSpeed, 0) * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("EnemyBlock") || other.gameObject.CompareTag("Enemy"))
        {
            moveSpeed = -moveSpeed;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageGiven);

            if(other.transform.position.x > transform.position.x)
            {
                other.gameObject.GetComponent<PlayerMovement>().TakeKnockback(knockbackForce, upwardsForce);
            }
            else
            {
                other.gameObject.GetComponent<PlayerMovement>().TakeKnockback(-knockbackForce, upwardsForce);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rgbd = other.attachedRigidbody;

            if (rgbd != null)
            {
                rgbd.linearVelocity = new Vector2(rgbd.linearVelocityX, 0);
                rgbd.AddForce(new Vector2(0, bounciness));
            }

            Destroy(gameObject);

        }


    }

}
