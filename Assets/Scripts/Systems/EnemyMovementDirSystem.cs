using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
public partial struct EnemyMovementDirSystem : ISystem 
{


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var enemy in SystemAPI.Query<RefRW<MovementComponent>, RefRO<EnemyTag>, RefRO<LocalTransform>>())
        {
            PlayerDir(ref state,enemy.Item1,enemy.Item3.ValueRO.Position);
        }
    }

    private float3 FindPlayer(ref SystemState state)
    {
        foreach (var player in SystemAPI.Query<RefRO<LocalTransform>, RefRO<PlayerTag>>())
        {
            return player.Item1.ValueRO.Position;
        }
        return float3.zero;
    }

    private void PlayerDir(ref SystemState state, RefRW<MovementComponent> enemy, float3 enemyPos)
    {
        Vector3 dir = (FindPlayer(ref state) - enemyPos);
        dir.Normalize();
        dir *= enemy.ValueRO.speed;

        enemy.ValueRW.dir = new Vector2(dir.x,dir.z);
    }

}
