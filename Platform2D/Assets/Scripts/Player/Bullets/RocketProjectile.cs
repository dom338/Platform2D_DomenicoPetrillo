using UnityEngine;

public class RocketProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float lifeTime = 4f;
    [SerializeField] private float explosionDamage = 60f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject explosionEffect;

    private Rigidbody2D rb;
    private Vector2 moveDirection = Vector2.right;
    private bool hasExploded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Debug.Log("Rocket Start. Direction: " + moveDirection + " Speed: " + speed);

        ApplyVelocity();
        Destroy(gameObject, lifeTime);

    }

    public void SetDirection(float newDirection)
    {
        moveDirection = newDirection >= 0f ? Vector2.right : Vector2.left;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveDirection.x);
        transform.localScale = scale;

        ApplyVelocity();
    }

    private void ApplyVelocity()
    {
        if (rb != null)
        {
            rb.linearVelocity = moveDirection * speed;
            Debug.Log("Rocket velocity: " + rb.linearVelocity);
        }
        else
        {
            Debug.LogWarning("Rocket Rigidbody2D mancante.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("Razzo ha colpito: " + collision.name);
        if (hasExploded)
            return;

        if (collision.isTrigger)
            return;

        Explode();
    }

    private void Explode()
    {
        hasExploded = true;

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            BaseEnemyAI enemy = hit.GetComponent<BaseEnemyAI>();

            if (enemy != null)
            {
                enemy.TakeDamageE(explosionDamage);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}