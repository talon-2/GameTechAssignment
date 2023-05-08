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
    private float newWalkableDistance = 0f;
    private float distance = 0f;
    Vector3 dirToPlayer, newPos;
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
                    if (GameController.enemyTyping == heavy)
                    {
                        EnemyBT.walkDistance = 2f;
                    }
                    else
                    {
                        EnemyBT.walkDistance = 4f;
                    }

                    if (Vector3.Distance(previousPosition, _transform.position) < EnemyBT.walkDistance)      //walk until near walk distance
                    {
                        if (Vector3.Distance(_transform.position, GameController.player.transform.position) > EnemyAttack.enemyHitDistance)      //keep walk towards target
                        {
                            GameController.enemyCurrentState = "Chasing";
                            _transform.position = Vector3.MoveTowards(_transform.position, GameController.player.transform.position, EnemyBT.speed * Time.deltaTime);
                            _transform.LookAt(GameController.player.transform.position);
                        }
                        else
                        {
                            counter = 0;
                            state = NodeState.RUNNING;
                            return state;
                        }
                    }
                    else
                    {
                        reached = true;
                        counter = 0;
                        GameController.ChangeTurn();
                    }
                }
                else
                {
                    if (GameController.enemyTyping == heavy)
                    {
                        EnemyBT.walkDistance = 4f;
                    }
                    else
                    {
                        EnemyBT.walkDistance = 6f;
                    }
                    if (GameController.enemyCurrentState == "Attacking")
                    {
                        distance = Vector3.Distance(_transform.position, GameController.player.transform.position);//maintain range from player
                        if (distance < EnemyAttack.enemyHitDistance)
                        {
                            newWalkableDistance = EnemyAttack.enemyHitDistance - distance;
                            dirToPlayer = _transform.position - GameController.player.transform.position;
                            newPos = _transform.position + dirToPlayer;
                            _transform.position = Vector3.MoveTowards(_transform.position, newPos, newWalkableDistance * Time.deltaTime);
                            _transform.LookAt(newPos);
                        }
                        else
                        {
                            if (Vector3.Distance(previousPosition, _transform.position) < EnemyBT.walkDistance)      //if player walk away
                            {
                                if (Vector3.Distance(_transform.position, GameController.player.transform.position) > EnemyAttack.enemyHitDistance)      //keep walk towards target
                                {
                                    GameController.enemyCurrentState = "Chasing";
                                    _transform.position = Vector3.MoveTowards(_transform.position, GameController.player.transform.position, EnemyBT.speed * Time.deltaTime);
                                    _transform.LookAt(GameController.player.transform.position);
                                }
                                else
                                {
                                    counter = 0;
                                    state = NodeState.RUNNING;
                                    return state;
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
                    else
                    {
                        if (Vector3.Distance(previousPosition, _transform.position) < EnemyBT.walkDistance)      //walk until near walk distance
                        {
                            if (Vector3.Distance(_transform.position, GameController.player.transform.position) > EnemyAttack.enemyHitDistance)      //keep walk towards target
                            {
                                GameController.enemyCurrentState = "Chasing";
                                _transform.position = Vector3.MoveTowards(_transform.position, GameController.player.transform.position, EnemyBT.speed * Time.deltaTime);
                                _transform.LookAt(GameController.player.transform.position);
                            }
                            else
                            {
                                counter = 0;
                                state = NodeState.RUNNING;
                                return state;
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

            }
        }
        state = NodeState.RUNNING;
        return state;
    }

}




