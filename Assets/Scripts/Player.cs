using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerActionType
{
    Jump,
    Crouch,
    Special
}

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerActionType playerActionType;

    [SerializeField]
    private float jumpForce;

    private PlayerInput playerInput;
    private Rigidbody body;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        body = GetComponent<Rigidbody>();

        playerInput.ActionKeyPressed += OnActionKeyPressed;
        playerInput.SpecialKeyPressed += OnSpecialActionKeyPressed;
    }

    private void OnActionKeyPressed()
    {
        switch (playerActionType)
        {
            case PlayerActionType.Jump:
                Jump();
                break;
            case PlayerActionType.Crouch:
                Crouch();
                break;
        }
    }

    private void OnSpecialActionKeyPressed()
    {
        Special();
    }

    private void Jump()
    {
        body.AddForce(jumpForce * Vector3.up);
    }

    private void Crouch()
    {
        // TODO: implement with animator (collider shrinks)
        Debug.Log("Crouch");
    }

    private void Special()
    {
        // TODO
        Debug.Log("Special Key Pressed");
    }

    private void OnDestroy()
    {
        playerInput.ActionKeyPressed -= OnActionKeyPressed;
        playerInput.SpecialKeyPressed -= OnSpecialActionKeyPressed;
    }
}
