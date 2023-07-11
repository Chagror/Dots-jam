using Unity.Entities;
using UnityEngine;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.InputSystem.Controls;

[BurstCompile]
public partial class PlayerMovement : SystemBase
{
    private Inputs inputs;

    protected override void OnCreate()
    {
        inputs = new Inputs();
    }

    protected override void OnStartRunning()
    {
        inputs.Enable();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        Vector2 currentMovement = inputs.BaseMap.PlayerMovement.ReadValue<Vector2>();
       

        //Check for InputGetterComponent and set movement value
        foreach(var inputs in SystemAPI.Query<RefRW<InputGetterComponent>>())
        {
            inputs.ValueRW.movement = currentMovement;
        }

        //TODO penser a rajouter un singleton pour Hugo

        //Search for all components together on entities and make calculus
        foreach (var player in SystemAPI.Query<RefRW<InputGetterComponent>, RefRW<LocalTransform>, RefRO<PlayerTag>>())
        {
            Vector3 dir = new Vector3(player.Item2.ValueRO.Position.x + player.Item1.ValueRO.movement.x,
                player.Item2.ValueRO.Position.y,
                player.Item2.ValueRO.Position.z + player.Item1.ValueRO.movement.y);

            dir.Normalize();
            
            player.Item2.ValueRW.Position += (float3)dir * player.Item1.ValueRW.speed;
            //Debug.Log(player.Item2.ValueRW.Position);

            //Rotation of the player
            if (math.lengthsq(player.Item1.ValueRW.movement) > float.Epsilon)
            {
                float3 forward = new float3(player.Item1.ValueRW.movement.x, 0f, player.Item1.ValueRW.movement.y);
                player.Item2.ValueRW.Rotation = Quaternion.LookRotation(forward, math.up());
            }
        }
    }

    protected override void OnStopRunning()
    {
        inputs.Disable();
    }
}
