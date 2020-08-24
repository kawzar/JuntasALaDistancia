using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class PlayerPathFollower : MonoBehaviour
{
    [SerializeField]
    private PathCreator pathCreator;

    [SerializeField]
    private EndOfPathInstruction endOfPathInstruction;

    [SerializeField]
    private float speed = 5;

    private float distanceTravelled;

    void Awake()
    {
        if (pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            pathCreator.pathUpdated += OnPathChanged;
        }
    }

    void Update()
    {
        if (pathCreator != null)
        {
            distanceTravelled += speed * Time.deltaTime;
            Vector3 newPosition = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            newPosition.y = transform.position.y;
            transform.position = newPosition;
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
        }
    }

    // If the path changes during the game, update the distance travelled so that the follower's position on the new path
    // is as close as possible to its position on the old path
    private void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }
}
