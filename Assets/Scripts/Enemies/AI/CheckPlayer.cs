using BehaviorTree;
using Unity.Mathematics;
using UnityEngine;

namespace AI
{
    public class CheckPlayer : Node
    {
        private Transform _transform;
        private Animator _animator;
        private GameObject[] _players;

        public CheckPlayer(Transform transform)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
        }

        public override NodeState Evaluate()
        {
            _players = GameObject.FindGameObjectsWithTag("Player");
            Transform target;
            float currentDistance;

            object t = GetData("target");
            if (t == null)
            {
                if (_players.Length > 0)
                {
                    if (!_players[0].activeSelf) return NodeState.FAILURE;
                    currentDistance = math.sqrt(math.lengthsq(_transform.position - _players[0].transform.position));
                    target = _players[0].transform;
                }
                else
                {
                    ClearData("target");
                    _state = NodeState.FAILURE;
                    return _state;
                }

                foreach (var player in _players)
                {
                    if (!(currentDistance > math.sqrt(math.lengthsq(_transform.position - player.transform.position))))
                        continue;
                    currentDistance = math.sqrt(math.lengthsq(_transform.position - player.transform.position));
                    target = player.transform;
                }

                Parent.Parent.SetData("target", target);
                _state = NodeState.SUCCESS;
                return _state;
            }
            else
            {
                _state = NodeState.SUCCESS;
                return _state;
            }
        }
    }
}