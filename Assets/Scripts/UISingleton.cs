using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class UISingleton : MonoBehaviour
{
    public static UISingleton instance { get; private set; }
    [SerializeField] private TMP_Text txt_enemyCounter;
    public int counterEnemy = 0;

    [SerializeField] private TMP_Text txt_health;
    public RectTransform fillBar;

    private void Awake()
    {
        if (instance != null) Destroy(this);
        else { instance = this; }
    }

    public void AddEnemy(int amountToAdd = 1)
    {
        counterEnemy += amountToAdd;
        txt_enemyCounter.text = counterEnemy.ToString();
    }

    public void UpdateHealth(int health)
    {
        txt_health.text = health.ToString();

        fillBar.localScale = new Vector3(health/100, 1, 1);
    }
}
