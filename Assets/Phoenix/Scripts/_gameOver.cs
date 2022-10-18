using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _gameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Sends us back to the Main Menu
    public void QuitGame()
    {
        SceneManager.LoadScene("Menu");
    }

    // Restarts Game, sends it back to Start Level
    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
}
