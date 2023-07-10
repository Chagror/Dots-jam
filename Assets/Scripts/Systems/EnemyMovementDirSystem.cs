using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Burst;

[BurstCompile]
public partial struct EnemyMovementDirSystem : ISystem 
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (RefRW<MovementComponent> enemy in SystemAPI.Query<RefRW<MovementComponent>>())
        {
            RandomDir(ref state, enemy);
        }
    }

    private void RandomDir(ref SystemState state, RefRW<MovementComponent> mc)
    {
        mc.ValueRW.dir = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
    }

}
