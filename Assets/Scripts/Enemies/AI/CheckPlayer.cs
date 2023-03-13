using System.Collections.Generic;
using BehaviorTree;
using Unity.Mathematics;
using UnityEngine;

namespace AI
{
    public class CheckPlayer : Node
    {
        private Transform _transform;
        private Animator _animator;
        private Ghost _ghostComponent;
        private GameObject[] _players;

        public CheckPlayer(Transform transform)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
            _ghostComponent = transform.GetComponent<Ghost>();
        }

        public override NodeState Evaluate()
        {
            _players = GameObject.FindGameObjectsWithTag("Player");
            Transform target;

            object t = GetData("target");
            if (t == null)
            {
                if (_players.Length < 1)
                {
                    ClearData("target");
                    _state = NodeState.FAILURE;
                    return _state;
                }
                
                float currentDistance = math.sqrt(math.lengthsq(_transform.position - _players[0].transform.position));
                target = _players[0].transform;

                List<GameObject> playersInSameRoom = new();

                if (Physics.Raycast(_transform.position, -Vector3.up, out var hit, 10.0f))
                {
                    Debug.DrawRay(_transform.position, -Vector3.up * hit.distance, Color.red);
                    if (hit.transform.gameObject.CompareTag("Floor"))
                    {
                        foreach (var room in RoomsManager.Instance.rooms)
                        {
                            if (room.Floors.Contains(hit.transform.gameObject))
                            {
                                _ghostComponent.CurRoom = room;
                            }
                        }
                    }
                }
                
                foreach (var player in _players)
                {
                    if (player.GetComponent<Player>().curRoom == _ghostComponent.CurRoom)
                    {
                        playersInSameRoom.Add(player);
                    }
                    
                    if (!(currentDistance > math.sqrt(math.lengthsq(_transform.position - player.transform.position))))
                        continue;
                    currentDistance = math.sqrt(math.lengthsq(_transform.position - player.transform.position));
                    target = player.transform;
                }

                if (playersInSameRoom.Count < 1)
                {
                    Parent.Parent.SetData("target", target);
                    _state = NodeState.SUCCESS;
                    return _state;
                }
                
                currentDistance = math.sqrt(math.lengthsq(_transform.position - playersInSameRoom[0].transform.position));
                target = playersInSameRoom[0].transform;
                
                foreach (var player in playersInSameRoom)
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

            target = (Transform)t;
            if (target.GetComponent<PlayerHealth>().curHealth <= 0)
            {
                _state = NodeState.FAILURE;
                return _state;
            }
            
            _state = NodeState.SUCCESS;
            return _state;
        }
    }
}