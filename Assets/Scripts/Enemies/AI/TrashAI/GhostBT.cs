using System.Collections.Generic;
using BehaviorTree;
using Scriptables;
using UnityEngine;
using Tree = BehaviorTree.Tree;

namespace AI.GhostAI
{
    public class GhostBT : Tree
    {
        [SerializeField] private LayerMask _enemiesMask;

        private GhostStatsSO _ghostStatsSO;

        protected override Node SetupTree()
        {
            _ghostStatsSO = GetComponent<Ghost>()._ghostSO;
            Node root = new Selector(new List<Node>
            {
                new CheckStun(transform),
                new Sequence(new List<Node>
                {
                    new CheckPlayerInAttackRange(transform, _ghostStatsSO.AttackRange, _ghostStatsSO.AttackCD),
                    new TaskAttack(transform, _ghostStatsSO.AttackDamage, _ghostStatsSO.AttackRadius,
                        _ghostStatsSO.AttackKey, _ghostStatsSO.AttackDelayBeforeAttack),
                }),
                new Sequence(new List<Node>
                {
                    new CheckPlayer(transform),
                    new TaskGoToTarget(transform, GetComponent<MeshRenderer>(), _enemiesMask, _ghostStatsSO.MoveSpeed,
                        _ghostStatsSO.AttackRange, _ghostStatsSO.RangeVisibleToPlayer, _ghostStatsSO.MaxHealth, _ghostStatsSO.MaxVeil),
                })
            });

            return root;
        }
    }
}