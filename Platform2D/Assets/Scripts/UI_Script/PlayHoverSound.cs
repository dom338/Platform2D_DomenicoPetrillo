using UnityEngine;

public class PlayHoverSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hoverSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayHover()
    {
        audioSource.PlayOneShot(hoverSound);
    }
}
