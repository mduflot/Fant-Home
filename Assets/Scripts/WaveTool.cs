using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class WaveTool : MonoBehaviour
{
    public EnemySpawner spawner;
    
    public string waveName = "Wave";
    public Wave[] waves = new Wave[3];

    private int index = 0;
    private float timer;

    private void Start()
    {
        if (waves.Length == 0) return;
        SpawnWave(waves[index]);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void GoToNextWave()
    {
        index++;
        if (index > waves.Length-1) return;
        SpawnWave(waves[index]);
    }

    private void SpawnWave(Wave wave)
    {
        if (wave.enemies.Length == 0) return;
        foreach (var enemy in wave.enemies)
        {
            spawner.MakeWave(enemy.id, enemy.myMesh, enemy.myMaterial, enemy.number, enemy.DistanceAlert);
        }

        StartCoroutine(WaveDuration(wave.spawnTime));
    }

    private IEnumerator WaveDuration(float t)
    {
        yield return new WaitForSeconds(t);
        GoToNextWave();
    }
}
