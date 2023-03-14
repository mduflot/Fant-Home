using System.Collections.Generic;
using System.Linq;
using BehaviorTree;
using Unity.Mathematics;
using UnityEngine;

namespace AI.PoltergeistAI
{
    public class TaskFleeing : Node
    {
        private Transform _transform;
        private Animator _animator;
        private List<Room> _roomWaypoints;
        private List<Room> _roomToRemove;
        private float _speed;

        public TaskFleeing(Transform transform, Room[] roomWaypoints, float speed)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
            _roomWaypoints = roomWaypoints.ToList();
            _roomToRemove = new List<Room>();
            _speed = speed;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");

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

            if (_roomWaypoints.Count != 1 && _roomWaypoints.Count < 1)
            {
                if (_roomWaypoints.Contains(target.GetComponent<Player>().curRoom))
                    _roomWaypoints.Remove(target.GetComponent<Player>().curRoom);
            }

            Transform wp;
            Vector3 wpPos = Vector3.zero;
            Vector3 waypointPos = Vector3.zero;
            float previousDistance = 0;
            Vector3 fleeingPos = Vector3.zero;

            foreach (var room in _roomWaypoints)
            {
                if (room.Floors.Count == 1)
                {
                    wp = room.Floors[0].transform;
                    waypointPos = new Vector3(wp.position.x, 1.23f, wp.position.z);
                }
                else
                {
                    foreach (var floor in room.Floors)
                    {
                        wpPos += floor.transform.position;
                    }

                    wpPos /= room.Floors.Count;
                    waypointPos = new Vector3(wpPos.x, 1.23f, wpPos.z);
                }

                float currentDistance = math.sqrt(math.lengthsq(_transform.position - waypointPos));
                if (currentDistance < previousDistance || previousDistance == 0)
                {
                    previousDistance = currentDistance;
                    fleeingPos = waypointPos;
                }
            }

            _transform.position =
                Vector3.MoveTowards(_transform.position, fleeingPos, _speed * Time.deltaTime);
            if (Vector3.Distance(_transform.position, fleeingPos) < 1f)
                _transform.GetComponent<Ghost>().IsFleeing = false;
            _transform.LookAt(fleeingPos);

            _state = NodeState.RUNNING;
            return _state;
        }
    }
}