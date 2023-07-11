using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using Random = UnityEngine.Random;


#region Hard to read but optimized with burst
/*
[BurstCompile]
public partial struct SpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState state) { }

    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer.ParallelWriter ecb = GetEntityCommandBuffer(ref state);

        // Creates a new instance of the job, assigns the necessary data, and schedules the job in parallel.
        new ProcessSpawnerJob
        {
            ElapsedTime = SystemAPI.Time.ElapsedTime,
            Ecb = ecb
        }.ScheduleParallel();
    }

    private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        return ecb.AsParallelWriter();
    }
}

[BurstCompile]
public partial struct ProcessSpawnerJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter Ecb;
    public double ElapsedTime;

    // IJobEntity generates a component data query based on the parameters of its `Execute` method.
    // This example queries for all Spawner components and uses `ref` to specify that the operation
    // requires read and write access. Unity processes `Execute` for each entity that matches the
    // component data query.
    private void Execute([ChunkIndexInQuery] int chunkIndex, ref EnemySpawner spawner)
    {
        // If the next spawn time has passed.
        if (spawner.nextSpawnTime < ElapsedTime)
        {
            // Spawns a new entity and positions it at the spawner.
            Entity newEntity = Ecb.Instantiate(chunkIndex, spawner.enemyPrefab);
            Ecb.SetComponent(chunkIndex, newEntity, LocalTransform.FromPosition(spawner.spawnPos));

            // Resets the next spawn time.
            spawner.nextSpawnTime = (float)ElapsedTime + spawner.spawnRate;
        }
    }
}
*/
#endregion



#region Easy but not optimised with burst

public partial struct SpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState state) { }
    public void OnDestroy(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {

        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);
        foreach ((RefRW<EnemySpawner>, RefRO<LocalTransform>) spawner in SystemAPI.Query<RefRW<EnemySpawner>,RefRO<LocalTransform>>())
        {
            ProcessEnemySpawner(ref state, spawner.Item1, spawner.Item2.ValueRO.Position, ecb);
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    private void ProcessEnemySpawner(ref SystemState state, RefRW<EnemySpawner> spawner, float3 spawnPos, EntityCommandBuffer ecb)
    {
        if (spawner.ValueRO.nextSpawnTime < SystemAPI.Time.ElapsedTime)
        {
            Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.enemyPrefab);
            ecb.AddComponent<EnemyTag>(newEntity);
            Vector3 pos = RandomCircle(spawnPos, spawner.ValueRO.innerRadius, spawner.ValueRO.outerRadius);
            state.EntityManager.SetComponentData(newEntity, LocalTransform.FromPosition(pos));

            spawner.ValueRW.nextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.spawnRate;
            
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

#endregion