using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckEnemy : Node
{
    private static int _enemyLayerMask = 1 << 6;

    private Transform _transform;
    string easy = "Easy";

    public CheckEnemy(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        object t = GetData("Player");
        if(GameController.difficulty == easy)
        {
            EnemyBT.fovRange = 6f;
        }
        else
        {
            EnemyBT.fovRange = 10f;
        }
        if (t == null)
        {
            
            float distance = Vector3.Distance(_transform.position, GameController.player.transform.position);
            
            if (distance < EnemyBT.fovRange)
            {
                parent.parent.SetData("Player", GameController.player.transform);
                state = NodeState.SUCCESS;
                return state;
            }
            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }

}
