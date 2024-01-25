using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using Dan.Main;
using Unity.VisualScripting;
using Dan.Models;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _entryTextObjects;
    [SerializeField] private TMP_InputField _usernameInputField;

    [SerializeField] private TextMeshProUGUI _playerScoreText;

    private int Score => int.Parse(_playerScoreText.text);

    private void Start()
    {
        LoadEntries();
    }

    private void LoadEntries()
    {
        Leaderboards.Leaderboard.GetEntries(entries =>
        {
            foreach (var t in _entryTextObjects)
            {
                t.text = "";
            }

            var length = Mathf.Min(_entryTextObjects.Length, entries.Length);
            for (int i = 0; i < length; i++)
            {
                _entryTextObjects[i].text = $"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score}";
            }
        });
    }

    public void UploadEntry()
    {
        Leaderboards.Leaderboard.UploadNewEntry(_usernameInputField.text, Score, isSuccessful =>
        {
            if (isSuccessful)
            {
                LoadEntries();
            }
        });
    }
}
