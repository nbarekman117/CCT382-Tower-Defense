using UnityEngine;
using UnityEngine.UI; // Required for working with UI components
using UnityEngine.SceneManagement; // Required for scene management

public class HealthSystem : MonoBehaviour
{
    public int playerHealth = 10; // Starting health
    public Text healthText;       // Reference to the Text UI element

    // Initialize health system and UI
    public void Init()
    {
        UpdateHealthUI(); // Make sure UI starts with the correct health value
    }

    // Decrease the player's health and update the UI
    public void DecreaseHealth(int amount)
    {
        playerHealth -= amount;
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
        // Pause the game (optional)
        Time.timeScale = 0f;

        // Load the Game Over scene
        SceneManager.LoadScene("Game over"); // Replace "GameOverScene" with the actual name of your Game Over scene
    }

    // Restart the game by reloading the gameplay scene
    public void RestartGame()
    {
        Time.timeScale = 1f;  // Unpause the game
        playerHealth = 10;    // Reset health (or you can save the state if needed)
        UpdateHealthUI();     // Reset the health UI
        SceneManager.LoadScene("GameplayScene"); // Replace "GameplayScene" with the actual name of your gameplay scene
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
