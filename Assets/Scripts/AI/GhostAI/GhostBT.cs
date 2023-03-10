using System.Collections.Generic;
using BehaviorTree;
using AI;
using UnityEngine;
using Tree = BehaviorTree.Tree;

namespace AI.GhostAI
{
    public class GhostBT : Tree
    {
        public Transform[] Waypoints;
        
        public static float Speed = 2f;
        public static float FOVRange = 6f;
        public static float AttackRange = 4f;

        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new CheckPlayerInAttackRange(transform),
                    new TaskAttack(transform),
                }),
                new Sequence(new List<Node>
                {
                    new CheckPlayer(transform),
                    new TaskGoToTarget(transform),
                })
            });

            return root;
        }
    }
}