using System.Collections;
using System.Collections.Generic;
using UnityEngine; //PlayerSpawnerSystem
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Collections;

public partial struct PlayerSpawnerSystem : ISystem
{
    EntityQuery enemyQuery;
    EntityQuery colisQuery;
    public void OnCreate(ref SystemState state) 
    {
        enemyQuery = new EntityQueryBuilder(Allocator.Temp)
            .WithAllRW<EnemyTag>().Build(ref state);

        colisQuery = new EntityQueryBuilder(Allocator.Temp)
            .WithAllRW<ColisTag>().Build(ref state);
    }
    public void OnDestroy(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
        foreach (RefRW<PlayerSpawnerComponent> spawner in SystemAPI.Query<RefRW<PlayerSpawnerComponent>>())
        {
            ProcessSpawner(ref state, spawner, ecb);
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    private void ProcessSpawner(ref SystemState state, RefRW<PlayerSpawnerComponent> spawner, EntityCommandBuffer ecb)
    {
        if(spawner.ValueRO.playerEntity == Entity.Null)
        {
            SpawnPlayer(ref state,spawner, ecb);
        }
        else if (SystemAPI.GetComponent<HealthComponent>(spawner.ValueRO.playerEntity).currentHealth <= 0)
        {
            SpawnPlayer(ref state, spawner, ecb);
        }
    }

    private void SpawnPlayer(ref SystemState state, RefRW<PlayerSpawnerComponent> spawner, EntityCommandBuffer ecb)
    {

        foreach (var enemy in enemyQuery.ToEntityArray(Allocator.Temp))
        {
            ecb.DestroyEntity(enemy);
        }

        foreach (var colis in colisQuery.ToEntityArray(Allocator.Temp))
        {
            ecb.DestroyEntity(colis);
        }

        Entity playerEntity = state.EntityManager.Instantiate(spawner.ValueRO.playerPrefab);
        ecb.AddComponent<PlayerTag>(playerEntity);
        state.EntityManager.SetComponentData(playerEntity, LocalTransform.FromPosition(spawner.ValueRO.spawnPos));

        spawner.ValueRW.playerEntity = playerEntity;

        foreach (var playerFollower in SystemAPI.Query<RefRW<FollowEntity>>())
        {
            playerFollower.ValueRW.entityToFollow = playerEntity;
        }

    }
}