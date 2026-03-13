using UnityEngine;

public class AmmoChestS : MonoBehaviour
{
    public int AmmoQuantity = 30;
    public GameObject Player;
    private PlayerShootS PlayerS;
    private AudioSource audioSource;
    public AudioClip pickUpSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            PlayerS = collision.GetComponent<PlayerShootS>();
            if (PlayerS != null)
            {
                PlayerS.AddAmmo(AmmoQuantity);
                audioSource.PlayOneShot(pickUpSound);
            }

            Destroy(gameObject, 0.2f);

        }
    }
}
