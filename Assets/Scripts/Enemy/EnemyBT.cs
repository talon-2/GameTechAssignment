using System.Collections.Generic;
using BehaviorTree;

public class EnemyBT : Tree
{
    public UnityEngine.Transform[] waypoints;

    public static float speed = 5f;
    public static float fovRange = 6f;
    public static float walkDistance = 4f;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckHealth(transform),
            }),
            new Sequence(new List<Node>
            {
                new CheckEnemy(transform),
                new GoToPlayer(transform),
                new EnemyAttack(transform),
            }),
            new EnemyPatrol(transform, waypoints),
        });

        return root;
    }
}
