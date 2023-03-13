using System.Collections.Generic;
using BehaviorTree;
using Scriptables;
using Tree = BehaviorTree.Tree;

namespace AI.GhostAI
{
    public class GhostBT : Tree
    {
        public static float Speed;
        public static float AttackRange;
        public static float AttackTime;

        private GhostStatsSO _ghostStatsSO;

        protected override Node SetupTree()
        {
            _ghostStatsSO = GetComponent<Ghost>()._ghostSO;
            Speed = _ghostStatsSO.MoveSpeed;
            AttackRange = _ghostStatsSO.AttackRange;
            AttackTime = _ghostStatsSO.AttackCD;
            
            Node root = new Selector(new List<Node>
            {
                new CheckStun(transform),
                new Sequence(new List<Node>
                {
                    new CheckPlayerInAttackRange(transform),
                    new TaskAttack(transform, AttackTime),
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