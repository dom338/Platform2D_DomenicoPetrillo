using UnityEngine;
using UnityEngine.Events;

public class KeyCollisionDetector : MonoBehaviour
{
    [SerializeField] private string colliderScript;
    [SerializeField] private UnityEvent _collisionEntered;

    [SerializeField] private UnityEvent _collisionExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent(colliderScript))
        {
            _collisionEntered?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent(colliderScript))
        {
            _collisionExit?.Invoke();
        }
    }
}
