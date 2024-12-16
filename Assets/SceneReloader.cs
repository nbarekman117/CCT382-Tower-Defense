using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    // Call this method to reload the scene
    public void ReloadScene()
    {
        // Optionally reset any static variables or singleton instances here
        ResetStaticVariables();

        // Reload the current scene using its name or index
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // If needed, clear PlayerPrefs (if you're using them)
        // PlayerPrefs.DeleteAll(); // Uncomment if you want to reset all PlayerPrefs data
    }

    // Example of resetting static variables or persistent data
    private void ResetStaticVariables()
    {
        // Reset static variables here if needed
        // Example: StaticClass.SomeStaticVariable = 0;

        // Reset any singleton instances if applicable
        // Example: YourSingleton.Instance.Reset(); 
    }
}
