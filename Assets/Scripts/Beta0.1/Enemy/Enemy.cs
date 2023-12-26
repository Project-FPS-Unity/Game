using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : CharacterBehaviour
{
    [Header("Health")]
    private float maxHealth = 100f;
    private HealthSystem health;
    private HealthBar healthBar;

    [Header("Movement")]
    private float moveSpeed = 7f;
    private float jumpForce = 7f;
    private float jumpCooldown = 0.25f;
    private float airMultiplier = 0.4f;
    private float groundDrag = 5f;
    private bool isRunning = false;
    bool readyToJump;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    bool grounded;

    //[Header("Transform")]
    //public Transform orientation;
    //float horizontalInput;
    //float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    //[Header("EquipmentHolder")]
    //public EquipmentHolder holder;

    private float enemyScore = 100f;

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
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Health Check
        health.CheckHealth();
        healthBar.SetHealth(health.GetCurrentHealth());
        if (health.isDead) Die();
    }

    protected override void Die()
    {
        ScoreManager.scoreCount += enemyScore;
        Destroy(gameObject);
    }

    protected override void Interact()
    {
    }

    protected override void Move()
    {
    }

    protected override void Jump()
    {
        //Reset Y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //Jump
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void TakeDamage(float damage)
    {
        health.TakeDamage(damage);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void SpeedLimit()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Apply limit
        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitVelocity.x, rb.velocity.y, limitVelocity.z);
        }
    }
}
