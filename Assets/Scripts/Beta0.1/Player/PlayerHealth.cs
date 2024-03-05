using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private Image backHealthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI medkitText;
    public static HealthSystem health;
    private float maxHealth = 100f;
    // private bool isHit = false;
    // private bool isUsedMedkit = false;

    // Mockup
    //private float damageAmount;
    //private float healAmount;
    // Start is called before the first frame update
    private void Awake()
    {
        health = new HealthSystem(maxHealth, 0f, 2f);
    }

    private void Start()
    {
        health.InitHealth();
    }

    // Update is called once per frame
    private void Update()
    {
        medkitText.text = "x " + Medkit.currentMedkit.ToString();
        //Health Check
        health.CheckHealth();
        Health();
    }
    public void TakeDamage(float damage)
    {
        health.TakeDamage(damage);
    }

    public float GetPlayerCurrentHealth()
    {
        return health.GetCurrentHealth();
    }
    private void Health()
    {
        health.SetHealth(Mathf.Clamp(health.GetHealth(), 0, health.GetMaxHealth()));
        UpdateHealthUI();
    }
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
