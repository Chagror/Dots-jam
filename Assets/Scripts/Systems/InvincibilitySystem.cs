using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
public partial struct InvincibilitySystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var player in SystemAPI.Query<RefRW<HealthComponent>,RefRO<PlayerTag>>())
        {
            player.Item1.ValueRW.nextTimeCanBeDamaged -= SystemAPI.Time.DeltaTime;
        }
    }
}
