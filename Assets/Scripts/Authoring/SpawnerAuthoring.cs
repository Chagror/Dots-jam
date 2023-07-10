using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

class SpawnerAuthoring : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public float spawnRate;
    public float innerRadius;
    public float outerRadius;
}

class SpawnerBaker : Baker<SpawnerAuthoring>
{
    public override void Bake(SpawnerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        Debug.Log(entity);

        AddComponent(entity, new EnemySpawner
        {
            enemyPrefab = GetEntity(authoring.prefabToSpawn, TransformUsageFlags.Dynamic),
            spawnPos = authoring.transform.position,
            nextSpawnTime = 0.0f,
            spawnRate = authoring.spawnRate,
            innerRadius = authoring.innerRadius,
            outerRadius = authoring.outerRadius
        });
    }
}