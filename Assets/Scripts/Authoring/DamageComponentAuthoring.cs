using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class DamageComponentAuthoring : MonoBehaviour
{
    public float damage;
    public float delayBetweenDamage;
}

class DamageBaker : Baker<DamageComponentAuthoring>
{
    public override void Bake(DamageComponentAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new DamageComponent
        {
            damage = authoring.damage,
            delayBetweenDamage = authoring.delayBetweenDamage,
        });
    }
}