using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckHealth : Node
{
    private Transform _transform;

    public CheckHealth(Transform transform) 
    { 
        _transform = transform; 
    }
    public override NodeState Evaluate()
    {
        
        Transform target = (Transform)GetData("Player");
        if (GameController.enemyHealthAmt == 1)
        {
                    //if health is low, run away to cover
                    //if player health is low ignore this node
        }
        else
        {
            state = NodeState.FAILURE;
            return state;
        } 
        state = NodeState.RUNNING;
        return state;
    }
}
