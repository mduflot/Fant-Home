using System.Collections.Generic;
using System.Linq;
using BehaviorTree;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    public class CheckPlayer : Node
    {
        private Transform _transform;
        private Animator _animator;
        private Ghost _ghostComponent;
        private GameObject[] _playersArray;
        private List<GameObject> _players;

        public CheckPlayer(Transform transform)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
            _ghostComponent = transform.GetComponent<Ghost>();
        }

        public override NodeState Evaluate()
        {
            _playersArray = GameObject.FindGameObjectsWithTag("Player");
            _players = _playersArray.ToList();
            List<GameObject> _playersToRemove = new List<GameObject>();
            Transform target;

            object t = GetData("target");
            if (t == null)
            {
                foreach (var player in _players.Where(player => player.GetComponent<PlayerHealth>().curHealth <= 0))
                {
                    _playersToRemove.Add(player);
                }

                foreach (var player in _playersToRemove)
                {
                    _players.Remove(player);
                }
                
                if (_players.Count < 1)
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
                    if (_players.Count == 1)
                    {
                        Parent.Parent.SetData("target", target);
                        _state = NodeState.SUCCESS;
                        return _state;
                    }

                    var random = Random.Range(0f, 1.01f);
                    if (random < 0.5f)
                    {
                        Parent.Parent.SetData("target", target);
                        _state = NodeState.SUCCESS;
                        return _state;
                    }

                    var randomPlayer = Random.Range(0, _players.Count);
                    target = _players[randomPlayer].transform;
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