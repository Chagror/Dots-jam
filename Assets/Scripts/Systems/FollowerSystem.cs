using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct FollowerSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var follower in SystemAPI.Query<RefRW<LocalTransform>, RefRO<FollowEntity>>())
        {
            ProcessFollow(ref state, follower);
        }
    }

    public void ProcessFollow(ref SystemState state, (RefRW<LocalTransform>, RefRO<FollowEntity>) followerEntity)
    {
        if (followerEntity.Item2.ValueRO.entityToFollow == null) return;
        followerEntity.Item1.ValueRW.Position = state.EntityManager.GetComponentData<LocalTransform>(followerEntity.Item2.ValueRO.entityToFollow).Position;
    }    
}
