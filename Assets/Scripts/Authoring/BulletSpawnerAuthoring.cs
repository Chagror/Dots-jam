using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Physics;

public class BulletSpawnerAuthoring : MonoBehaviour
{

    public GameObject bulletPrefab;
    public Vector3 spawnPos;
    public float nextSpawnTime;
    public float spawnRate;
    public float radius;
    public int numBullets;
    public float angle;
    public CollisionFilter Filter;
    public float range;

}

class ProjectileSpawnerBaker : Baker<BulletSpawnerAuthoring>
{
    public override void Bake(BulletSpawnerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        //Debug.Log(entity);

        AddComponent(entity, new ProjectileSpawner
        {
            bulletPrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
            spawnPos = authoring.transform.position,
            nextSpawnTime = 0.0f,
            spawnRate = authoring.spawnRate,
            radius = authoring.radius,
            numBullets = authoring.numBullets,
            angle = authoring.angle,
            filter = authoring.Filter,
            range = authoring.range


        }) ;
    }
}
