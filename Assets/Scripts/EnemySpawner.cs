using System;
using Entities;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;
    [SerializeField] private Transform[] _spawns;
    [SerializeField] private float _spawnRadius = 5;
    [SerializeField] private float _minSpeed = 0.75f;
    [SerializeField] private float _maxSpeed = 1.25f;

    private EntityManager _entityManager;

    private RenderMeshDescription _renderMeshDescription =
        new(ShadowCastingMode.Off, receiveShadows: false);

    public void MakeWave(int identifiant, Mesh mesh, Material mat, int enemyCount, int distanceAlert)
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        _mesh = mesh;
        _material = mat;

        EntityArchetype entityArchetype;

        switch (identifiant)
        {
            case 1:
                entityArchetype =
                    _entityManager.CreateArchetype(typeof(LocalTransform), typeof(RenderMeshArray),
                        typeof(MoveForward),
                        typeof(PlayerLooker), typeof(EnemyTag), typeof(Avoidance), typeof(Patrol));
                break;
            case 2:
                entityArchetype =
                    _entityManager.CreateArchetype(typeof(LocalTransform), typeof(RenderMeshArray),
                        typeof(MoveForward),
                        typeof(PlayerLooker), typeof(EnemyTag), typeof(Avoidance), typeof(Patrol));
                break;
            case 3:
                entityArchetype =
                    _entityManager.CreateArchetype(typeof(LocalTransform), typeof(RenderMeshArray),
                        typeof(MoveForward),
                        typeof(PlayerLooker), typeof(EnemyTag), typeof(Avoidance), typeof(Patrol));
                break;
            default:
                throw new IndexOutOfRangeException();
        }

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy(entityArchetype, distanceAlert);
        }
    }

    private void SpawnEnemy(EntityArchetype entityArchetype, int distanceAlert)
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

        _entityManager.SetComponentData(entity, new PlayerLooker()
        {
            CurrentCheckpoint = 0,
            IsTargeting = false,
            DistanceAlert = distanceAlert
        });
    }
}