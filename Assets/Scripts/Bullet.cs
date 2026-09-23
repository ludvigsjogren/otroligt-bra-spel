using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] private float moveSpeed = 10f;
    
    [Range(1, 10)]
    [SerializeField] private float lifeTime = 3f;

    private Rigidbody2D rgbd;

    private void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        rgbd.linearVelocity = transform.right * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Terrain") || gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
