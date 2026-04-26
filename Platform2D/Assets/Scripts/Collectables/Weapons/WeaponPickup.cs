using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private WeaponData weaponToGive;
    [SerializeField] private int ammoToGive = 10;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    public AudioClip pickupSound;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (weaponToGive != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = weaponToGive.weaponSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerShootS player = collision.GetComponent<PlayerShootS>();

        if (player != null)
        {
            player.PickupWeapon(weaponToGive, ammoToGive);
            audioSource.PlayOneShot(pickupSound);
            Destroy(gameObject, 0.2f);
        }
    }
}
