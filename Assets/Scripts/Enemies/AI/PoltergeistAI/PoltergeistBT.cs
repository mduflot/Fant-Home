using System.Collections.Generic;
using AI.GhostAI;
using BehaviorTree;
using UnityEngine;
using Tree = BehaviorTree.Tree;

namespace AI.PoltergeistAI
{
    public class PoltergeistBT : Tree
    {
        public Transform[] Waypoints;

        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _fovRange = 6f;
        [SerializeField] private float _attackRange = 4f;
        [SerializeField] private float _attackTime = 2f;

        [SerializeField] private LayerMask _enemiesMask;
        
        public static float Speed;
        public static float FOVRange;
        public static float AttackRange;
        
        protected override Node SetupTree()
        {
            Speed = _speed;
            FOVRange = _fovRange;
            AttackRange = _attackRange;
            
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
                    new TaskGoToTarget(transform, _enemiesMask),
                }),
                new TaskPatrol(transform, Waypoints),
            });

            return root;
        }
    }
}