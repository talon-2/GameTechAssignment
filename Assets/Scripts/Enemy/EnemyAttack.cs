using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class EnemyAttack : Node
{
    private Transform _transform;
    private float counter = 0f;
    private float waitForPlayer = 2f;
    public static float enemyHitDistance;

    string range = "Range";
    string easy = "Easy";

    public EnemyAttack(Transform tranform)
    {
        _transform = tranform;
    }

    public override NodeState Evaluate()
    {
        if (!GameController.isPlayerTurn)
        {
            counter += Time.deltaTime;
            if (counter > waitForPlayer)
            {
                Transform target = (Transform)GetData("Player");

                if (GameController.difficulty == easy)//make sure enemy can only walk certain distance, hard mode will increase the numbers
                {
                    if (GameController.enemyWeapon == range)
                    {
                        enemyHitDistance = 3f;
                    }
                    else
                    {
                        enemyHitDistance = 1.5f;
                    }
                }
                else
                {
                    if (GameController.enemyWeapon == range)        //maintain distance while attacking4
                    {
                        enemyHitDistance = 5f;
                    }
                    else
                    {
                        enemyHitDistance = 2.5f;
                    }
                }
                float distance = Vector3.Distance(_transform.transform.position, GameController.player.transform.position);
                if (distance <= enemyHitDistance)
                {
                    _transform.LookAt(GameController.player.transform.position);
                    GameController.enemyCurrentState = "Attacking";
                    GameController.playerHealthAmt -= GameController.damage;
                    counter = 0f;
                    if (GameController.playerHealthAmt <= 0f)
                    {
                        ClearData("Player");
                        state = NodeState.RUNNING;
                        return state;
                    }
                    GameController.ChangeTurn();
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
