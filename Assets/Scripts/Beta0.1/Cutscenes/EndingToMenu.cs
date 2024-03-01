using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingToMenu : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Invoke(nameof(toMainMenu), 3.70f);
    }

    private void toMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        SceneManager.LoadScene(0);
    }
}