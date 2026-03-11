using UnityEngine;

public class PersistentMusic : MonoBehaviour
{
    public static PersistentMusic Instance;

    private AudioClip backgroundMusic;

    private AudioSource audiosource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audiosource = GetComponent<AudioSource>();

        if (audiosource == null)
        {
            audiosource = gameObject.AddComponent<AudioSource>();
        }

        audiosource.loop = true;
        audiosource.spatialBlend = 0f;

        if (backgroundMusic != null && audiosource.clip != backgroundMusic)
        {
            audiosource.clip = backgroundMusic;
        }

        if (!audiosource.isPlaying && audiosource.clip != null)
        {
            audiosource.Play();
        }
    }
}
