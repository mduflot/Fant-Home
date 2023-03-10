using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using Tree = BehaviorTree.Tree;

namespace AI.PoltergeistAI
{
    public class PoltergeistBT : Tree
    {
        public Transform[] Waypoints;

        public static float Speed = 2f;
        public static float FOVRange = 6f;
        public static float AttackRange = 1f;
        
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
                    new CheckPlayerInFOVRange(transform),
                    new TaskGoToTarget(transform),
                }),
                new TaskPatrol(transform, Waypoints),
            });

            return root;
        }
    }
}