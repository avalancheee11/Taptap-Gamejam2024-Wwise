using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionDetectPlayer : FSMDecision

{
    [Header("Config")]
    // [SerializeField] private float range;
    [SerializeField] private MonsterStats_SO stats;
    [SerializeField] private LayerMask playerMask;
    private EnemyBrain enemy;

    private void Awake() {
        enemy = GetComponent<EnemyBrain>();
    }
    
    public override bool Decide()
    {
        return DetectPlayer();
    }

    private bool DetectPlayer(){
        // Use Physics.OverlapSphere for 3D detection
        Collider[] playerColliders = Physics.OverlapSphere(enemy.transform.position, stats.range, playerMask);

        if(playerColliders.Length > 0){
            // Assume first detected player is the target
            enemy.Player = playerColliders[0].transform;
            Debug.Log("发现玩家，开始追逐...");
            return true;
        }

        enemy.Player = null;
        return false;   
    }

//TODO：开发用的，记得comment掉  
   private void OnDrawGizmosSelected() {
    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(transform.position, stats.range);
   }
}
