
using NUnit.Framework;
using UnityEngine;

public class BaseEnemyAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float patrolDistance = 4f;


    [SerializeField] private Transform playerTarget;
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float attackRange = 4f;

    [SerializeField] private float lifeAmount = 100f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    private Vector2 startingPosition;
    private bool movingRight = true;
    private bool isAlive = true;
    private float fireTimer;

    private enum EnemyState
    {
        Patrol,
        Chase,
        Attack,
        Dead
    }

    private EnemyState currentState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        startingPosition = transform.position;
        currentState = EnemyState.Patrol;

    }

    private void Update()
    {
        if (!isAlive)
        {
            return;
        }
        if (playerTarget == null)
        {
            return;
        }
        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);
        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
        else if (distanceToPlayer <= detectionRange)
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            currentState = EnemyState.Patrol;
        }
    }

    void FixedUpdate()
    {
        if (!isAlive)
        {
            return;
        }

        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Chase:
                ChasePlayer();
                break;

            case EnemyState.Attack:
                AttackPlayer();
                break;

            case EnemyState.Dead:
                rb.linearVelocity = Vector2.zero;
                break;
        }
    }

    private void Patrol()
    {
        float direction = movingRight ? 1f : -1f;

        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        if (movingRight)
        {
            if (transform.position.x >= startingPosition.x + patrolDistance)
            {
                movingRight = false;
            }
        }
        else
        {
            if (transform.position.x <= startingPosition.x - patrolDistance)
            {
                movingRight = true;
            }
        }

        UpdateSpriteDirection(direction);
    }

    private void ChasePlayer()
    {
        float direction = 0f;

        if (playerTarget.position.x > transform.position.x)
        {
            direction = 1f;
        }
        else if (playerTarget.position.x < transform.position.x)
        {
            direction = -1f;
        }

        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        UpdateSpriteDirection(direction);
    }

    private void AttackPlayer()
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        float direction = 0f;

        if (playerTarget.position.x > transform.position.x)
        {
            direction = 1f;
        }
        else if (playerTarget.position.x < transform.position.x)
        {
            direction = -1f;
        }

        UpdateSpriteDirection(direction);

        fireTimer += Time.fixedDeltaTime;

        if (fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
            return;

        animator.SetTrigger("Shoot");

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        Vector2 bulletDirection = sprite.flipX ? Vector2.left : Vector2.right;

        EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(bulletDirection);
        }
    }
    private void UpdateSpriteDirection(float direction)
    {
        if (direction > 0)
        {
            sprite.flipX = false;
        }
        else if (direction < 0)
        {
            sprite.flipX = true;
        }
    }
    private void HandleAnimations()
    {
        if (animator == null)
            return;

        bool isMoving = currentState == EnemyState.Patrol || currentState == EnemyState.Chase;
        bool isAttacking = currentState == EnemyState.Attack;

        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsAttacking", isAttacking);
    }
    public float TakeDamageE(float damage)
    {
        if (!isAlive)
            return lifeAmount;

        lifeAmount -= damage;

        if (lifeAmount > 0)
        {
            animator.SetTrigger("Hit");
        }

        if (lifeAmount <= 0)
        {
            lifeAmount = 0;
            Die();
        }

        return lifeAmount;
    }

    private void Die()
    {
        isAlive = false;
        currentState = EnemyState.Dead;

        rb.linearVelocity = Vector2.zero;

        animator.SetBool("IsMoving", false);
        animator.SetBool("IsAttacking", false);
        animator.SetTrigger("Death");

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        Destroy(gameObject, 2f);
    }
}




