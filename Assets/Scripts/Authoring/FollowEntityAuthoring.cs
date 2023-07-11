using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.VisualScripting;

[AddComponentMenu("Custom Authoring/Follow Entity Authoring")]
public class FollowEntityAuthoring : MonoBehaviour
{

}

class FollowEntityBaker : Baker<FollowEntityAuthoring>
{
    public override void Bake(FollowEntityAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new FollowEntity());
    }
}
