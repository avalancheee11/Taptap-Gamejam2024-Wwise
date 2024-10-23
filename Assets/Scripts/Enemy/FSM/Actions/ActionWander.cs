

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionWander : FSMAction
{
    // Configuration for wandering
    [Header("Config")]
    [SerializeField] private MonsterStats_SO stats;

    // [SerializeField] private float speed;          // Speed of movement
    [SerializeField] private float wanderTime = 1f;     // Time interval to change direction
    [SerializeField] private Vector2 moveRange;    // XZ plane movement range (Vector2 to limit X and Z, no Y)
    // [SerializeField] private float patrolRadius = 5f;   // Radius for patrol area

    private Vector3 movePosition;  // The next destination for wandering
    private Vector3 initialPosition; // Original spawn position
    private float timer;           // Timer to manage when to get a new destination

    private void Start() {
        // initialPosition = transform.position;  // Set the initial spawn position
        initialPosition = stats.initialPosition;  // Set the initial spawn position
        GetNewDestination();  // Set the initial random destination
        timer = wanderTime;   // Initialize timer
    }

    public override void Act() {
        timer -= Time.deltaTime;  // Decrease the timer

        // Calculate the direction and movement (on XZ plane, Y remains the same)
        Vector3 moveDirection = (movePosition - transform.position).normalized;
        Vector3 movement = new Vector3(moveDirection.x, 0, moveDirection.z) * Time.deltaTime * stats.speed;

        // Move the entity if it's not yet near the destination (XZ plane movement only)
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(movePosition.x, 0, movePosition.z)) >= 0.5f) {
            transform.Translate(movement, Space.World);
        }

        // If the wander time has elapsed, get a new random destination
        if (timer <= 0) {
            GetNewDestination();
            timer = wanderTime;
        }
    }

    // private void GetNewDestination() {
    //     // Generate a random position within the movement range (on the XZ plane, Y remains fixed)
    //     float randomX = Random.Range(-moveRange.x, moveRange.x);
    //     float randomZ = Random.Range(-moveRange.y, moveRange.y);

    //     // Y position stays the same as the current position, only change X and Z
    //     movePosition = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    // }

    private void GetNewDestination() {
        // Generate a random position within the patrol radius from the initialPosition
        float randomAngle = Random.Range(0f, Mathf.PI * 2); // Random angle for circle
        float randomRadius = Random.Range(0f, stats.patrolRadius); // Random distance within radius

        // Convert polar coordinates to cartesian for movement on XZ plane
        float offsetX = Mathf.Cos(randomAngle) * randomRadius;
        float offsetZ = Mathf.Sin(randomAngle) * randomRadius;

        // Set move position while keeping Y constant
        movePosition = new Vector3(initialPosition.x + offsetX, initialPosition.y, initialPosition.z + offsetZ);
    }


    // Debugging: Draw the wandering range and destination in the XZ plane
    // private void OnDrawGizmosSelected() {
    //     if (moveRange != Vector2.zero) {
    //         Gizmos.color = Color.cyan;
    //         // Draw a wireframe box showing the movement range in the XZ plane
    //         Gizmos.DrawWireCube(transform.position, new Vector3(moveRange.x * 2, 0, moveRange.y * 2));
    //         // Draw a line from the current position to the move position
    //         Gizmos.DrawLine(transform.position, movePosition);
    //     }
    // }
        // Debugging: Draw the patrol range and destination in the XZ plane
    private void OnDrawGizmosSelected() {
        if (stats.patrolRadius > 0f) {
            Gizmos.color = Color.cyan;
            // Draw a wireframe sphere showing the movement range in the XZ plane
            Gizmos.DrawWireSphere(initialPosition, stats.patrolRadius);
            // Draw a line from the current position to the move position
            Gizmos.DrawLine(transform.position, movePosition);
        }
    }
}
