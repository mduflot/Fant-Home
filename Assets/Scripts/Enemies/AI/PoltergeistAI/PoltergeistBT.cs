using System.Collections.Generic;
using AI.GhostAI;
using BehaviorTree;
using Scriptables;
using UnityEngine;
using Tree = BehaviorTree.Tree;

namespace AI.PoltergeistAI
{
    public class PoltergeistBT : Tree
    {
        public Transform[] Waypoints;
        
        [SerializeField] private LayerMask _enemiesMask;
        
        private PoltergeistStatsSO _poltergeistStatsSO;

        protected override Node SetupTree()
        {
            _poltergeistStatsSO = (PoltergeistStatsSO)GetComponent<Ghost>()._ghostSO;
            
            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new CheckPlayerInAttackRange(transform, _poltergeistStatsSO.AttackRange),
                    new TaskAttack(transform, _poltergeistStatsSO.AttackCD),
                }),
                new Sequence(new List<Node>
                {
                    new CheckPlayerInFOVRange(transform, _poltergeistStatsSO.DetectionRange),
                    new TaskGoToTarget(transform, _enemiesMask, _poltergeistStatsSO.MoveSpeed),
                }),
                new TaskPatrol(transform, Waypoints, _poltergeistStatsSO.MoveSpeed),
            });

            return root;
        }
    }
}