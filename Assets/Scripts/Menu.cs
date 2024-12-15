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
}
