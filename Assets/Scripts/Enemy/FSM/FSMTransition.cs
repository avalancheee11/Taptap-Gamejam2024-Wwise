using UnityEngine;
using System;

[Serializable]
public class FSMTransition  {
    public FSMDecision Decision; // PlayerInRangeOfAtack -> true or False
    public string TrueState; // CurrentState -> AttackState
    public string FalseState; // CurrentState -> PatrolState
    
}