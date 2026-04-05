using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootS : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private PlayerS playerS;
    [SerializeField] private AudioSource PlayerAudio;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer weaponRenderer;
    [SerializeField] private WeaponData baseWeapon;


    private PlayerControls controls;
    private float nextFireTime;
    public AudioClip FireSound;
    public int ammo = 50;
    public TextMeshProUGUI ammoText;
    private WeaponData currentWeapon;
    private WeaponData secondaryWeapon;
    private int secondaryAmmo = 0;

    private void Awake()
    {
        controls = new PlayerControls();

        if (playerS == null)
        {
            playerS = GetComponent<PlayerS>();
        }
        if (animator == null)
        {
            animator = GetComponent<Animator>();
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
        currentWeapon = baseWeapon;
        UpdateWeaponSprite();
        UpdateAmmoUI();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (currentWeapon == null)
        {
            return;
        }
        if (Time.time < nextFireTime)
        {
            return;
        }
        ammo = GetCurrentAmmo();

        if (ammo <= 0)
        {
            HandleEmptyWeapon();
            return;
        }
        if (firePoint == null)
        {
            Debug.Log("firePoint equal null");
            return;
        }
        if (currentWeapon.bulletPrefab == null)
        {
            Debug.Log("bulletPrefab equal null");
            return;
        }
        Shoot();
        DecreaseAmmo();

        nextFireTime = Time.time + currentWeapon.fireRate;

        if (GetCurrentAmmo() <= 0)
        {
            HandleEmptyWeapon();
        }
        else
        {
            UpdateAmmoUI();
        }

    }

    private void Shoot()
    {
        if (animator != null)
        {
            animator.SetTrigger("Shooting");
        }
        GameObject bullet = Instantiate(currentWeapon.bulletPrefab, firePoint.position, quaternion.identity);

        BasicProjectile bulletScript = bullet.GetComponent<BasicProjectile>();
        if (bulletScript != null)
        {
            float direction = 1f;

            if (playerS != null)
            {
                direction = playerS.IsFacingRight() ? 1f : -1f;
            }
            bulletScript.SetDirection(direction);
        }
        if (currentWeapon.fireSound != null && PlayerAudio != null)
        {
            PlayerAudio.PlayOneShot(currentWeapon.fireSound);
        }

    }

    private void DecreaseAmmo()
    {
        if (currentWeapon == baseWeapon)
        {
            ammo--;
            if (ammo < 0)
            {
                ammo = 0;
            }
        }
        else
        {
            secondaryAmmo--;
            if (secondaryAmmo < 0)
            {
                secondaryAmmo = 0;
            }
        }
    }

    private int GetCurrentAmmo()
    {
        if (currentWeapon == baseWeapon)
        {
            return ammo;
        }
        return secondaryAmmo;
    }

    private void HandleEmptyWeapon()
    {
        if (currentWeapon != null && currentWeapon != baseWeapon)
        {
            secondaryWeapon = null;
            secondaryAmmo = 0;
            currentWeapon = baseWeapon;

            UpdateWeaponSprite();
            UpdateAmmoUI();
            return;
        }
        UpdateAmmoUI();
    }

    public void PickupWeapon(WeaponData newWeapon, int ammoAmount)
    {
        if (newWeapon == null)
        {
            return;
        }

        secondaryWeapon = newWeapon;
        secondaryAmmo = ammoAmount;

        if (secondaryAmmo < 0)
        {
            secondaryAmmo = 0;
        }
        currentWeapon = secondaryWeapon;
        UpdateWeaponSprite();
        UpdateAmmoUI();
    }

    public WeaponData GetCurrentWeapon()
    {
        return currentWeapon;
    }
    public WeaponData GetBaseWeapon()
    {
        return baseWeapon;
    }

    public bool HasSecondaryWeapon()
    {
        return secondaryWeapon != null;
    }

    private void UpdateWeaponSprite()
    {
        if (weaponRenderer != null)
        {
            if (currentWeapon != null)
            {
                weaponRenderer.sprite = currentWeapon.weaponSprite;
                weaponRenderer.enabled = currentWeapon.weaponSprite != null;
            }
            else
            {
                weaponRenderer.sprite = null;
                weaponRenderer.enabled = false;
            }
        }
    }

    private void UpdateAmmoUI()
    {
        ammo = GetCurrentAmmo();
        if (ammoText != null)
        {
            ammoText.text = ammo.ToString();

            if (ammo > 29)
            {
                ammoText.color = Color.white;
            }
            else if (ammo > 10)
            {
                ammoText.color = Color.yellow;
            }
            else
            {
                ammoText.color = Color.red;
            }

        }
    }

    public void AddAmmo(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        ammo += amount;

        if (secondaryWeapon != null)
        {
            secondaryAmmo += amount;
        }
        UpdateAmmoUI();
    }
}