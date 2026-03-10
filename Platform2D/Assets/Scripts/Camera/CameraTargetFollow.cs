using UnityEngine;

public class CameraTargetFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(1.5f, 0.75f, 0f);
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private bool followX = true;
    [SerializeField] private bool followY = true;

    private void FixedUpdate()
    {
        if (player == null) return;

        Vector3 targetPosition = transform.position;
        Vector3 desiredPosition = player.position + offset;

        if (followX)
            targetPosition.x = Mathf.Lerp(transform.position.x, desiredPosition.x, followSpeed * Time.deltaTime);

        if (followY)
            targetPosition.y = Mathf.Lerp(transform.position.y, desiredPosition.y, followSpeed * Time.deltaTime);

        targetPosition.z = 0f;
        transform.position = targetPosition;
    }

    public void SetFacingDirection(bool facingRight)
    {
        offset.x = Mathf.Abs(offset.x) * (facingRight ? 1f : -1f);
    }
}