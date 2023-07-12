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
    [SerializeField] private TMP_Text txt_playerCounter;
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

    public void SetEnemy(int newAmount)
    {
        counterEnemy = newAmount;
        txt_enemyCounter.text = counterEnemy.ToString();
    }

    public void UpdateHealth(float health, float maxHealth)
    {
        float percent = health / maxHealth;
        float fillPercent = fillBar.localScale.x * percent;

        txt_health.text = health.ToString();
        fillBar.localScale = new Vector3 (fillPercent, 1.0f, 1.0f);
    }
}
