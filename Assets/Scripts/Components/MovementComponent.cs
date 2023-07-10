using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct MovementComponent : IComponentData
{
    public Vector2 dir;
    public float speed;
}
