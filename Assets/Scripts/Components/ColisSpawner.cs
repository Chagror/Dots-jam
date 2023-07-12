using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct ColisSpawner : IComponentData
{
    public Entity colisPrefab;
    public float3 spawnPos;
    public float nextSpawnTime;
    public float spawnRate;
    public float innerRadius;
    public float outerRadius;
}
