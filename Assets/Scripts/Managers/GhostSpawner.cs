using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.AI;


public class GhostSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawns;
    [SerializeField] private InteriorSpawner[] interiorSpawners;
    
    [SerializeField] private float _spawnRadius = 5;
    
    [Header("DEBUG")]
    [SerializeField] private List<Transform> curAvailableInteriorSpawners;

    public void MakeWave(int enemyCount, string enemyKey)
    {
        UpdateAvailableSpawners();
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy(enemyKey);
        }
    }

    private void UpdateAvailableSpawners()
    {
        List<Transform> list = new List<Transform>();
        foreach (var spawners in interiorSpawners)
        {
            if (!spawners.associatedRoom.IsLocked)
            {
                list.AddRange(spawners.spawners);
                
            }
        }

        curAvailableInteriorSpawners = list;
    }

    private void SpawnEnemy(string enemyKey)
    {
        int randomSpawnPosition = 0;
        float2 randomPosition2D;
        float3 randomPosition3D;
        if (enemyKey == "ECTOPLASMA" || enemyKey == "ZOMBIE")
        {
            if (curAvailableInteriorSpawners.Count == 0)
            {
                Debug.Log("No interior spawner available");
                return;
            }
            randomSpawnPosition = UnityEngine.Random.Range(0, curAvailableInteriorSpawners.Count);
            
            randomPosition3D = new(curAvailableInteriorSpawners[randomSpawnPosition].position.x, 1.23f,
                curAvailableInteriorSpawners[randomSpawnPosition].position.z);
        }
        else
        {
            randomSpawnPosition = UnityEngine.Random.Range(0, _spawns.Length);
            randomPosition2D = UnityEngine.Random.insideUnitCircle * _spawnRadius;
            randomPosition3D = new(_spawns[randomSpawnPosition].position.x + randomPosition2D.x, 1.23f,
                _spawns[randomSpawnPosition].position.z + randomPosition2D.y);
        }
        
        GameObject ghost = Pooler.instance.Pop(enemyKey);
        if (enemyKey == "ECTOPLASMA" || enemyKey == "ZOMBIE")
        {
            ghost.GetComponent<NavMeshAgent>().Warp(randomPosition3D);
        }
        else ghost.transform.position = randomPosition3D;
    }
}


[Serializable]
public class InteriorSpawner
{
    public Room associatedRoom;
    public Transform[] spawners;
}