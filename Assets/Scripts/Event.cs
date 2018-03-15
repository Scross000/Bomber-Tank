using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Event : MonoBehaviour {

    public Button pauseButton;
    public Button quitButton;
    private bool paused;

    // Use this for initialization
    void Start()
    {
        paused = false;
        pauseButton.onClick.AddListener(pauseGame);
        quitButton.onClick.AddListener(quit);
    }

    public void quit()
    {
        SceneManager.LoadScene("Start");
    }

    public void pauseGame()
    {
        if (!paused)
        {
            Time.timeScale = 0;
            paused = true;
        }
        else
        {
            Time.timeScale = 1;
            paused = false;
        }
    }
}
