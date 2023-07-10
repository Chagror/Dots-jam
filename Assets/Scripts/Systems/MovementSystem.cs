using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
public partial struct MovementSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var movingEntity in SystemAPI.Query<RefRO<MovementComponent>, RefRW<LocalTransform>>())
        {
            Move(ref state, movingEntity);
        }
    }

    private void Move(ref SystemState state, (RefRO<MovementComponent>, RefRW<LocalTransform>) entityToMove)
    {
        Vector3 dir = new Vector3(entityToMove.Item2.ValueRO.Position.x + entityToMove.Item1.ValueRO.dir.x * entityToMove.Item1.ValueRO.speed,
            entityToMove.Item2.ValueRO.Position.y,
            entityToMove.Item2.ValueRO.Position.z + entityToMove.Item1.ValueRO.dir.y * entityToMove.Item1.ValueRO.speed);
        entityToMove.Item2.ValueRW.Position = new float3(dir);
    }
}
