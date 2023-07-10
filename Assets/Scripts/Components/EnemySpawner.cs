using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct EnemySpawner : IComponentData
{
    public Entity enemyPrefab;
    public float3 spawnPos;
    public float nextSpawnTime;
    public float spawnRate;
    public float innerRadius;
    public float outerRadius;
}
