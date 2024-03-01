using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI time;
    public static float timeRemain = 120f;
    public static float surviveTime = 0f;
    public static int enemyKillCount = 0;

    private void Awake()
    {
        time = GameObject.Find("TimeRemain").GetComponent<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            timeRemain = 120f;
            surviveTime = 0f;
            enemyKillCount = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeRemain = timeRemain - Time.deltaTime;
        surviveTime += Time.deltaTime;
        time.text = "Time Remain : " + timeRemain.ToString("0.00") + " s";
    }
}
