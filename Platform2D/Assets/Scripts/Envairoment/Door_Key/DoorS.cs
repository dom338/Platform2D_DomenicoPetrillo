using UnityEngine;

public class DoorS : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D _collider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
    }

    public void Open()
    {
        animator.SetTrigger("IsOpen");
        _collider.enabled = false;
    }
}
