using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovementAuthoring : MonoBehaviour
{
    public float2 movement;
    public float speed;
}

class PlayerMovementBaker : Baker<PlayerMovementAuthoring>
{
    public override void Bake(PlayerMovementAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        //Debug.Log(entity);

        AddComponent(entity, new InputGetterComponent
        {
            movement = authoring.movement,
            speed = 0f
        });
    }
}
