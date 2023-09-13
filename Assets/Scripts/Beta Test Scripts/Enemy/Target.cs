using UnityEngine;

public class Target : MonoBehaviour
{
    float MaxHealth = 100f;
    float CurrentHealth;

    public HealthBar HealthBar;

    private void Start()
    {
        CurrentHealth = MaxHealth;
        HealthBar.SetMaxHealth(MaxHealth);
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        HealthBar.SetHealth(CurrentHealth);
        if (CurrentHealth <= 0f) Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
