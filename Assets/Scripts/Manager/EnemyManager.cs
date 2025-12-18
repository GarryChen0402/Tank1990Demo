using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonClass<EnemyManager>
{
    [Header("Enemy Settings")]
    public List<GameObject> livingEnemy;
    public List<Transform> enemySpawnPointList;
    public List<GameObject> enemyTypes;
    [SerializeField] private float spawnTime;
    [SerializeField] private float spawnTimer;
    public GameObject SpawnStarPrefab;
    [SerializeField] private bool canSpawnEnemy;
    [SerializeField] private int MaxLivingEnemyCount = 6;

    [Header("Level Settings")]
    [SerializeField] private int enemyPerLevel;
    [SerializeField] private int spawnedEnemyCount;

    [Header("Freeze Settings")]
    [SerializeField] public bool isFreeze;
    [SerializeField] private float freezeTime;
    [SerializeField] private float freezeTimer;

    public bool CanSpawnEnemy { get => canSpawnEnemy; set => canSpawnEnemy = value; }

    public bool isAllEnemySpawned { get => spawnedEnemyCount >= enemyPerLevel; }
    public bool isAllEnemyCleared { get => livingEnemy.Count == 0; }

    protected override void Awake()
    {
        base.Awake();

        spawnTime = 0.5f;
        spawnTimer = 0;
        canSpawnEnemy = false;
        spawnedEnemyCount = 0;
        enemyPerLevel = 5;
        isFreeze = false;
        freezeTime = 10;
        freezeTimer = 0;
    }

    private void Update()
    {
        if (livingEnemy.Count < MaxLivingEnemyCount) SpawnEnemyUpdate();
        if (isFreeze)
        {   
            FreezeAllEnemy();
            //isFreeze = false;
            freezeTimer += Time.deltaTime;
            if(freezeTimer > freezeTime)
            {
                freezeTimer = 0;
                isFreeze = false;
                UnFreezeAllEnemy();
            }
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        livingEnemy.Remove(enemy);
    }

    public void AddEnemy(GameObject enemy)
    {
        livingEnemy.Add(enemy);
    }

    public void DisableAllEnemy()
    {
        foreach (GameObject enemy in livingEnemy)
        {
            enemy.SetActive(false);
        }
    }

    public void EnableAllEnemy()
    {
        foreach(GameObject enemy in livingEnemy)
        {
            enemy.SetActive(true);
        }
    }

    private void SpawnEnemyUpdate()
    {
        if (!canSpawnEnemy) return;
        if (spawnedEnemyCount >= enemyPerLevel) return;
        spawnTimer += Time.deltaTime;
        if(spawnTimer >= spawnTime)
        {
            spawnTimer -= spawnTime;
            spawnedEnemyCount++;
            //SpawnSingleEnemy(enemySpawnPointList[Random.Range(0, enemySpawnPointList.Count)].position);
            GameObject.Instantiate(SpawnStarPrefab, enemySpawnPointList[Random.Range(0, enemySpawnPointList.Count)].position, Quaternion.identity);
        }
    }

    public void SpawnSingleEnemy(Vector3 position)
    {
        GameObject.Instantiate(enemyTypes[Random.Range(0, enemyTypes.Count)], position, Quaternion.identity);
    }

    public void ResetState()
    {
        canSpawnEnemy = true;
        spawnTimer = 0;
        spawnedEnemyCount = 0;
    }

    private void FreezeAllEnemy()
    {
        foreach (GameObject enemy in livingEnemy)enemy.GetComponent<Enemy>().Freeze();
    }

    private void UnFreezeAllEnemy()
    {
        foreach(GameObject enemy in livingEnemy)enemy.GetComponent<Enemy>().UnFreeze();
    }
}
