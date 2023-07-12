using Unity.Entities;
using UnityEngine;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.InputSystem.Controls;

[BurstCompile]
public partial class PlayerShoot : SystemBase
{
    private Inputs inputs;

    protected override void OnStartRunning()
    {
        inputs.Enable();
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        bool shooting = inputs.BaseMap.Shoot.ReadValue<bool>();


        //Check for InputGetterComponent and set movement value
        foreach (var inputs in SystemAPI.Query<RefRW<InputGetterComponent>>())
        {
            inputs.ValueRW.shoot = shooting;
        }

        //Search for all components together on entities and make calculus
        foreach (var player in SystemAPI.Query<RefRW<InputGetterComponent>, RefRO<PlayerTag>>())
        {
            //Shoot here
        }
    }
}
