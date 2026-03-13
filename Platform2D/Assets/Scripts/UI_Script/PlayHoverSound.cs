using UnityEngine;

public class PlayHoverSound : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip hoverSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayHover()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);

        }

    }
}
