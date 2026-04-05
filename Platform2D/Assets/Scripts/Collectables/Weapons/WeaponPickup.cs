using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private WeaponData weaponToGive;
    [SerializeField] private SpriteRenderer pickupSpriteRenderer;
    public int ammoToGive = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (pickupSpriteRenderer != null && weaponToGive != null)
        {
            pickupSpriteRenderer.sprite = weaponToGive.weaponSprite;
        }

    }

    private void OTriggerEnter2D(Collider2D collision)
    {
        PlayerShootS Player = collision.GetComponent<PlayerShootS>();

        if (Player != null)
        {
            Player.PickupWeapon(weaponToGive, ammoToGive);
            Destroy(gameObject, 0.2f);
        }
    }
}
