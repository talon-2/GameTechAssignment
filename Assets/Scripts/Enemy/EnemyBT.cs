using System.Collections.Generic;
using BehaviorTree;

public class EnemyBT : Tree
{
    public UnityEngine.Transform[] waypoints;

    public static float speed = 4f;
    public static float fovRange;
    public static float walkDistance;

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
            }),
            new EnemyPatrol(transform, waypoints),
        });

        return root;
    }
}
