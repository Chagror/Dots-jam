using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ImpactAuthoring : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Prefab;

}

class ImpactBaker : Baker<ImpactAuthoring>
{

    public override void Bake(ImpactAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        //Debug.Log(entity);

        AddComponent(entity, new Impact
        {
            Prefab = GetEntity( authoring.Prefab, TransformUsageFlags.None),

        }) ;
    }
}
