using UnityEngine;

public class PlayerScript : CharacterBehaviour
{
    [Header("Health")]
    private float maxHealth = 100f;
    private HealthSystem health;

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

    [Header("Transform")]
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    [Header("EquipmentHolder")]
    public EquipmentHolder holder;

    // Start is called before the first frame update
    void Start()
    {
        health = new HealthSystem(maxHealth);
        health.InitHealth();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Health Check
        health.CheckHealth();
        if (health.isDead) Die();

        //Ground Check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        //foreach (Transform item in holder.transform)
        //{
            
        //}

        GetInput();
        SpeedLimit();
        if (grounded) rb.drag = groundDrag;
        else rb.drag = 0;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    protected override void Attack()
    {

    }

    protected override void Die()
    {
        Destroy(gameObject);
    }

    protected override void Interact()
    {
        
    }

    protected override void Move()
    {
        //Move to cam direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //Add force
        if (grounded && isRunning == false)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); //on ground
        }
        else if (grounded && isRunning == true)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 15f, ForceMode.Force); //on ground
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force); //in air
        }
    }

    protected override void Run()
    {

    }

    protected override void Jump()
    {
        //Reset Y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
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
