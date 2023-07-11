using System.Collections;
using System.Collections.Generic;
using UnityEngine; //PlayerSpawnerSystem
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

public partial struct PlayerSpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState state) { }
    public void OnDestroy(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        foreach (RefRW<PlayerSpawnerComponent> spawner in SystemAPI.Query<RefRW<PlayerSpawnerComponent>>())
        {
            ProcessSpawner(ref state, spawner);
        }
    }

    private void ProcessSpawner(ref SystemState state, RefRW<PlayerSpawnerComponent> spawner)
    {
        if(spawner.ValueRO.playerEntity == Entity.Null)
        {
            SpawnPlayer(ref state,spawner);
        }
        else if (SystemAPI.GetComponent<HealthComponent>(spawner.ValueRO.playerEntity).currentHealth <= 0)
        {
            SpawnPlayer(ref state, spawner);
        }
    }

    private void SpawnPlayer(ref SystemState state, RefRW<PlayerSpawnerComponent> spawner)
    {
        Entity playerEntity = state.EntityManager.Instantiate(spawner.ValueRO.playerPrefab);

        state.EntityManager.SetComponentData(playerEntity, LocalTransform.FromPosition(spawner.ValueRO.spawnPos));

        spawner.ValueRW.playerEntity = playerEntity;

    }
}