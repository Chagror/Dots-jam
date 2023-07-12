using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

public struct ProjectileSpawner : IComponentData
{
    public Entity bulletPrefab;
    public float3 spawnPos;
    public float nextSpawnTime;
    public float spawnRate;
    public float radius;
    public int numBullets;
    public float angle;
    public CollisionFilter filter;
    public float range;
}
