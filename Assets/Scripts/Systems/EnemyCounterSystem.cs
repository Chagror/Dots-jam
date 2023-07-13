using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;

[BurstCompile]
public partial struct EnemyCounterSystem : ISystem
{
    EntityQuery enemyQuery;
    public void OnCreate(ref SystemState state)
    {
        enemyQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<EnemyTag>().Build(ref state);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        UISingleton.instance.SetEnemy(enemyQuery.CalculateEntityCount());
    }
}