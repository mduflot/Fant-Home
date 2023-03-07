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
        [SerializeField] private Mesh _mesh;
        [SerializeField] private Material _material;
        [SerializeField] private int _enemyCount = 100;
        [SerializeField] private Transform[] _spawns;
        [SerializeField] private float _spawnRadius = 5;
        [SerializeField] private float _minSpeed = 0.75f;
        [SerializeField] private float _maxSpeed = 1.25f;

        private EntityManager _entityManager;

        private RenderMeshDescription _renderMeshDescription =
            new(ShadowCastingMode.Off, receiveShadows: false);

        private void Start()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            EntityArchetype entityArchetype =
                _entityManager.CreateArchetype(typeof(LocalTransform), typeof(RenderMeshArray), typeof(MoveForward),
                    typeof(PlayerLooker), typeof(EnemyTag), typeof(Avoidance));

            for (int i = 0; i < _enemyCount; i++)
            {
                SpawnEnemy(entityArchetype);
            }
        }

        private void SpawnEnemy(EntityArchetype entityArchetype)
        {
            Entity entity = _entityManager.CreateEntity(entityArchetype);
            _entityManager.SetName(entity, "New Entity");

            int randomSpawnPosition = UnityEngine.Random.Range(0, _spawns.Length);
            float2 randomPosition2D = UnityEngine.Random.insideUnitCircle * _spawnRadius;
            float3 randomPosition3D = new(_spawns[randomSpawnPosition].position.x + randomPosition2D.x, 0,
                _spawns[randomSpawnPosition].position.z + randomPosition2D.y);

            _entityManager.SetComponentData(entity, new LocalTransform()
            {
                Position = randomPosition3D,
                Rotation = new quaternion(),
                Scale = 1
            });

            RenderMeshUtility.AddComponents(entity, _entityManager, _renderMeshDescription,
                new(new[] { _material }, new[] { _mesh }), MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));

            _entityManager.SetComponentData(entity, new MoveForward()
            {
                Speed = UnityEngine.Random.Range(_minSpeed, _maxSpeed)
            });

            _entityManager.SetComponentData(entity, new Avoidance()
            {
                AvoidanceStrength = 2f,
                AvoidanceDistance = 0.75f
            });
        }
    }
}