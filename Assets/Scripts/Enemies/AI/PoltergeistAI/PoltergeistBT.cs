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
        [SerializeField] private LayerMask _enemiesMask;

        private PoltergeistStatsSO _poltergeistStatsSO;
        private Transform[] _waypoints;

        protected override Node SetupTree()
        {
            _poltergeistStatsSO = (PoltergeistStatsSO)GetComponent<Ghost>()._ghostSO;
            _waypoints = WaypointsManager.Instance.GetWaypoints();

            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new CheckPlayerInAttackRange(transform, _poltergeistStatsSO.AttackRange, _poltergeistStatsSO.AttackCD),
                    new TaskAttack(transform, _poltergeistStatsSO.AttackDamage, _poltergeistStatsSO.AttackRadius,
                        _poltergeistStatsSO.AttackKey, _poltergeistStatsSO.AttackDelayBeforeAttack),
                }),
                new Sequence(new List<Node>
                {
                    new CheckPlayerInFOVRange(transform, _poltergeistStatsSO.DetectionRange),
                    new TaskGoToTarget(transform, _enemiesMask, _poltergeistStatsSO.MoveSpeed, _poltergeistStatsSO.AttackRange),
                }),
                new TaskPatrol(transform, _waypoints, _poltergeistStatsSO.MoveSpeed),
            });

            return root;
        }
    }
}