using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Tilemaps;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    [SerializeField] string nextScene;

    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            TransitionToNextScene();
        }
    }

    void TransitionToNextScene()
    {
        SceneManager.LoadScene(nextScene);

        // Reinitialize spawner in new scene (if needed)
        if (GameManager.instance.spawner != null)
        {
            var tilemap = FindObjectOfType<Tilemap>();
            GameManager.instance.spawner.Init(tilemap);
        }
    }
}
