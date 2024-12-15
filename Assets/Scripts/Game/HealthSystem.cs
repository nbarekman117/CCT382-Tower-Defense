using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    public int playerHealth = 10; // Starting health
    public Text healthText; // Reference to the Text UI element

    [SerializeField]
    private AudioClip loseLifeSound; // Sound effect for losing a life
    private AudioSource audioSource; // AudioSource to play the sound

    // Initialize health system and UI
    public void Init()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource component
        audioSource.clip = loseLifeSound; // Assign the sound effect
        audioSource.playOnAwake = false; // Ensure the sound doesn't play on initialization

        UpdateHealthUI(); // Make sure UI starts with the correct health value
    }

    // Decrease the player's health and update the UI
    public void DecreaseHealth(int amount)
    {
        playerHealth -= amount;

        // Play the lose life sound effect
        if (audioSource && loseLifeSound)
        {
            audioSource.Play();
        }

        UpdateHealthUI();

        if (playerHealth <= 0)
        {
            TriggerGameOver();
        }
    }

    // Update the health UI text
    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = playerHealth.ToString(); // Update the UI Text
        }
        else
        {
            Debug.LogWarning("Health Text not assigned!");
        }
    }

    // Trigger the Game Over screen by loading the Game Over scene
    private void TriggerGameOver()
    {
        Time.timeScale = 0f; // Pause the game (optional)
        SceneManager.LoadScene("Game over"); // Replace with the actual name of your Game Over scene
    }

    // Restart the game by reloading the gameplay scene
    public void RestartGame()
    {
        Time.timeScale = 1f; // Unpause the game
        playerHealth = 10; // Reset health
        UpdateHealthUI(); // Reset the health UI
        SceneManager.LoadScene("GameplayScene"); // Replace with the actual name of your gameplay scene
    }

    // Quit the game (in a built game, this will close the application)
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops the game if running in the Editor
#else
        Application.Quit(); // Quits the game in a built application
#endif
    }
}
