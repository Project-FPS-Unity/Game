using UnityEngine;

public class Target : MonoBehaviour
{
    float Health = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0f) Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
