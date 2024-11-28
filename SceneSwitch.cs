using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            // current scene 
            string currentScene = SceneManager.GetActiveScene().name;

            // Toggle between GameScene and GraphScene
            if (currentScene == "GameScene")
            {
                SceneManager.LoadScene("GraphScene");
            }
            else if (currentScene == "GraphScene")
            {
                SceneManager.LoadScene("GameScene");
            }
        }
    }
}
