public class HealthSystem
{
    private float maxHealth;
    private float currentHealth;
    public bool isDead;

    public HealthSystem(float maxHealth)
    {
        this.maxHealth = maxHealth;
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
    }

    public void RestoreHealth(float heal)
    {
        currentHealth += heal;
    }
}
