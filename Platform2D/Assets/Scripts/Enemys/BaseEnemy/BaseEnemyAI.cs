using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public class BaseEnemyAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    public float speed = 5;
    public float patrolDistance = 5f;

    private Vector2 startingPosition;
    private bool movingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        startingPosition = transform.position;
    }

    void FixedUpdate()
    {
        Patrol();
    }

    private void Patrol()
    {
        if (movingRight)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

            if (transform.position.x >= startingPosition.x + patrolDistance)
            {
                movingRight = false;
                sprite.flipX = true;
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);

            if (transform.position.x <= startingPosition.x - patrolDistance)
            {
                movingRight = true;
                sprite.flipX = false;
            }
        }
    }



}
