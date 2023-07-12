using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct InputGetterComponent : IComponentData
{
    public float2 movement;
    public float speed;
    public bool shoot;
}