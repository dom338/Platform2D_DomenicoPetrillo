using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject bulletPrefab;
    public float fireRate = 0.2f;
    public AudioClip fireSound;
    public Sprite weaponSprite;
    public int projectilesPerShot = 1;
    public float spreadAngle = 0f;

}
