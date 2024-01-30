using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Loader")]
    [SerializeField] private AsyncLoader asyncLoader;

    [Header("Panel")]
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject scoreboardPanel;
    public void PlayGame()
    {
        asyncLoader.LoadLevel(1);
    }

    public void Option()
    {
        optionPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void Scoreboard()
    {
        scoreboardPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
