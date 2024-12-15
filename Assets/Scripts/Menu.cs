using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu"); // Replace "Menu" with the exact name of your Main Menu scene
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        // Exit play mode in Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application
        Application.Quit();
#endif
    }
}
