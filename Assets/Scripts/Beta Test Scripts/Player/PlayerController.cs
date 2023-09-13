using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed = 7f;

    public float JumpForce = 7f;
    public float JumpCooldown = 0.25f;
    public float AirMultiplier = 0.4f;
    bool ReadyToJump;

    public float GroundDrag = 5f;

    [Header("Ground Check")]
    public float PlayerHeight = 2f;
    public LayerMask WhatIsGround;
    bool Grounded;

    public Transform Orientation;

    float HorizontalInput;
    float VerticalInput;

    Vector3 MoveDirection;

    Rigidbody Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.freezeRotation = true;
        ReadyToJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Ground Check
        Grounded = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.2f, WhatIsGround);

        GetInput();
        SpeedLimit();

        if (Grounded) Rigidbody.drag = GroundDrag;
        else Rigidbody.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && ReadyToJump && Grounded)
        {
            ReadyToJump = false;
            JumpPlayer();
            Invoke(nameof(ResetJump), JumpCooldown);
        }
    }

    private void MovePlayer()
    {
        //Move to cam direction
        MoveDirection = Orientation.forward * VerticalInput + Orientation.right * HorizontalInput;
        //Add force
        if (Grounded) {
            Rigidbody.AddForce(MoveDirection.normalized * MoveSpeed * 10f, ForceMode.Force); //on ground
        }
        else if (!Grounded)
        {
            Rigidbody.AddForce(MoveDirection.normalized * MoveSpeed * 10f * AirMultiplier, ForceMode.Force); //in air
        }
    }

    private void JumpPlayer()
    {
        //Reset Y velocity
        Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 0f, Rigidbody.velocity.z);

        Rigidbody.AddForce(transform.up * JumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        ReadyToJump = true;
    }

    private void SpeedLimit()
    {
        Vector3 flatVelocity = new Vector3(Rigidbody.velocity.x, 0f, Rigidbody.velocity.z);

        //Apply limit
        if (flatVelocity.magnitude > MoveSpeed)
        {
            Vector3 limitVelocity = flatVelocity.normalized * MoveSpeed;
            Rigidbody.velocity = new Vector3(limitVelocity.x, Rigidbody.velocity.y, limitVelocity.z);
        }
    }
}
