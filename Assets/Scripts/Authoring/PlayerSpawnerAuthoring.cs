using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

class PlayerSpawnerAuthoring : MonoBehaviour
{
    public GameObject playerPrefab;
}

class PlayerSpawnerBaker : Baker<PlayerSpawnerAuthoring>
{
    public override void Bake(PlayerSpawnerAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new PlayerSpawnerComponent
        {
            playerPrefab = GetEntity(authoring.playerPrefab, TransformUsageFlags.Dynamic),
            spawnPos = authoring.transform.position
        });
    }
}