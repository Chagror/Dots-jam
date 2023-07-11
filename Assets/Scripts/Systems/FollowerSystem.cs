using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct FollowerSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var follower in SystemAPI.Query<RefRW<LocalTransform>, RefRO<PlayerTag>>())
        {
            ProcessFollow(ref state, follower);
        }
    }

    public void ProcessFollow(ref SystemState state, (RefRW<LocalTransform>, RefRO<PlayerTag>) player)
    {
        FollowerSingleton.instance.transform.position = player.Item1.ValueRO.Position;
    }    
}
