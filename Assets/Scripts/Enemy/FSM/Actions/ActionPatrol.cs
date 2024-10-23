
using UnityEngine;

public class ActionPatrol : FSMAction
{
    [Header("Config")]
    [SerializeField] private MonsterStats_SO stats;
    // [SerializeField] private float speed;

    private Waypoint waypoint;
    private int pointIndex;
    private Vector3 nextPosition;

    private void Awake(){
        transform.position = stats.initialPosition; //使怪物初始位置在配置表设置的位置
        waypoint = GetComponent<Waypoint>();
    }

    public override void Act()
    {
        FollowPath();
    }

    private void FollowPath(){
        // Move towards the current waypoint
        Vector3 targetPosition = GetCurrentPosition();
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, stats.speed * Time.deltaTime);

        // Check if the enemy is close enough to the current waypoint
        if(Vector3.Distance(transform.position, targetPosition) <= 0.1f){
            UpdateNextPosition();
        }
    }

    private void UpdateNextPosition(){
        // Move to the next waypoint
        pointIndex++;
        if(pointIndex >= waypoint.Points.Length){
            pointIndex = 0;  // Loop back to the first waypoint
        }
    }

    private Vector3 GetCurrentPosition(){
        // Get the world position of the current waypoint
        return waypoint.GetPosition(pointIndex);
    }
}
