using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    public static float scoreCount;

    private void Awake()
    {
        score = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        scoreCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "Score : " + Mathf.Round(scoreCount);
    }
}
