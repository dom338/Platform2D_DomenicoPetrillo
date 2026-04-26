using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifeTime = 3f;

    private Rigidbody2D rb;
    private Vector2 moveDirection = Vector2.right;

    public float damage = 50;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.linearVelocity = moveDirection * speed;
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(float newDirection)
    {
        if (newDirection >= 0)
        {
            moveDirection = Vector2.right;
        }
        else
        {
            moveDirection = Vector2.left;
        }
        UpdateVisualDirection();
    }

    public void SetDirection(Vector2 newDirection)
    {
        moveDirection = newDirection.normalized;
        UpdateVisualDirection();
    }

    private void UpdateVisualDirection()
    {
        Vector3 scale = transform.localScale;

        if (moveDirection.x != 0f)
        {
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveDirection.x);
        }
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            BaseEnemyAI baseEnemy = collision.GetComponent<BaseEnemyAI>();
            if (baseEnemy != null)
            {
                baseEnemy.TakeDamageE(damage);
            }
            Destroy(gameObject);
        }
        if (!collision.isTrigger)
        {
            Destroy(gameObject);
        }

    }
}

