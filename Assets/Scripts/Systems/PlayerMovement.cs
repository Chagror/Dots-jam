using Unity.Entities;
using UnityEngine;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Device;

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
        Vector2 mousePosition = inputs.BaseMap.MousePosition.ReadValue<Vector2>();


        //Check for InputGetterComponent and set movement value
        foreach (var inputs in SystemAPI.Query<RefRW<InputGetterComponent>>())
        {
            inputs.ValueRW.movement = currentMovement;
            inputs.ValueRW.mousePosition = mousePosition;
        }

        //TODO penser a rajouter un singleton pour Hugo

        //Search for all components together on entities and make calculus
        foreach (var player in SystemAPI.Query<RefRW<InputGetterComponent>, RefRW<LocalTransform>, RefRO<PlayerTag>>())
        {
            float3 dir = new float3(player.Item1.ValueRO.movement.x, 0, player.Item1.ValueRO.movement.y);
                        
            player.Item2.ValueRW.Position += dir * player.Item1.ValueRW.speed * SystemAPI.Time.DeltaTime;
            player.Item1.ValueRW.mousePosition -= new float2(UnityEngine.Device.Screen.width/2, UnityEngine.Device.Screen.height/2);
            //Debug.Log(player.Item1.ValueRW.mousePosition);

            //Rotation of the player
            float3 forward = new float3(player.Item1.ValueRW.mousePosition.x, 0f, player.Item1.ValueRW.mousePosition.y);
            player.Item2.ValueRW.Rotation = Quaternion.LookRotation(forward, math.up());
        }
    }

    protected override void OnStopRunning()
    {
        inputs.Disable();
    }
}
