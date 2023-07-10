using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;


[DisallowMultipleComponent]
public class HealthComponentAuthoring : MonoBehaviour
{
    public int maxHealth = 100;
    
}

class HealthBaker : Baker<HealthComponentAuthoring>
{
    public override void Bake(HealthComponentAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new HealthComponent
        {
            maxHealth =authoring.maxHealth,
            currentHealth =authoring.maxHealth,
        });
    }
}