using UnityEngine;

public class MedikitS : MonoBehaviour
{
    public int healingAmount = 30;
    public PlayerS PlayerScript;
    private AudioSource audioSource;
    public AudioClip HealingSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerS>() != null)
        {
            audioSource.PlayOneShot(HealingSound);
            PlayerScript.Heal(healingAmount);
            Destroy(this.gameObject, 0.2f);
        }

    }
}
