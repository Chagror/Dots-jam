using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct UpdateUI : ISystem
{
    public void OnCreate(ref SystemState state) { }
    public void OnDestroy(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
        foreach (var player in SystemAPI.Query<RefRW<HealthComponent>, RefRO<PlayerTag>>())
        {
            UISingleton.instance.UpdateHealth(player.Item1.ValueRW.currentHealth, 100.0f);
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
