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
    private float walkDistance;

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
                Transform target = (Transform)GetData("Player");

                if(GameController.enemyTyping == "Heavy")
                {
                    walkDistance = 4f;
                }
                else
                {
                    walkDistance = 6f;
                }


                if (Vector3.Distance(_transform.position, GameController.player.transform.position) > 1.5f)
                {
                    _transform.position = Vector3.MoveTowards( _transform.position, GameController.player.transform.position, EnemyBT.speed * Time.deltaTime);
                    _transform.LookAt(GameController.player.transform.position);
                }
                else if (Vector3.Distance(_transform.position, GameController.player.transform.position) < 1.5f)
                {
                    GameController.ChangeTurn();
                }
            }
        }
        state = NodeState.RUNNING;
        return state;
    }

}
