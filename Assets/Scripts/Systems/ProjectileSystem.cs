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
[UpdateAfter(typeof(SpawnerSystem))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
[BurstCompile]
public partial struct DamageToPlayerSystem : ISystem
{
    ComponentLookup<PlayerTag> playerLookup;
    ComponentLookup<BulletTag> bulletLookup;

    ComponentLookup<LocalTransform> localTransformLookup;
    ComponentLookup<Impact> impactLookup;
    ComponentLookup<EnemyTag> enemyLookup;
    ComponentLookup<ColisTag> colisLookup;
    ComponentLookup<HealthComponent> healthLookup;

    public void OnCreate(ref SystemState state) {
        playerLookup = SystemAPI.GetComponentLookup<PlayerTag>(true);
        bulletLookup = SystemAPI.GetComponentLookup<BulletTag>(true);
        impactLookup = SystemAPI.GetComponentLookup<Impact>(false);
        enemyLookup = SystemAPI.GetComponentLookup<EnemyTag>(true);
        healthLookup = SystemAPI.GetComponentLookup<HealthComponent>(false);
        localTransformLookup = SystemAPI.GetComponentLookup<LocalTransform>(false);
        colisLookup = SystemAPI.GetComponentLookup<ColisTag>(false);

    }
    public void OnDestroy(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        var ecbBOS = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        SimulationSingleton simulation = SystemAPI.GetSingleton<SimulationSingleton>();

        playerLookup.Update(ref state);
        enemyLookup.Update(ref state);
        healthLookup.Update(ref state);
        bulletLookup.Update(ref state);
        impactLookup.Update(ref state);
        localTransformLookup.Update(ref state);
        colisLookup.Update(ref state);

        state.Dependency = new PlayerDamageHitJob()
        {
            
            Enemies = enemyLookup,
            Player = playerLookup,
            Healths = healthLookup,
            ECB = ecbBOS
        }.Schedule(simulation, state.Dependency);

        state.Dependency = new ProjectileHitJob()
        {

            Enemies = enemyLookup,
            Bullets = bulletLookup,
            Healths = healthLookup,
            Impacts = impactLookup,
            LocalTransforms = localTransformLookup,
            ECB = ecbBOS
        }.Schedule(simulation, state.Dependency);

        state.Dependency = new ColisHitJob()
        {
            Colis = colisLookup
            ,
            Players = playerLookup
            ,
            ECB = ecbBOS
        }.Schedule(simulation, state.Dependency);
    }


    public struct ColisHitJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentLookup<ColisTag> Colis;
        [ReadOnly] public ComponentLookup<PlayerTag> Players;
        public EntityCommandBuffer ECB;

        public void Execute(CollisionEvent triggerEvent)
        {


            Entity colis = Entity.Null;
            Entity player = Entity.Null;

            // Identiy which entity is which
            if (Colis.HasComponent(triggerEvent.EntityA))
                colis = triggerEvent.EntityA;
            if (Colis.HasComponent(triggerEvent.EntityB))
                colis = triggerEvent.EntityB;
            if (Players.HasComponent(triggerEvent.EntityA))
                player = triggerEvent.EntityA;
            if (Players.HasComponent(triggerEvent.EntityB))
                player = triggerEvent.EntityB;

            // if its a pair of entity we don't want to process, exit
            if (Entity.Null.Equals(colis)
                || Entity.Null.Equals(player)) return;
            ECB.DestroyEntity(colis);
        }

    }


    [BurstCompile]
    public struct ProjectileHitJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<BulletTag> Bullets;
        [ReadOnly] public ComponentLookup<EnemyTag> Enemies;
        public ComponentLookup<HealthComponent> Healths;
        public ComponentLookup<Impact> Impacts;
        public ComponentLookup<LocalTransform> LocalTransforms;

        public EntityCommandBuffer ECB;


        public void Execute(TriggerEvent triggerEvent)
        {


            Entity bullet = Entity.Null;
            Entity enemy = Entity.Null;

            // Identiy which entity is which
            if (Bullets.HasComponent(triggerEvent.EntityA))
                bullet = triggerEvent.EntityA;
            if (Bullets.HasComponent(triggerEvent.EntityB))
                bullet = triggerEvent.EntityB;
            if (Enemies.HasComponent(triggerEvent.EntityA))
                enemy = triggerEvent.EntityA;
            if (Enemies.HasComponent(triggerEvent.EntityB))
                enemy = triggerEvent.EntityB;

            // if its a pair of entity we don't want to process, exit
            if (Entity.Null.Equals(bullet)
                || Entity.Null.Equals(enemy)) return;


            


            HealthComponent hp = Healths[enemy];
            hp.currentHealth -= 100;
            Healths[enemy] = hp;
            Debug.Log("damage dealt");
            // Destroy enemy if it is out of health
            if (hp.currentHealth <= 0)
            {
                //UISingleton.instance.AddEnemy(-1);
                ECB.DestroyEntity(enemy);
            }
            Entity impactEntity = ECB.Instantiate(Impacts[bullet].Prefab);
            ECB.SetComponent(impactEntity, LocalTransform.FromPosition(LocalTransforms[enemy].Position));

            ECB.DestroyEntity(bullet);

            // Spawn VFX
            //Entity impactEntity = ECB.Instantiate(Projectiles[projectile].Prefab);
            //ECB.SetComponent(impactEntity,
            //    LocalTransform.FromPosition(Positions[enemy].Position));

            // Destroy projectile if it hits all its targets
            //ECB.DestroyEntity(projectile);

        }

    }

    [BurstCompile]
    public struct PlayerDamageHitJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<PlayerTag>Player;
        [ReadOnly] public ComponentLookup<EnemyTag> Enemies;
        public ComponentLookup<HealthComponent> Healths;

        public EntityCommandBuffer ECB;

        
        public void Execute(TriggerEvent triggerEvent)
        {
            

            Entity player = Entity.Null;
            Entity enemy = Entity.Null;

            // Identiy which entity is which
            if (Player.HasComponent(triggerEvent.EntityA))
                player = triggerEvent.EntityA;
            if (Player.HasComponent(triggerEvent.EntityB))
                player = triggerEvent.EntityB;
            if (Enemies.HasComponent(triggerEvent.EntityA))
                enemy = triggerEvent.EntityA;
            if (Enemies.HasComponent(triggerEvent.EntityB))
                enemy = triggerEvent.EntityB;

            // if its a pair of entity we don't want to process, exit
            if (Entity.Null.Equals(player)
                || Entity.Null.Equals(enemy)) return;
            


 

            // Damage player
            HealthComponent hp = Healths[player];
            hp.currentHealth -= 5;
            Healths[player] = hp;
            //Debug.Log("damage dealt");
            // Destroy enemy if it is out of health
            if (hp.currentHealth <= 0)
                ECB.DestroyEntity(player);
            
            // Spawn VFX
            //Entity impactEntity = ECB.Instantiate(Projectiles[projectile].Prefab);
            //ECB.SetComponent(impactEntity,
            //    LocalTransform.FromPosition(Positions[enemy].Position));

            // Destroy projectile if it hits all its targets
                //ECB.DestroyEntity(projectile);

        }

    }
}