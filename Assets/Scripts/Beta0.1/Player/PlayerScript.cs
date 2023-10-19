using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : CharacterBehaviour
{
    [Header("Health")]
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private Image backHealthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    private HealthSystem health;
    private float maxHealth = 100f;
    private bool isUsedMedkit = true;
    private float healAmount;
    private bool isHit = false;
    private float damageAmount;

    [Header("Movement")]
    private float moveSpeed = 7f;
    private float jumpForce = 7f;
    private float jumpCooldown = 0.25f;
    private float airMultiplier = 0.4f;
    private float groundDrag = 5f;
    private bool isRunning = false;
    private bool readyToJump;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWater;
    private bool grounded;

    [Header("Transform")]
    [SerializeField] private Transform orientation;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rb;

    [Header("Interact Message")]
    [SerializeField] private Camera cam;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask interactMask;
    [SerializeField] private TextMeshProUGUI promptText;

    private void Awake()
    {
        health = new HealthSystem(maxHealth, 0f, 2f);
    }
    // Start is called before the first frame update
    void Start()
    {
        health.InitHealth();
        rb = GetComponent<Rigidbody>(); 
        rb.freezeRotation = true;
        readyToJump = true;

        var fps = GetComponentInParent<FPS>();
        cam = fps.GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //Health Check
        health.CheckHealth();
        if (health.isDead) Die();
        Health();

        //Ground Check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        GetInput();
        SpeedLimit();
        if (grounded)
        {
            rb.drag = groundDrag;
            Debug.Log("Grounded");
        }
        else rb.drag = 0;

        // Interact Check
        Interact();
    }

    // Inherited Function
    protected override void Interact()
    {
        UpdateInteractText();
    }

    protected override void Move()
    {
        //Move to cam direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //Add force
        if (grounded && !isRunning)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); //on ground
        }
        else if (grounded && isRunning)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 40f, ForceMode.Force); //on ground
        }
        else if (!grounded && !isRunning)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force); //in air
        }
        else if (!grounded && isRunning)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 40f * airMultiplier, ForceMode.Force); //in air
        }
    }

    protected override void Jump()
    {
        //Reset Y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //Jump
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }

    // Player Function
    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Get Jump Input
        if (Input.GetKeyDown(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Get Sprint Input
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
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

    private void Health()
    {
        health.SetHealth(Mathf.Clamp(health.GetHealth(), 0, health.GetMaxHealth()));
        UpdateHealthUI();
        // Test Damage and Heal System
        if (Input.GetKeyDown(KeyCode.F))
        {
            health.TakeDamage(Random.Range(5, 10));
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            health.RestoreHealth(Random.Range(5, 10));
        }
        // Damage and Heal System
        if (isHit)
        {
            health.TakeDamage(damageAmount);
            isHit = false;
        }
        if (!isUsedMedkit)
        {
            health.RestoreHealth(healAmount);
            isUsedMedkit = true;
        }
    }

    // Call These function 
    public void UseMedkit(float heal)
    {
        healAmount = heal;
        isUsedMedkit = false;
    }

    public void TakeDamage(float damage)
    {
        damageAmount = damage;
        isHit = true;
    }

    // UI
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
    
    private void UpdateInteractText()
    {
        UpdateText(string.Empty);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, interactMask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                UpdateText(interactable.promptMessage);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.BaseInteract();
                }
            }
        }
    }
    
    private void UpdateText(string message)
    {
        promptText.text = message;
    }
}
