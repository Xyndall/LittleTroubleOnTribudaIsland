using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChallengeRoomSpawn : EnemyRoomSpawn
{



    //[SerializeField]
    //private Transform[] chaspawnZones; // Array filled with spawn zones transform

    //[SerializeField]
    //private GameObject[] enemyPrefabs; // All available enemy prefabs stored here

    [SerializeField]
    private GameObject[] elitePrefabs;

    [SerializeField]
    public List<GameObject> challengeenemyList = new List<GameObject>();


    public override void FixedUpdate()
    {
        enemyList.RemoveAll(GameObject => GameObject == null);
    }

    public override void Start()
    {


        maxEnemySpawn = RoomManager.instance.CalculateEnemySpawns();

        StartCoroutine(ChallengeWaitForPlayer());

    }



    IEnumerator ChallengeWaitForPlayer()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < maxEnemySpawn; i++)
        {
            SpawnEnemies();
        }
        for (int i = 0; i < RoomManager.instance.EliteEnemies; i++)
        {
            SpawnElite();
        }
        
    }

    public override void ShowNewItems()
    {
        base.ShowNewItems();
        ShowItems.instance.ShowItemChoice();
    }

    void SpawnElite()
    {
        int spawnNum = Random.Range(0, spawnZones.Length);
        int enemyNum = Random.Range(0, elitePrefabs.Length);

        GameObject eliteEnemy = Instantiate(elitePrefabs[enemyNum], spawnZones[spawnNum].transform.position, Quaternion.identity, transform);
        eliteEnemy.GetComponent<EnemyHealth>().MaxHealth += RoomManager.instance.CalculateEnemyHealthScaler();
        eliteEnemy.GetComponent<EnemyHealth>().Health += RoomManager.instance.CalculateEnemyHealthScaler();
        eliteEnemy.GetComponent<EnemyHealth>().XpGiven = RoomManager.instance.difficultyXp + 20;
        eliteEnemy.GetComponent<EnemyAiController>().roomspawn = this;
        eliteEnemy.GetComponent<EnemyAiController>().IsRogueLite = true;
        enemyList.Add(eliteEnemy);
    }


}
