using System;
using System.Collections.Generic;
using System.Linq;
using BehaviorTree;
using UnityEngine;

namespace AI.PoltergeistAI
{
    public class TaskPatrol : Node
    {
        private Transform _transform;
        private Animator _animator;
        private List<Room> _roomWaypoints;
        private List<Room> _roomToRemove;

        private int _currentWaypointIndex;

        private float _waitTime = 1f;
        private float _waitCounter;
        private bool _waiting;
        private float _speed;

        public TaskPatrol(Transform transform, Room[] roomWaypoints, float speed)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
            _roomWaypoints = roomWaypoints.ToList();
            _roomToRemove = new List<Room>();
            _speed = speed;
        }

        public override NodeState Evaluate()
        {
            _roomToRemove.Clear();
            foreach (var room in _roomWaypoints)
            {
                if (room.IsLocked)
                {
                    _roomToRemove.Add(room);
                }
            }

            foreach (var room in _roomToRemove)
            {
                _roomWaypoints.Remove(room);
            }
            
            if (_waiting)
            {
                _waitCounter += Time.deltaTime;
                if (_waitCounter >= _waitTime)
                {
                    _waiting = false;
                    // _animator.SetBool("Walking", true);
                }
            }
            else
            {
                Transform wp;
                Vector3 wpPos = Vector3.zero;
                Vector3 waypointPos = Vector3.zero;
                if (_roomWaypoints[_currentWaypointIndex].Floors.Count < 1)
                {
                    throw new NullReferenceException();
                }
                if (_roomWaypoints[_currentWaypointIndex].Floors.Count == 1)
                {
                    wp = _roomWaypoints[_currentWaypointIndex].Floors[0].transform;
                    waypointPos = new Vector3(wp.position.x, 1.23f, wp.position.z);
                }
                else
                {
                    foreach (var floor in _roomWaypoints[_currentWaypointIndex].Floors)
                    {
                        wpPos += floor.transform.position;
                    }

                    wpPos /= _roomWaypoints[_currentWaypointIndex].Floors.Count;
                    waypointPos = new Vector3(wpPos.x, 1.23f, wpPos.z);
                }
                
                if (Vector3.Distance(_transform.position, waypointPos) < 1f)
                {
                    _transform.position = waypointPos;
                    _waitCounter = 0f;
                    _waiting = true;

                    _currentWaypointIndex = (_currentWaypointIndex + 1) % _roomWaypoints.Count;
                    // _animator.SetBool("Walking", false);
                }
                else
                {
                    _transform.position =
                        Vector3.MoveTowards(_transform.position, waypointPos, _speed * Time.deltaTime);
                    _transform.LookAt(waypointPos);
                }
            }
            _state = NodeState.RUNNING;
            return _state;
        }
    }
}