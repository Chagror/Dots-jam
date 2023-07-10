using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;



[DisallowMultipleComponent]
public class MovementComponentAuthoring : MonoBehaviour
{
    public float speed;
}

class MovementBaker : Baker<MovementComponentAuthoring>
{
    public override void Bake(MovementComponentAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new MovementComponent
        {
            speed = authoring.speed
        });
    }
}