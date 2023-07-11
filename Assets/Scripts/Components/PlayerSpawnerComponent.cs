using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
public struct PlayerSpawnerComponent : IComponentData
{
    public Entity playerPrefab;
    public float3 spawnPos;
    public Entity playerEntity;
}
