using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerActionType
{
    Jump,
    DestroyObject
}

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerActionType playerActionType;

    [SerializeField]
    private bool isActive;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float moveSpeed;

    private PlayerInput playerInput;
    private Rigidbody body;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        body = GetComponent<Rigidbody>();

        playerInput.SwitchPlayerKeyPressed += OnSwitchPlayerKeyPressed;
        playerInput.ActionKeyPressed += OnActionKeyPressed;
    }

    private void Update()
    {
        if (isActive)
        {
            Move();
        }
    }

    private void OnActionKeyPressed()
    {
        if (!isActive) return;

        switch (playerActionType)
        {
            case PlayerActionType.Jump:
                Jump();
                break;
            case PlayerActionType.DestroyObject:
                break;
        }
    }

    private void OnSwitchPlayerKeyPressed()
    {
        isActive = !isActive;
    }

    private void Jump()
    {
    }

    private void Move()
    {
        float positionX = transform.position.x + playerInput.Horizontal * moveSpeed * Time.deltaTime;
        float positionZ = transform.position.z + playerInput.Vertical * moveSpeed * Time.deltaTime;
        transform.position = new Vector3(positionX, transform.position.y, positionZ);
    }

    private void OnDestroy()
    {
        playerInput.SwitchPlayerKeyPressed -= OnSwitchPlayerKeyPressed;
        playerInput.ActionKeyPressed -= OnActionKeyPressed;
    }
}
