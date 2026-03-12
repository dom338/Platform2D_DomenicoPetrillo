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

    private void OnAttack(InputAction.CallbackContext context)
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
}