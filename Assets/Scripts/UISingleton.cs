using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISingleton : MonoBehaviour
{
    public static UISingleton instance { get; private set; }
    [SerializeField]
    private TMP_Text enemyCounter;
    public int counterEnemy = 0;

    private void Awake()
    {
        if (instance != null) Destroy(this);
        else { instance = this; }
    }

    public void AddEnemy(int amountToAdd = 1)
    {
        counterEnemy += amountToAdd;
        enemyCounter.text = counterEnemy.ToString();
    }
}
