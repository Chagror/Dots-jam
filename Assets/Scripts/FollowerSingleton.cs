using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerSingleton : MonoBehaviour
{
    public static FollowerSingleton instance;

    private void Awake()
    {
        if (instance != null) Destroy(this);
        else
        {
            instance = this;
        }
    }
}
