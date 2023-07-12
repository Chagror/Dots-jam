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
   

<<<<<<< Updated upstream
    protected override void OnCreate()
    {
        inputs = new Inputs();
    }

    protected override void OnStartRunning()
=======
    [BurstCompile]
    public void OnCreate(ref SystemState state)
>>>>>>> Stashed changes
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
        
    {
<<<<<<< Updated upstream
        float shooting = inputs.BaseMap.Shoot.ReadValue<float>();

        //Check for InputGetterComponent and set movement value
        foreach (var inputs in SystemAPI.Query<RefRW<InputGetterComponent>>())
        {
            inputs.ValueRW.shoot = shooting == 0 ? false : true;
=======
        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);

        
        foreach (var player in SystemAPI.Query<RefRO<LocalTransform>, RefRO<PlayerTag>, RefRW<ProjectileSpawner>>())
        {

                player.Item3.ValueRW.spawnPos = player.Item1.ValueRO.Position;
                ProcessBulletSpawner(ref state, player.Item3, player.Item1.ValueRO.Position, ecb);
            
>>>>>>> Stashed changes
        }
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    private void ProcessBulletSpawner(ref SystemState state, RefRW<ProjectileSpawner> spawner, float3 spawnPos, EntityCommandBuffer ecb)
    {

        ClosestHitCollector<DistanceHit> closestHitCollector = new ClosestHitCollector<DistanceHit>(spawner.ValueRO.range);
        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        

        if (spawner.ValueRO.nextSpawnTime < SystemAPI.Time.ElapsedTime)
        {
            Vector3 nearestEnemyPos = new Vector3(spawnPos.x-1f,0f,spawnPos.z);
            if (physicsWorld.OverlapSphereCustom(spawnPos, spawner.ValueRO.range, ref closestHitCollector, spawner.ValueRO.filter))
                nearestEnemyPos = closestHitCollector.ClosestHit.Position;

            float angleTowardsNearestEnemy = ConvertPosToAngle(nearestEnemyPos.x - spawnPos.x, nearestEnemyPos.y - spawnPos.y);
            for (int i = 0; i < spawner.ValueRO.numBullets; i++)
            {
                Entity newEntity = state.EntityManager.Instantiate(spawner.ValueRO.bulletPrefab);
                ecb.AddComponent<BulletTag>(newEntity);
                Vector3 pos ;
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
        float angle = Mathf.Atan2(y, x) ;
        return angle;
    }
}
