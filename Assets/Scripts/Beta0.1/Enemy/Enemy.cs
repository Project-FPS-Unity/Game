using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    private float maxHealth = 100f;
    private HealthSystem health;
    private HealthBar healthBar;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    bool grounded;

    private float timeBonus = 10f;

    [Header("ItemDrop")]
    [SerializeField] private GameObject[] itemDropList;
    private int totalItemsList = 0;
    private int itemIndex;

    private void Awake()
    {
        health = new HealthSystem(maxHealth);
        healthBar = gameObject.GetComponentInChildren<HealthBar>();

    }

    // Start is called before the first frame update
    void Start()
    {
        health.InitHealth();
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //Health Check
        health.CheckHealth();
        healthBar.SetHealth(health.GetCurrentHealth());
        if (health.isDead) 
        {
            Die();
        }
    }

    private void Die()
    {
        DropItem();
        TimeUpdater.timeRemain += timeBonus;
        TimeUpdater.enemyKillCount += 1;
        Destroy(gameObject); 
    }

    public void TakeDamage(float damage)
    {
        health.TakeDamage(damage);
    }

    private void DropItem()
    {
        foreach (GameObject item in itemDropList)
        {
            totalItemsList++;
        }
        itemIndex = Random.Range(0, totalItemsList);
        Instantiate(itemDropList[itemIndex], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, gameObject.transform.position.z), Quaternion.identity);
    }
}
