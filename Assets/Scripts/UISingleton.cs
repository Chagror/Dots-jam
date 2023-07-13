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

    [SerializeField] private RectTransform arrow;

    private void Awake()
    {
        if (instance != null) Destroy(this);
        else { instance = this; }
    }

    public void AddEnemy(int amountToAdd = 1)
    {
        counterEnemy += amountToAdd;
        txt_enemyCounter.text = "Enemies : " + counterEnemy.ToString();
    }

    public void SetEnemy(int newAmount)
    {
        counterEnemy = newAmount;
        txt_enemyCounter.text = "Enemies : " + counterEnemy.ToString();
        txt_playerCounter.text = "Player : 4,3";
    }

    public void UpdateHealth(float health, float maxHealth)
    {
        float percent = (float)health / (float)maxHealth;

        txt_health.text = health.ToString();
        fillBar.localScale = new Vector3 (percent, 1.0f, 1.0f);
    }

    public void RotateColisArrow(float angle)
    {
        //arrow.gameObject.SetActive(true);
        //arrow.eulerAngles = new Vector3(0,0,angle);
    }
}
