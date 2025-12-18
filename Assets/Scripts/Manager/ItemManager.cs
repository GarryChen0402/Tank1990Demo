using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonClass<ItemManager>
{
    public List<GameObject> itemTypePrefabs;
    private List<GameObject> livingItems;

    private bool canSpawnItem;

    [SerializeField] private float spawnTime;
    [SerializeField] private float spawnTimer;

    protected override void Awake()
    {
        base.Awake();
        livingItems = new List<GameObject>();
        spawnTime = 15f;
        spawnTimer = 0f;
    }

    private void Update()
    {
        if (!canSpawnItem) return;
        spawnTimer += Time.deltaTime;
        if(spawnTimer >= spawnTime)
        {
            spawnTimer -= spawnTime;
            SpawnRandomItem();
        }
    }


    private void SpawnRandomItem()
    {
        GameObject item = itemTypePrefabs[Random.Range(0, itemTypePrefabs.Count)];
        Vector3 position = new Vector3(Random.Range(-12f, 12f), Random.Range(-12f, 12f), 0);
        GameObject.Instantiate(item, position, Quaternion.identity);
    }

    public void StartSpawnItem()
    {
        canSpawnItem = true;
    }

    public void StopSpawnItem()
    {
        canSpawnItem = false;
    }

    public void AddItem(GameObject item)
    {
        livingItems.Add(item);
    }

    public void RemoveItem(GameObject item)
    {
        livingItems.Remove(item);
    }

    //public void ClearAllItems()
    //{
    //    foreach (GameObject item in livingItems) item.GetComponent<BasicItem>().RemoveItem();
    //}
}
