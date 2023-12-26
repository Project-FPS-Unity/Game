public class HealthSystem
{
    private float maxHealth;
    private float currentHealth;
    private float lerpTimer;
    private float chipSpeed;
    public bool isDead;

    public HealthSystem(float maxHealth, float lerpTimer = 0, float chipSpeed = 0)
    {
        this.maxHealth = maxHealth;
        this.lerpTimer = lerpTimer;
        this.chipSpeed = chipSpeed;
        isDead = false;
    }

    public void InitHealth()
    {
        currentHealth = maxHealth;
    }

    public float CheckHealth()
    {
        if (currentHealth <= 0) isDead = true;
        return currentHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        this.SetLerpTimer(0);
    }

    public void RestoreHealth(float heal)
    {
        currentHealth += heal;
        this.SetLerpTimer(0);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public float GetHealth()
    {
        return currentHealth;
    }

    public void SetHealth(float health)
    {
        this.currentHealth = health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetLerpTimer()
    {
        return lerpTimer;
    }

    public void SetLerpTimer(float lerpTimer)
    {
        this.lerpTimer = lerpTimer;
    }

    public float GetChipSpeed()
    {
        return chipSpeed;
    }

    public void SetChipSpeed(float chipSpeed)
    {
        this.chipSpeed = chipSpeed;
    }
}
