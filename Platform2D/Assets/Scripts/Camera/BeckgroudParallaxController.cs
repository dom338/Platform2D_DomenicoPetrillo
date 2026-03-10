using UnityEngine;

public class BeckgroudParallaxController : MonoBehaviour
{
    private float startPos;
    private float lenght;
    public Transform cam;
    public float parallaxSpeed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        startPos = transform.position.x;

        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        float distance = cam.transform.position.x * parallaxSpeed;
        float mouvment = cam.transform.position.x * (1 - parallaxSpeed);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (mouvment > startPos + lenght)
        {
            startPos += lenght;
        }
        else if (mouvment < startPos - lenght)
        {
            startPos -= lenght;
        }
    }
}
