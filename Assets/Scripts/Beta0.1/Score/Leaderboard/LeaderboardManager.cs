using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using Dan.Main;
using Unity.VisualScripting;
using Dan.Models;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] GameObject highscoreUIElementPrefab;
    [SerializeField] Transform elementWrapper;

    List<GameObject> uiElements = new List<GameObject>();

    HighScoreHandler highScoreHandler;

    private void OnEnable()
    {
        HighScoreHandler.onHighscoreListChanged += UpdateUI;
    }

    private void OnDisable()
    {
        HighScoreHandler.onHighscoreListChanged -= UpdateUI;
    }

    private void UpdateUI(List<HighScoreElement> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            HighScoreElement el = list[i];

            if (el != null && el.points >= 0)
            {
                if (i >= uiElements.Count)
                {
                    // instantiate new entry
                    var inst = Instantiate(highscoreUIElementPrefab, Vector3.zero, Quaternion.identity);
                    inst.transform.SetParent(elementWrapper, false);

                    uiElements.Add(inst);
                }

                // write or overwrite name & points
                var texts = uiElements[i].GetComponentsInChildren<TextMeshProUGUI>();
                texts[0].text = el.playerName;
                texts[1].text = el.points.ToString();
            }
        }

        //Leaderboards.Leaderboard.GetEntries(entries =>
        //{
        //    foreach (var t in _entryTextObjects)
        //    {
        //        t.text = "";
        //    }

        //    var length = Mathf.Min(_entryTextObjects.Length, entries.Length);
        //    for (int i = 0; i < length; i++)
        //    {
        //        _entryTextObjects[i].text = $"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score}";
        //    }
        //});
    }

    //public void UploadEntry()
    //{
    //    Leaderboards.Leaderboard.UploadNewEntry(_usernameInputField.text, Score, isSuccessful =>
    //    {
    //        if (isSuccessful)
    //        {
    //            LoadEntries();
    //        }
    //    });
    //}
}
