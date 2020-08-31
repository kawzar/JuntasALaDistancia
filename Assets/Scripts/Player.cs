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
    public event Action PlayerArrived;

    [Header("This Player")]
    [SerializeField]
    private PlayerActionType playerActionType;

    [SerializeField]
    private ParticleSystem myParticles;

    [Header("Maia Settings")]
    // TODO: Uncomment to implement crouch
    //[SerializeField]
    //private GameObject idleBody;

    //[SerializeField]
    //private GameObject animationBody;

    //[SerializeField]
    //private GameObject playerReference;

    [SerializeField]
    private string crouchAnimationTriggerName = "Crouch";

    [SerializeField]
    private string maiaCollisionTag = "MaiaObstacle";

    [Header("General Settings")]
    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private float movementSpeed;

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
    private bool isActive = false;
    private MultiAudioSource audioSource;

    // TODO: Uncomment to implement crouch
    //private Animator animator;
    //private MeshFilter idleMeshFilter;
    //private MeshFilter animationMeshFilter;
    //private CapsuleCollider idleCapsuleCollider;
    //private CapsuleCollider animationCapsuleCollider;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        body = GetComponent<Rigidbody>();
        audioSource = GetComponent<MultiAudioSource>();
        playerInput.ActionKeyPressed += OnActionKeyPressed;

        // TODO: Uncomment to implement crouch
        //if (playerActionType == PlayerActionType.Crouch)
        //{
        //    animator = GetComponent<Animator>();
        //    idleMeshFilter = idleBody.GetComponent<MeshFilter>();
        //    idleCapsuleCollider = idleBody.GetComponent<CapsuleCollider>();
        //    animationMeshFilter = animationBody.GetComponent<MeshFilter>();
        //    animationCapsuleCollider = animationBody.GetComponent<CapsuleCollider>();
        //}
    }

    private void Update()
    {
        if (!isActive)
        {
            body.Sleep();
            return;
        }

        Vector3 pos = transform.position;
        pos += transform.forward * Time.deltaTime * movementSpeed;
        transform.position = pos;

        Ray feetRay = new Ray(gameObject.transform.position, Vector3.down);
        RaycastHit hit;
        isGrounded = Physics.Raycast(feetRay, out hit, maxDistanceToGround, floorLayerMask);

        if (isGrounded)
        {
            transform.forward = -hit.collider.transform.right; // weird due to floor nature
            Debug.DrawRay(transform.position, transform.forward);
        }

        if (playerActionType == PlayerActionType.Jump && transform.position.y <= LevelManager.Instance.LowestPlayerPositionY)
        {
            PlayerLost?.Invoke();
        }
    }

    private void OnActionKeyPressed()
    {
        if (myParticles != null)
        {
            myParticles.Play();
        }

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

    private void OnCollisionEnter(Collision collision)
    {
        if (playerActionType == PlayerActionType.Crouch && collision.gameObject.tag == maiaCollisionTag)
        {
            PlayerLost?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        if (other.gameObject.tag == "Goal")
        {
            MusicManager.Instance.PlayVictoryFx();
            PlayerArrived?.Invoke();
            ToggleEnabled();
        }
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
        // TODO: Uncomment to implement crouch
        //animator.SetTrigger(crouchAnimationTriggerName);
    }


    private void OnDestroy()
    {
        playerInput.ActionKeyPressed -= OnActionKeyPressed;
    }

    public void ToggleEnabled()
    {
        isActive = !isActive;
    }

    public void CrouchAnimation()
    {
        // TODO: check animation
        //// TODO: Uncomment to implement crouch
        //playerReference.GetComponent<MeshFilter>().mesh.MarkDynamic();
        //playerReference.GetComponent<MeshFilter>().mesh = animationMeshFilter.sharedMesh;
        //playerReference.GetComponent<CapsuleCollider>().height = animationCapsuleCollider.height;
        //playerReference.GetComponent<CapsuleCollider>().center = animationCapsuleCollider.center;

    }

    public void ResetAnimation()
    {
        // TODO: check animation
        //// TODO: Uncomment to implement crouch
        //playerReference.GetComponent<MeshFilter>().mesh.MarkDynamic();
        //playerReference.GetComponent<MeshFilter>().mesh = idleMeshFilter.sharedMesh;
        //playerReference.GetComponent<CapsuleCollider>().height = idleCapsuleCollider.height;
        //playerReference.GetComponent<CapsuleCollider>().center = idleCapsuleCollider.center;
    }

}
