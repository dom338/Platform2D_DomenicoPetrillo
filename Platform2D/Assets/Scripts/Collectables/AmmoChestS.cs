using UnityEngine;

public class AmmoChestS : MonoBehaviour
{
    public int AmmoQuantity = 30;
    public GameObject Player;
    private PlayerShootS PlayerS;

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
            }

            Destroy(gameObject);

        }
    }
}
