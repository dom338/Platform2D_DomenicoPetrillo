using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private int damage = 15;
    [SerializeField] private float lifeTime = 3f;

    private Vector2 moveDirection;


    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerS player = collision.GetComponent<PlayerS>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        if (!collision.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}