using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using Tree = BehaviorTree.Tree;

namespace AI.GhostAI
{
    public class GhostBT : Tree
    {
        [SerializeField] float _speed = 2f;
        [SerializeField] private float _fovRange = 6f;
        [SerializeField] private float _attackRange = 4f;
        [SerializeField] private float _attackTime = 2f;

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
                new CheckStun(transform),
                new Sequence(new List<Node>
                {
                    new CheckPlayerInAttackRange(transform),
                    new TaskAttack(transform, _attackTime),
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