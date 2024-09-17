using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;

    private Rigidbody rb;
    private CharacterController controller;
    private PlayerInput playerInput;
    private float direction;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        playerInput.onActionTriggered += PlayerInput_onActionTriggered;
    }

    private void Update()
    {
        float moveX = direction * speed * Time.deltaTime; 
        controller.Move(new Vector3(moveX, controller.velocity.y, controller.velocity.z));
    }

    private void PlayerInput_onActionTriggered(InputAction.CallbackContext context)
    {
        Debug.Log(context);
    }

    public void Move(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<float>();
    }

    public void Jump()
    {
        Debug.Log("Jump");
    }
}
