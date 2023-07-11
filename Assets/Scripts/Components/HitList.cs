using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct HitList : IBufferElementData
{
    public Entity Entity;
}