using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;


public struct BulletComponent : IComponentData
{
    public Vector3 direction;
    public float speed;
    public float nbHits;
    public float damage;
    public float lifeTime;
}