using Unity.Entities;
using UnityEngine;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.InputSystem.Controls;
using Unity.Physics;

[BurstCompile]
public partial struct PlayerShoot : ISystem
{

    



    private float3 playerPos;
    private float3 ClosestEnemyPos;
    private float minDist;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);


        ClosestEnemyPos = float3.zero;
        minDist = float.MaxValue;

        foreach (var player in SystemAPI.Query<RefRO<LocalTransform>, RefRO<PlayerTag>, RefRW<ProjectileSpawner>>())
        {

                player.Item3.ValueRW.spawnPos = player.Item1.ValueRO.Position;
            playerPos = player.Item1.ValueRO.Position;
            player.Item3.ValueRW.filter = new CollisionFilter()
            {
                BelongsTo = 0,
                CollidesWith = 1
            };
                ProcessBulletSpawner(ref state, player.Item3, player.Item1.ValueRO.Position, ecb);
            

        }

        foreach (var enemy in SystemAPI.Query<RefRO<EnemyTag>, RefRO<LocalTransform>>())
        {
            float tempDist = Distance(playerPos, enemy.Item2.ValueRO.Position);
            if (tempDist<minDist)
            {
                minDist = tempDist;
                ClosestEnemyPos = enemy.Item2.ValueRO.Position;
            }

        }
        Debug.Log(minDist.ToString());
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    private void ProcessBulletSpawner(ref SystemState state, RefRW<ProjectileSpawner> spawner, float3 spawnPos, EntityCommandBuffer ecb)
    {

        ClosestHitCollector<DistanceHit> closestHitCollector = new ClosestHitCollector<DistanceHit>(spawner.ValueRO.range);
        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        

        if (spawner.ValueRO.nextSpawnTime < SystemAPI.Time.ElapsedTime)
        {


            float angleTowardsNearestEnemy = ConvertPosToAngle(ClosestEnemyPos.x - spawnPos.x, ClosestEnemyPos.y - spawnPos.y);
            for (int i = 0; i < spawner.ValueRO.numBullets; i++)
            {
                Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.bulletPrefab);
                ecb.AddComponent<BulletTag>(newEntity);
                Vector3 pos ;

                //pos.z = spawnPos.x + spawner.ValueRO.radius * Mathf.Sin(angleTowardsNearestEnemy );
                //pos.y = spawnPos.y;
                //pos.x = spawnPos.z + spawner.ValueRO.radius * Mathf.Cos(angleTowardsNearestEnemy);

                pos.x = spawnPos.x + spawner.ValueRO.radius * Mathf.Sin(angleTowardsNearestEnemy - spawner.ValueRO.angle * Mathf.Deg2Rad + (2 * i * spawner.ValueRO.angle * Mathf.Deg2Rad) / spawner.ValueRO.numBullets);
                pos.y = spawnPos.y;
                pos.z = spawnPos.z + spawner.ValueRO.radius * Mathf.Cos(angleTowardsNearestEnemy - spawner.ValueRO.angle * Mathf.Deg2Rad + (2 * i * spawner.ValueRO.angle * Mathf.Deg2Rad) / spawner.ValueRO.numBullets);
                state.EntityManager.SetComponentData(newEntity, LocalTransform.FromPosition(pos));


            }


            spawner.ValueRW.nextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.spawnRate;
            

            
        }
    }


    //Angle en radian !!!
    float ConvertPosToAngle(float x, float y)
    {
        float angle = 2*Mathf.Atan(y/(x+Mathf.Sqrt(x*x+y*y ))) ;
        return angle;
    }

    float Distance( float3 a, float3 b)
    {
        return Mathf.Sqrt(Mathf.Pow((a.x - b.x),2) + Mathf.Pow((a.z - b.z),2));
    }
}
