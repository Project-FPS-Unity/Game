using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI time;
    public static float timeRemain;
    public static int enemyKillCount = 0;

    private void Awake()
    {
        time = GameObject.Find("TimeRemain").GetComponent<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        timeRemain = 60f;
        enemyKillCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeRemain = timeRemain - Time.deltaTime;
        time.text = "Time Remain : " + timeRemain.ToString("0.00") + " s";
    }
}
