using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerTagAuthoring : MonoBehaviour
{
    
}

class PlayerTagBaker : Baker<PlayerTagAuthoring>
{
    public override void Bake(PlayerTagAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new PlayerTag());
    }
}
