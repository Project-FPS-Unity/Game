using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFootActions;

    private Player player;
    private PlayerLook playerLook;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        onFootActions = playerInput.OnFoot;

        player = GetComponent<Player>();
        playerLook = GetComponent<PlayerLook>();

        onFootActions.Jump.performed += ctx => player.Jump();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        player.ProcessMove(onFootActions.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        playerLook.ProcessLook(onFootActions.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFootActions.Enable();
    }

    private void OnDisable()
    {
        onFootActions.Disable();
    }
}
