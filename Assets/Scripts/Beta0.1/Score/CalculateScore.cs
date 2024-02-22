using Dan.Main;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class CalculateScore : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] private TextMeshProUGUI timeRemainText;
    [SerializeField] private TextMeshProUGUI timeScoreText;
    [SerializeField] private TextMeshProUGUI enemyKillCountText;
    [SerializeField] private TextMeshProUGUI killScoreText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TMP_InputField username;
    [SerializeField] private HighScoreHandler highScoreHandler;

    private int Score()
    {
        return (int)((TimeUpdater.timeRemain * 10) + (TimeUpdater.enemyKillCount * 100));
    }

    private void Update()
    {
        if (TimeUpdater.timeRemain < 0)
        {
            TimeUpdater.timeRemain = 0;
        }
        timeRemainText.text = TimeUpdater.timeRemain.ToString();
        timeScoreText.text = (TimeUpdater.timeRemain * 10).ToString();
        enemyKillCountText.text = TimeUpdater.enemyKillCount.ToString();
        killScoreText.text = (100 * TimeUpdater.enemyKillCount).ToString();
        totalScoreText.text = ((TimeUpdater.timeRemain * 10) + (100 * TimeUpdater.enemyKillCount)).ToString();
    }

    public void UploadEntry()
    {
        Debug.Log(username.text);
        if (username.text == "")
        {
            return;
        }
        else
        {
            Debug.Log("Saved");
            highScoreHandler.AddHighScoreIfPossible(new HighScoreElement(username.text, Score()));
            Time.timeScale = 1f;
            SceneManager.LoadSceneAsync(0);
        }
    }
}
