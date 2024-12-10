using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    // Singleton instance
    private static AudioManager instance;
    private AudioSource audioSource;

    void Awake()
    {
        // Ensure only one AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevents the object from being destroyed on scene load
            audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
            audioSource.Play(); // Start playing the audio
        }
        else
        {
            Destroy(gameObject); // Destroy this instance if one already exists
        }
    }

    // Optional: method to change the volume
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    // Fade out music smoothly
    public void FadeOutMusic(float fadeDuration)
    {
        StartCoroutine(FadeOut(fadeDuration));
    }

    // Coroutine to fade out the audio
    private IEnumerator FadeOut(float duration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / duration); // Gradually reduce volume
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop(); // Stop the audio once it's completely faded out
    }
}
