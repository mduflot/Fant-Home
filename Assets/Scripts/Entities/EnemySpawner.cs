using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

namespace Entities
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _mob;
        [SerializeField] private int _enemyCount = 100;
        [SerializeField] private Transform[] _spawns;
        [SerializeField] private float _spawnRadius = 5;
        [SerializeField] private float _minSpeed = 0.75f;
        [SerializeField] private float _maxSpeed = 1.25f;
        
        private void Start()
        {
            for (int i = 0; i < _enemyCount; i++)
            {
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            int randomSpawnPosition = UnityEngine.Random.Range(0, _spawns.Length);
            float2 randomPosition2D = UnityEngine.Random.insideUnitCircle * _spawnRadius;
            float3 randomPosition3D = new(_spawns[randomSpawnPosition].position.x + randomPosition2D.x, 1.5f,
                _spawns[randomSpawnPosition].position.z + randomPosition2D.y);

            GameObject ghost = Pooler.instance.Pop("Ghost");
            ghost.transform.position = randomPosition3D;

        }
    }
}