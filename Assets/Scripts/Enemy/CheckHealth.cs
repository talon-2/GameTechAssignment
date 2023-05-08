using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckHealth : Node
{
    private Transform _transform;
    private Vector3 previousPosition;
    private bool reached = true;
    private float counter = 0f;
    private float waitForPlayer = 2f;

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
                if (reached)
                {
                    previousPosition = _transform.position;
                    reached = false;
                }
                Transform target = (Transform)GetData("Player");

                if(GameController.difficulty == "Easy")
                {
                    if (GameController.enemyHealthAmt == 1)
                    {
                        if (Vector3.Distance(previousPosition, _transform.position) < EnemyBT.walkDistance)
                        {
                            GameController.enemyCurrentState = "Running";
                            Debug.Log("tset");
                            //if health is low, 
                            Vector3 dirToPlayer = _transform.position - target.transform.position;

                            Vector3 newPos = _transform.position + dirToPlayer;

                            _transform.position = Vector3.MoveTowards(_transform.position, newPos, EnemyBT.walkDistance * Time.deltaTime);

                        }
                        else
                        {
                            reached = true;
                            GameController.ChangeTurn();
                        }
                    }
                    else
                    {
                        state = NodeState.FAILURE;
                        return state;
                    }
                }
                else{//hard mode
                    
                }
            }
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
