using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics.Systems;
using Unity.Physics;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))] // We are updating after `PhysicsSimulationGroup` - this means that we will get the events of the current frame.
public partial struct GetNumTriggerEventsSystem : ISystem
{
    [BurstCompile]
    public partial struct CountNumTriggerEvents : ITriggerEventsJob
    {
        public NativeReference<int> NumTriggerEvents;
        public void Execute(TriggerEvent collisionEvent)
        {
            NumTriggerEvents.Value++;
        }
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        NativeReference<int> numTriggerEvents = new NativeReference<int>(0, Allocator.TempJob);

        state.Dependency = new CountNumTriggerEvents
        {
            NumTriggerEvents = numTriggerEvents
        }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>());

       
    }
}