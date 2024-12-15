using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager instance; // Singleton instance

    public AudioClip menuMusic;       // Music for Menu scene
    public AudioClip level1Music;    // Music for Level 1
    public AudioClip level2Music;    // Music for Level 2
    public AudioClip gameOverMusic;  // Music for Game Over
    public AudioClip gameWinMusic;   // Music for Game Win

    private AudioSource audioSource;  // Reference to the AudioSource
    private Coroutine fadeCoroutine;  // Reference to the fade coroutine

    public float fadeDuration = 1.5f; // Duration of fade-in and fade-out

    private void Awake()
    {
        // Implement the Singleton pattern to ensure only one AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
            audioSource = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Determine the appropriate music for the current scene
        AudioClip newClip = null;

        switch (scene.name)
        {
            case "Menu":
                newClip = menuMusic;
                break;
            case "level1":
                newClip = level1Music;
                break;
            case "level2":
                newClip = level2Music;
                break;
            case "Game over":
                newClip = gameOverMusic;
                break;
            case "Game win":
                newClip = gameWinMusic;
                break;
        }

        // Start fading to the new music
        if (newClip != null)
        {
            FadeToMusic(newClip);
        }
        else
        {
            StopMusic(); // Ensure no music plays if no new clip is assigned
        }
    }

    private void FadeToMusic(AudioClip newClip)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeMusicCoroutine(newClip));
    }

    private IEnumerator FadeMusicCoroutine(AudioClip newClip)
    {
        // Fade out the current music
        if (audioSource.isPlaying)
        {
            float startVolume = audioSource.volume;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
                yield return null;
            }

            audioSource.volume = 0;
            audioSource.Stop(); // Stop the previous music completely
        }

        // Change the clip and fade in the new music
        audioSource.clip = newClip;
        audioSource.Play();

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = 1;
    }

    private void StopMusic()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // Fade out and stop the music
        fadeCoroutine = StartCoroutine(FadeOutAndStopCoroutine());
    }

    private IEnumerator FadeOutAndStopCoroutine()
    {
        if (audioSource.isPlaying)
        {
            float startVolume = audioSource.volume;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
                yield return null;
            }

            audioSource.volume = 0;
            audioSource.Stop();
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event to avoid memory leaks
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
