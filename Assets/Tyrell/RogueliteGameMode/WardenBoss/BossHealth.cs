using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealth : MonoBehaviour
{

    public float Health = 100;
    public float MaxHealth = 100;

    public float XpGiven = 500;

    public TextMeshProUGUI Name;
    public Slider HealthBar;

    public GameObject EnemyDeathParticle;
    public Color customColor;

    private void Update()
    {
        HealthBar.value = CalculateHealth();

        if (Health <= 0)
        {
            EnemyKilled("BossKilled");
            
        }
    }

    void EnemyKilled(string Name)
    {
        GameObject enemyDeathParticle = Instantiate(EnemyDeathParticle, transform.position, Quaternion.identity);
        enemyDeathParticle.GetComponent<EnemyDeathParticle>().newColor = customColor;
        Destroy(enemyDeathParticle, 2);

        AchievementManager.instance.AddAchievementProgress(Name, 1);
        LevelSystem.instance.GainExperienceScalable(XpGiven, LevelSystem.instance.level);
        MoneyManager.instance.DropMoney();
        DestroyWardenBoss();
    }

    public void EnemyTakeDamage(float amount, bool isCrit)
    {
        Health -= amount;
        float damageSpawn = Random.Range(0, 4);
        Vector3 enemyPos = new Vector3(transform.position.x + damageSpawn, transform.position.y + 15, transform.position.z);
        DamagePopUp.Create(enemyPos, amount, isCrit);
    }

    public void DestroyWardenBoss()
    {
        Destroy(gameObject);
    }

    float CalculateHealth()
    {
        return Health / MaxHealth;
    }

}
