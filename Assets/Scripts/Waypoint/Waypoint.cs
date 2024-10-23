// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Waypoint : MonoBehaviour
// {
//     [Header("Config")]
//     [SerializeField] private Vector3[] points;
//     public Vector3[] Points => points;
//     public Vector3 EntityPosition { get; set; }
//     private bool gameStarted;

//     private void Start() {
//         EntityPosition = transform.position;
//         gameStarted = true;
//     }

//     public Vector3 GetPosition(int pointIndex){
//         return EntityPosition + points[pointIndex];  
//     }
//     private void OnDrawGizmos() {
//         if(gameStarted == false && transform.hasChanged){
//             EntityPosition = transform.position;
//         }
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Header("Config: Please ensure Y = 0")]
    [SerializeField] private Vector3[] points;  // Waypoints in local space
    public Vector3[] Points => points;  // Expose points publicly
    public Vector3 EntityPosition { get; set; }
    private bool gameStarted;

    private void Start() {
        // Set the entity's starting position to its current position
        EntityPosition = transform.position;
        gameStarted = true;

    }

    public Vector3 GetPosition(int pointIndex){
        // Force the Y position to be 0
        //因为是2.5D，所以Y要设为0
        Vector3 point = EntityPosition + points[pointIndex];
        point.y = 0;  // Set Y to 0
        return point;

    }

    private void OnDrawGizmos() {
        if(gameStarted == false && transform.hasChanged){
            EntityPosition = transform.position;
        }
    }


}
