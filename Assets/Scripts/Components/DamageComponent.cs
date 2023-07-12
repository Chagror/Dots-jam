using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;


public struct DamageComponent : IComponentData
{
    public float damage; 
    public float delayBetweenDamage;
    public float nextTimeDamage;

}