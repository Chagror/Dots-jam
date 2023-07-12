using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct BulletMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var bullet in SystemAPI.Query<RefRW<MovementComponent>, RefRO<BulletTag>, RefRO<LocalTransform>>())
        {
            PlayerDir(ref state, bullet.Item1, bullet.Item3.ValueRO.Position);
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

        enemy.ValueRW.dir = new Vector2(-dir.x, -dir.z);
    }

}
