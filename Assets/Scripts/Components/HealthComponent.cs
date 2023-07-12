using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;


public struct HealthComponent : IComponentData
{
    public int maxHealth;
    public int currentHealth;
    public float delayBetweenDamage;
    public float nextTimeCanBeDamaged;
}
