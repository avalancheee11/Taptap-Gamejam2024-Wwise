using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionAttackRange : FSMDecision
{
    [Header("Config")]
    // [SerializeField] private float AttackRange;
    [SerializeField] private MonsterStats_SO stats;
    [SerializeField] private LayerMask playerMask;
    private EnemyBrain enemy;

    private void Awake() {
        enemy = GetComponent<EnemyBrain>();
    }

    public override bool Decide() {
        return PlayerInAttackRange();
    }

    // if our playe can be attacked/player is within attack range
    private bool PlayerInAttackRange(){
        if(enemy.Player == null) return false;

        // Use Physics.OverlapSphere for 3D detection
        Collider[] playerColliders = Physics.OverlapSphere(enemy.transform.position, stats.attackRange, playerMask);

        if(playerColliders.Length > 0){
            Debug.Log("Attacking 正在攻击玩家!");
            return true;
        }

        return false;
    }
///TODO： 开发时候用于辅助的线条，上线前记得删掉！
    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stats.attackRange);
    }
}
