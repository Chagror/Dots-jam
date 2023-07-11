using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct Impact : IComponentData
{
    public Entity Prefab;
    public int MaxImpactCount;
}
