using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

public partial struct IndicatorDisplay : ISystem
{
    (RefRW<LocalTransform>, RefRO<ColisTag>) colisEntity;
    (RefRW<LocalTransform>, RefRO<PlayerTag>) playerEntity;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var colis in SystemAPI.Query<RefRW<LocalTransform>, RefRO<ColisTag>>())
        {
            colisEntity = colis;
        }

        foreach (var player in SystemAPI.Query<RefRW<LocalTransform>, RefRO<PlayerTag>>())
        {
            playerEntity = player;
        }

        //if (colisEntity.Item2.ValueRO.IsUnityNull())
            //return;

        //UISingleton.instance.RotateColisArrow(DistanceBetweenEntity(colisEntity, playerEntity));
    }

    private float DistanceBetweenEntity((RefRW<LocalTransform>, RefRO<ColisTag>) entity1, (RefRW<LocalTransform>, RefRO<PlayerTag>) entity2)
    {
        return(Vector3.Angle(entity1.Item1.ValueRW.Position, entity2.Item1.ValueRW.Position));
    }
}
