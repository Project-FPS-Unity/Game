using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageController : MonoBehaviour
{
    private bool isChange = false;
    [Header("UI")]
    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject endPanel;
    // Update is called once per frame
    void Update()
    {
        Debug.Log(TimeUpdater.timeRemain);
        if (TimeUpdater.timeRemain < 0 && isChange == false)
        {
            // Ending
            ToGameOverPanel();
            //SceneManager.LoadSceneAsync(0);
            isChange = true;
        }
    }

    public void ToGameOverPanel()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerUI.SetActive(false);
        endPanel.SetActive(true);
    }
}
