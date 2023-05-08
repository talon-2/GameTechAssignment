using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckHealth : Node
{
    private Transform _transform;
    private float waitForPlayer = 2f;
    private float counter = 0f;

    public CheckHealth(Transform transform) 
    { 
        _transform = transform; 
    }
    public override NodeState Evaluate()
    {
        if (!GameController.isPlayerTurn)
        {
            counter += Time.deltaTime;
            if (counter > waitForPlayer)
            {
                Debug.Log("Checking Health");
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
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}
