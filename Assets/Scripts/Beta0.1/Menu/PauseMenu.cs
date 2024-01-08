using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject bgPanel;
    public GameObject pauseMenuPanel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused && pauseMenuPanel.active == true)
            {
                Resume();
            }
            else if (!gameIsPaused)
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        gameIsPaused = false;
        // toggle cursor off
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // close menu
        bgPanel.SetActive(false);

        // resume timer
        Time.timeScale = 1.0f;
    }

    public void Pause()
    {
        gameIsPaused = true;
        // toggle cursor on
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // open menu
        bgPanel.SetActive(true);

        // pause timer
        Time.timeScale = 0f;
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        SceneManager.LoadSceneAsync(0);
    }
}
