using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class EnemyPatrol : Node
{
    private Transform _transform;
    private Transform[] _waypoints;

    private int currentWaypointIndex = 0;

    private float waitTime = 1f; // in seconds
    private float waitCounter = 0f;
    private bool waiting = false;

    private float waitForPlayer = 2f;
    private float counter = 0f;
    public EnemyPatrol(Transform transform, Transform[] waypoints)
    {
        _transform = transform;
        _waypoints = waypoints;
    }

    public override NodeState Evaluate()
    {
        if (!GameController.isPlayerTurn)
        {
            GameController.enemyCurrentState = "Patroling";
            counter += Time.deltaTime;//wait few secs to move 
            if(counter > waitForPlayer)
            {
                if (waiting)
                {
                    waitCounter += Time.deltaTime;
                    if (waitCounter >= waitTime)
                    {
                        waiting = false;    //wait if reach new waypoint
                    }
                }
                else
                {
                    
                    Transform wp = _waypoints[currentWaypointIndex];
                    if (Vector3.Distance(_transform.position, wp.position) < 0.01f)
                    {
                        _transform.position = wp.position;
                        waitCounter = 0f;
                        waiting = true;

                        currentWaypointIndex = (currentWaypointIndex + 1) % _waypoints.Length;
                        counter = 0;
                        GameController.ChangeTurn();
                    }
                    else
                    {
                        _transform.position = Vector3.MoveTowards(_transform.position, wp.position, EnemyBT.speed * Time.deltaTime);
                        _transform.LookAt(wp.position);
                    }
                }
            }
        }
        state = NodeState.RUNNING;
        return state;
    }

}
