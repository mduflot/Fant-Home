using UnityEngine;
using Unity.Mathematics;

namespace Entities
{
    public class GhostSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawns;
        [SerializeField] private float _spawnRadius = 5;

        public void MakeWave(int enemyCount, string enemyKey)
        {
            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEnemy(enemyKey);
            }
        }

        private void SpawnEnemy(string enemyKey)
        {
            int randomSpawnPosition = UnityEngine.Random.Range(0, _spawns.Length);
            float2 randomPosition2D = UnityEngine.Random.insideUnitCircle * _spawnRadius;
            float3 randomPosition3D = new(_spawns[randomSpawnPosition].position.x + randomPosition2D.x, 1.5f,
                _spawns[randomSpawnPosition].position.z + randomPosition2D.y);
            GameObject ghost = Pooler.instance.Pop(enemyKey);
            ghost.transform.position = randomPosition3D;
        }
    }
}