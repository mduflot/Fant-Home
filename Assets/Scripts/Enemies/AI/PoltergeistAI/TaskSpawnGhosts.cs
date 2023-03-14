using BehaviorTree;
using Scriptables;
using Unity.Mathematics;
using UnityEngine;

namespace AI.PoltergeistAI
{
    public class TaskSpawnGhosts : Node
    {
        private Transform _transform;
        private int _numberToSpawn;
        private MonsterType _ghostKeyToSpawn;
        
        public TaskSpawnGhosts(Transform transform, int numberToSpawn, MonsterType ghostKeyToSpawn)
        {
            _transform = transform;
            _numberToSpawn = numberToSpawn;
            _ghostKeyToSpawn = ghostKeyToSpawn;
        }
        
        public override NodeState Evaluate()
        {
            
            for (int i = 0; i < _numberToSpawn; i++)
            {
                float2 randomPosition2D = UnityEngine.Random.insideUnitCircle * 4;
                float3 randomPosition3D = new(_transform.position.x + randomPosition2D.x, 1.23f,
                    _transform.position.z + randomPosition2D.y);
                GameObject ghost = Pooler.instance.Pop(_ghostKeyToSpawn.ToString());
                ghost.transform.position = randomPosition3D;
            }
            
            return NodeState.RUNNING;
        }
    }
}