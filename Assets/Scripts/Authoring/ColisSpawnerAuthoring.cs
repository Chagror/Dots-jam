using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

class ColisSpawnerAuthoring : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnRate;
    public float innerRadius;
    public float outerRadius;
}

class ColisSpawnerBaker : Baker<ColisSpawnerAuthoring>
{
    public override void Bake(ColisSpawnerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        //Debug.Log(entity);

        AddComponent(entity, new ColisSpawner
        {
            colisPrefab = GetEntity(authoring.prefabToSpawn, TransformUsageFlags.Dynamic),
            spawnPos = authoring.transform.position,
            nextSpawnTime = 0.0f,
            spawnRate = authoring.spawnRate,
            innerRadius = authoring.innerRadius,
            outerRadius = authoring.outerRadius
        });
    }
}