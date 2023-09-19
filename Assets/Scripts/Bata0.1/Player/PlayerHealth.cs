using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // Health bar UI
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private Image backHealthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    // Varuables
    private HealthSystem health;
    private float maxHealth = 100f;
    private void Awake()
    {
        health = new HealthSystem(maxHealth, 0f, 2f);
    }
    // Start is called before the first frame update
    void Start()
    {
        health.InitHealth();
    }

    // Update is called once per frame
    void Update()
    {
        health.SetHealth(Mathf.Clamp(health.GetHealth(), 0, health.GetMaxHealth()));
        UpdateHealthUI();
        // Test Damage and Heal Function
        if (Input.GetKeyDown(KeyCode.F))
        {
            health.TakeDamage(Random.Range(1, 10));
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            health.RestoreHealth(Random.Range(1, 10));
        }
    }
    
    // Player's HealthBar UI Update
    private void UpdateHealthUI()
    {
        // Debug.Log(health);
        healthText.text = health.GetHealth().ToString();
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health.GetHealth() / health.GetMaxHealth();
        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            health.SetLerpTimer(health.GetLerpTimer() + Time.deltaTime);
            float percentComplete = health.GetLerpTimer() / health.GetChipSpeed();
            percentComplete *= percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }

        if (fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            health.SetLerpTimer(health.GetLerpTimer() + Time.deltaTime);
            float percentComplete = health.GetLerpTimer() / health.GetChipSpeed();
            percentComplete *= percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }
}
