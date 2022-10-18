using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyHealth : MonoBehaviour
{
    public float Health = 35;
    public float MaxHealth = 35;
    public float XpGiven;

    public Vector3 pubDamageSpawn = Vector3.zero;

    public EnemyAiController AiController;

    public Slider HealthSlider;

    public GameObject EnemyDeathParticle;
    public Color customColor;
    

    private void Update()
    {
        HealthSlider.value = CalculateHealth();

        if (Health <= 0)
        {
            EnemyKilled("EnemiesKilled");
        }

        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }


    public virtual void EnemyKilled(string Name)
    {
        GameObject enemyDeathParticle = Instantiate(EnemyDeathParticle, transform.position, Quaternion.identity);
        enemyDeathParticle.GetComponent<EnemyDeathParticle>().newColor = customColor;
        Destroy(enemyDeathParticle, 2);


        AchievementManager.instance.AddAchievementProgress(Name, 1);
        LevelSystem.instance.GainExperienceScalable(XpGiven, LevelSystem.instance.level);
        MoneyManager.instance.DropMoney();
        AiController.DestroyEnemy();
    }

    public void EnemyTakeDamage(float amount, bool isCrit)
    {
        Health -= amount;
        float damageSpawn = Random.Range(0, 4);
        Vector3 enemyPos = new Vector3(transform.position.x + damageSpawn, transform.position.y + 5, transform.position.z);

        DamagePopUp.Create(enemyPos + pubDamageSpawn, amount, isCrit);

        AiController.IncreaseSightRange();
    }

    public void EnemyGainHealth(float amount)
    {
        Health += amount;
    }


    float CalculateHealth()
    {
        return Health / MaxHealth;
    }

    

}
