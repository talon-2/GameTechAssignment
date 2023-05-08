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


    string heavy = "Heavy";
    string light = "Light";
    string melee = "Melee";
    string range = "Range";

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
                
                Debug.Log(Vector3.Distance(_transform.position, previousPosition));
                if (GameController.difficulty == "Easy")
                {
                    
                    if (GameController.enemyHealthAmt <= 1)     //if enemy at one health, just run
                    {
                        if (Vector3.Distance(previousPosition, _transform.position) < EnemyBT.walkDistance)      //walk until near walk distance
                        {

                            GameController.enemyCurrentState = "Running";
                            //if health is low, 
                            Vector3 dirToPlayer = _transform.position - GameController.player.transform.position;

                            Vector3 newPos = _transform.position + dirToPlayer;

                            _transform.position = Vector3.MoveTowards(_transform.position, newPos, EnemyBT.walkDistance * Time.deltaTime);
                            _transform.LookAt(newPos);

                            //Debug.Log(previousPosition);
                        }
                        else
                        {
                            Debug.Log("Test2");
                            reached = true;
                            counter = 0;
                            GameController.ChangeTurn();
                        }
                    }
                    else
                    {
                        reached = true;
                        Debug.Log("test");
                        state = NodeState.FAILURE;
                        return state;
                    }

                }
                else{//hard mode

                    //if enemy at one health
                    if (GameController.enemyHealthAmt <= 1)
                    {
                        //if enemy weapon is range and player weapon is melee, run away
                        // if enemy type is light and enemy weapon is melee, run away
                        //rest of conditions set to FAILURE to skip.
                        
                    }
                    state = NodeState.FAILURE;
                    return state;


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
