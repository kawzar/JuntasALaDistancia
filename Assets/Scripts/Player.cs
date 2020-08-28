using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using Assets.MultiAudioListener;

public enum PlayerActionType
{
    Jump,
    Crouch,
    Special
}

public class Player : MonoBehaviour
{
    public event Action PlayerLost;

    [Header("This Player")]
    [SerializeField]
    private PlayerActionType playerActionType;

    [SerializeField]
    private Path path;

    [Header("General Settings")]
    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float rotationDuration = 0.025f;


    [SerializeField]
    private LayerMask floorLayerMask;

    [SerializeField]
    private float maxDistanceToGround = 0.25f;

    [Header("Audio Settings")]
    [SerializeField]
    private AudioClip jumpSound;

    private PlayerInput playerInput;
    private Rigidbody body;
    private bool isGrounded = true;
    private bool isActive = true;
    private MultiAudioSource audioSource;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        body = GetComponent<Rigidbody>();
        audioSource = GetComponent<MultiAudioSource>();
        SetCurrentWaypoint(path.NextWaypoint().transform);
        playerInput.ActionKeyPressed += OnActionKeyPressed;
        playerInput.SpecialKeyPressed += OnSpecialActionKeyPressed;
    }

    private void Update()
    {
        if (!isActive)
        {
            body.Sleep();
            return;
        }

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
            SetCurrentWaypoint(path.NextWaypoint().transform);
        }
        else
        {
            Debug.Log(other.gameObject.name);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // TODO: Handle collision for maia
    }

    private void OnSpecialActionKeyPressed()
    {
        Special();
    }

    private void Jump()
    {
        if (isGrounded)
        {
            audioSource.AudioClip = jumpSound;
            audioSource.Play();
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

    private void SetCurrentWaypoint(Transform waypoint)
    {
        transform.DOLookAt(waypoint.transform.position, rotationDuration, AxisConstraint.Y);
    }

    private void OnDestroy()
    {
        playerInput.ActionKeyPressed -= OnActionKeyPressed;
        playerInput.SpecialKeyPressed -= OnSpecialActionKeyPressed;
    }

    public void DisablePlayer()
    {
        isActive = false;
    }
}
