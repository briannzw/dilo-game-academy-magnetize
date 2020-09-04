using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControllerScript : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject resumeButton;
    public GameObject leverClearText;

    private Scene currentActiveScene;

    private void Start()
    {
        currentActiveScene = SceneManager.GetActiveScene();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pausePanel.activeSelf) PauseGame();
            else ResumeGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(currentActiveScene.name);
    }

    public void EndGame(string msg, Color color)
    {
        pausePanel.SetActive(true);
        resumeButton.SetActive(false);
        leverClearText.GetComponent<Text>().text = msg;
        leverClearText.GetComponent<Text>().color = color;
        leverClearText.SetActive(true);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
