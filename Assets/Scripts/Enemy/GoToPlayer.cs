using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class GoToPlayer : Node
{
    private Transform _transform;
    private float waitForPlayer = 2f;
    private float counter = 0f;
    private Vector3 previousPosition;
    private bool reached = true;
    public static float enemyHitDistance;

    string heavy = "Heavy";
    string range = "Range";
    string easy = "Easy";

    public GoToPlayer(Transform transform)
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
                if (GameController.difficulty == easy)//make sure enemy can only walk certain distance, hard mode will increase the numbers
                {
                    if (GameController.enemyTyping == heavy)        //add more health
                        EnemyBT.walkDistance = 2f;
                        
                    else
                        EnemyBT.walkDistance = 4f;
                    if (GameController.enemyWeapon == range)        
                        enemyHitDistance = 3f;
                    else
                        enemyHitDistance = 1.5f;
                }
                else
                {
                    if (GameController.enemyTyping == heavy)
                        EnemyBT.walkDistance = 4f;
                    else
                        EnemyBT.walkDistance = 6f;
                    if (GameController.enemyWeapon == range)        //maintain distance while attacking
                        enemyHitDistance = 5f;
                    else
                        enemyHitDistance = 2.5f;
                }
                //make sure the AI only goes in attack distance
                //maintain distance
                //if can attack, stop walking towards player

                if (Vector3.Distance(previousPosition, _transform.position) < EnemyBT.walkDistance)      //walk until near walk distance
                {
                    if (Vector3.Distance(_transform.position, GameController.player.transform.position) > enemyHitDistance)      //walk towards target
                    {
                        GameController.enemyCurrentState = "Chasing";
                        if (!GameController.damaged)
                        {
                            _transform.position = Vector3.MoveTowards(_transform.position, GameController.player.transform.position, EnemyBT.speed * Time.deltaTime);
                            _transform.LookAt(GameController.player.transform.position);
                        }
                    }
                    else if (Vector3.Distance(_transform.position, GameController.player.transform.position) < enemyHitDistance)
                    {
                        counter = 0;
                        GameController.ChangeTurn();
                    }
                }
                else
                {
                    reached = true;
                    counter = 0;
                    GameController.ChangeTurn();
                }
            }
        }
        state = NodeState.RUNNING;
        return state;
    }

}




