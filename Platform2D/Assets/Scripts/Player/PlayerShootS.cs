using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootS : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private PlayerS playerS;
    [SerializeField] private AudioSource PlayerAudio;

    private PlayerControls controls;
    private float nextFireTime;
    public AudioClip FireSound;
    public int ammo = 50;
    public TextMeshProUGUI ammoText;

    private void Awake()
    {
        controls = new PlayerControls();

        if (playerS == null)
        {
            playerS = GetComponent<PlayerS>();
        }
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        controls.Player.Attack.performed -= OnAttack;
        controls.Disable();
    }

    private void Start()
    {
        UpdateAmmoUI();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (ammo > 0)
        {
            if (Time.time < nextFireTime)
                return;

            if (firePoint == null || bulletPrefab == null)
            {
                Debug.LogWarning("FirePoint o BulletPrefab non assegnato nel PlayerWeaponS.");
                return;
            }

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            BasicProjectile bulletScript = bullet.GetComponent<BasicProjectile>();
            if (bulletScript != null)
            {
                float direction = playerS != null && playerS.IsFacingRight() ? 1f : -1f;
                bulletScript.SetDirection(direction);
            }

            nextFireTime = Time.time + fireRate;
            if (FireSound != null)
            {
                PlayerAudio.PlayOneShot(FireSound);
            }
        }

        ammo--;

        if (ammo >= 0)
        {
            UpdateAmmoUI();
        }
        else if (ammo < 0)
        {
            ammo = 0;
        }

    }

    private void UpdateAmmoUI()
    {
        ammoText.text = ammo.ToString();
    }

    public void AddAmmo(int amount)
    {
        ammo += amount;
        UpdateAmmoUI();

    }
}