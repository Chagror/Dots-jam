using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
[BurstCompile]
public partial struct ProjectileSystem : ISystem
{
    ComponentLookup<LocalTransform> positionLookup;
    ComponentLookup<Impact> impactLookup;
    //BufferLookup<HitList> hitListLookup;
    ComponentLookup<HealthComponent> healthLookup;
    ComponentLookup<DamageComponent> damageLookup;

    public void OnCreate(ref SystemState state) {
        positionLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
        impactLookup = SystemAPI.GetComponentLookup<Impact>(false);    
        healthLookup = SystemAPI.GetComponentLookup<HealthComponent>(false);
        damageLookup = SystemAPI.GetComponentLookup<DamageComponent>(false);
    }
    public void OnDestroy(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        var ecbBOS = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        SimulationSingleton simulation = SystemAPI.GetSingleton<SimulationSingleton>();
        state.Dependency = new ProjectileHitJob()
        {
            Projectiles = impactLookup,
            EnemiesHealth = healthLookup,
            Positions = positionLookup,
            ECB = ecbBOS
        }.Schedule(simulation, state.Dependency);
    }

    [BurstCompile]
    public struct ProjectileHitJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<LocalTransform> Positions;
        public ComponentLookup<Impact> Projectiles;
        public ComponentLookup<HealthComponent> EnemiesHealth;

        public EntityCommandBuffer ECB;


        public void Execute(TriggerEvent triggerEvent)
        {


            Entity projectile = Entity.Null;
            Entity enemy = Entity.Null;

            // Identiy which entity is which
            if (Projectiles.HasComponent(triggerEvent.EntityA))
                projectile = triggerEvent.EntityA;
            if (Projectiles.HasComponent(triggerEvent.EntityB))
                projectile = triggerEvent.EntityB;
            if (EnemiesHealth.HasComponent(triggerEvent.EntityA))
                enemy = triggerEvent.EntityA;
            if (EnemiesHealth.HasComponent(triggerEvent.EntityB))
                enemy = triggerEvent.EntityB;

            // if its a pair of entity we don't want to process, exit
            if (Entity.Null.Equals(projectile)
                || Entity.Null.Equals(enemy)) return;


            // Check we did not already hit that traget in previous frames


            // Add enemy to list of already hit entities
            // to avoid hitting it next frame due to the
            // stateless nature of the Physics
 

            // Damage enemy
            HealthComponent hp = EnemiesHealth[enemy];
            hp.currentHealth -= 5;
            EnemiesHealth[enemy] = hp;

            // Destroy enemy if it is out of health
            if (hp.currentHealth <= 0)
                ECB.DestroyEntity(enemy);

            // Spawn VFX
            Entity impactEntity = ECB.Instantiate(Projectiles[projectile].Prefab);
            ECB.SetComponent(impactEntity,
                LocalTransform.FromPosition(Positions[enemy].Position));

            // Destroy projectile if it hits all its targets
                ECB.DestroyEntity(projectile);

        }

    }
}