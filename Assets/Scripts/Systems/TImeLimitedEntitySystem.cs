using Unity.Burst;
using Unity.Entities;


[BurstCompile]
public partial struct TimeLimitedEntitySystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbBOS = SystemAPI
            .GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (time, entity)
            in SystemAPI.Query<RefRW<TimeLimitedEntityComponent>>().WithEntityAccess())
        {
            time.ValueRW.duration -= SystemAPI.Time.DeltaTime;
            if (time.ValueRO.duration < 0)
            {
                ecbBOS.DestroyEntity(entity);
            }
        }
    }
}