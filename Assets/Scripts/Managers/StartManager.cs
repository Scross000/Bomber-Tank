using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour {

    public Button startButton;
    public Button exitButton;

	// Use this for initialization
	void Start () {
        startButton.onClick.AddListener(startGame);
        exitButton.onClick.AddListener(exitApplication);
	}
	
	public void exitApplication()
    {
        Application.Quit();
    }

    public void startGame()
    {
        SceneManager.LoadScene("Main");
    }
}
