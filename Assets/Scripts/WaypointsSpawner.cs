using System;
using Entities;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class WaypointsSpawner : MonoBehaviour
{
    [Tooltip("Add waypoints in order")] [SerializeField]
    private Vector3[] _waypoints;

    private int _numberOfWaypoints;

    private EntityManager _entityManager;

    private void Awake()
    {
        _numberOfWaypoints = _waypoints.Length;
        MakeWave();
    }

    public void MakeWave()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype entityArchetype;
        
        entityArchetype =
            _entityManager.CreateArchetype(typeof(LocalTransform), typeof(WaypointTag));

        for (int i = 0; i < _numberOfWaypoints; i++)
        {
            SpawnEnemy(entityArchetype, i);
        }
    }

    private void SpawnEnemy(EntityArchetype entityArchetype, int idWaypoints)
    {
        Entity entity = _entityManager.CreateEntity(entityArchetype);
        _entityManager.SetName(entity, "New Entity");

        _entityManager.SetComponentData(entity, new LocalTransform()
        {
            Position = _waypoints[idWaypoints],
            Rotation = new quaternion(),
            Scale = 1
        });
    }
}