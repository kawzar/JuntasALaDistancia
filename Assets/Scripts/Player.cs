using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public enum PlayerActionType
{
    Jump,
    Crouch,
    Special
}

public class Player : MonoBehaviour
{
    public event Action PlayerLost;

    [SerializeField]
    private PlayerActionType playerActionType;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float rotationSpeed = 0.25f;

    [SerializeField]
    private Path path;

    [SerializeField]
    private LayerMask floorLayerMask;

    [SerializeField]
    private float maxDistanceToGround = 0.25f;

    private PlayerInput playerInput;
    private Rigidbody body;
    private Vector3 currentDirection;
    private bool isGrounded = true;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        body = GetComponent<Rigidbody>();
        SetCurrentDirection(path.NextWaypoint().transform);
        playerInput.ActionKeyPressed += OnActionKeyPressed;
        playerInput.SpecialKeyPressed += OnSpecialActionKeyPressed;
    }

    private void Update()
    {
        //Vector3 deltaPosition = transform.forward * movementSpeed * Time.deltaTime;
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime, Space.Self);

        Ray feetRay = new Ray(gameObject.transform.position, Vector3.down);
        isGrounded = Physics.Raycast(feetRay, maxDistanceToGround, floorLayerMask);


        if (playerActionType == PlayerActionType.Jump && transform.position.y <= LevelManager.Instance.LowestPlayerPositionY)
        {
            PlayerLost?.Invoke();
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == path.PathTag)
        {
            SetCurrentDirection(path.NextWaypoint().transform);
        }
        else
        {
            Debug.Log(other.gameObject.name);
        }
    }

    private void OnSpecialActionKeyPressed()
    {
        Special();
    }

    private void Jump()
    {
        if (isGrounded)
        {
            body.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
        }
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

    private void SetCurrentDirection(Transform waypoint)
    {
        //Debug.Log("Update direction " + waypoint.gameObject.name);
        currentDirection = waypoint.transform.position - transform.position;
        currentDirection = currentDirection.normalized;
        transform.forward = waypoint.transform.forward;
        //transform.DOLookAt(waypoint.transform.position, rotationSpeed);
        //transform.LookAt(waypoint.transform);
    }

    private void OnDestroy()
    {
        playerInput.ActionKeyPressed -= OnActionKeyPressed;
        playerInput.SpecialKeyPressed -= OnSpecialActionKeyPressed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, currentDirection);
    }
}
