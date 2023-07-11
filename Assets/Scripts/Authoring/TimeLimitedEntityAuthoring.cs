using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[DisallowMultipleComponent]
public class TimeLimitedEntityAuthoring : MonoBehaviour
{
    public float duration =5f;
}

class TimeLimitedEntityBaker : Baker<TimeLimitedEntityAuthoring>
{
    public override void Bake(TimeLimitedEntityAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        Debug.Log(entity);

        AddComponent(entity, new TimeLimitedEntityComponent
        {
            duration = authoring.duration,
        });

    }
}
