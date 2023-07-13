using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using UnityEngine;
using Unity.Collections;

public partial struct ColisSpawnerSystem : ISystem
{

    EntityQuery numberOfColis;

    public static int totalColisSpawned;
    public void OnCreate(ref SystemState state) {

        totalColisSpawned = 0;

        numberOfColis = new EntityQueryBuilder(Allocator.Temp).WithAll<ColisTag>().Build(ref state);
    }
    public void OnDestroy(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {

        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
        foreach ((RefRW<ColisSpawner>, RefRO<LocalTransform>) spawner in SystemAPI.Query<RefRW<ColisSpawner>, RefRO<LocalTransform>>())
        {
            ProcessEnemySpawner(ref state, spawner.Item1, spawner.Item2.ValueRO.Position, ecb);
        }
        UISingleton.instance.SetPoints(totalColisSpawned - numberOfColis.CalculateEntityCount());
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    private void ProcessEnemySpawner(ref SystemState state, RefRW<ColisSpawner> spawner, float3 spawnPos, EntityCommandBuffer ecb)
    {
        if (spawner.ValueRO.nextSpawnTime < SystemAPI.Time.ElapsedTime)
        {
            Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.colisPrefab);
            ecb.AddComponent<ColisTag>(newEntity);
            Vector3 pos = RandomCircle(spawnPos, spawner.ValueRO.innerRadius, spawner.ValueRO.outerRadius);
            state.EntityManager.SetComponentData(newEntity, LocalTransform.FromPosition(pos));

            spawner.ValueRW.nextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.spawnRate;
            totalColisSpawned++;
        }
    }

    Vector3 RandomCircle(Vector3 center, float innerRadius, float outerRadius)
    {
        float ang = Random.value * 360;
        float radius = Random.Range(innerRadius, outerRadius);
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }
}
