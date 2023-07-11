using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class BulletAuthoring : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float nbHits;
    public float damage;
    public float lifeTime;
    
    

}

class BulletBaker : Baker<BulletAuthoring>
{
    public override void Bake(BulletAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        Debug.Log(entity);

        AddComponent(entity, new BulletComponent
        {
            direction = authoring.direction,
            speed = authoring.speed,
            nbHits = authoring.nbHits,
            damage = authoring.damage,
            lifeTime = authoring.lifeTime,
        });

    }
}
