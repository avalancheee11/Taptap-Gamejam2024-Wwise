// 保存状态在这里
using System;

[Serializable]
public class FSMState{ // AttrackState
    public string ID;
    public FSMAction[] Actions; // MoveAction | AttackAction | ... (each state can perform different actions)
    public FSMTransition[] Transitions; // (also each state can perform different transitions)

    public void UpdateState(EnemyBrain enemyBrain){

        ExecuteActions();
        ExecuteTransitions(enemyBrain);
    }

    private void ExecuteActions(){
        for (int i = 0; i < Actions.Length; i++)
        {
            Actions[i].Act(); 
        }
    }

    private void ExecuteTransitions(EnemyBrain enemyBrain) {
        if(Transitions == null || Transitions.Length <= 0) return;
        
        for (int i = 0;i<Transitions.Length;i++){
            bool value = Transitions[i].Decision.Decide();
            if (value){
                enemyBrain.ChangeState(Transitions[i].TrueState);
            }else
            {
                enemyBrain.ChangeState(Transitions[i].FalseState);
            }
        }
    }
}