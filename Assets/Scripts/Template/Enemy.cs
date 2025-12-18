using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Attack Settings")]
    public GameObject bulletPrefabs;
    [SerializeField] protected int leftBulletCount;
    [SerializeField] protected bool canAttack;
    [SerializeField] protected float attackTimer;
    [SerializeField] protected float attackTime;

    public GameObject explosionPrefab;

    protected bool isFreeze;

    protected virtual void Awake()
    {
        EnemyManager.Instance.AddEnemy(gameObject);
        isFreeze = false;
    }

    public virtual void GetShot()
    {
        //Debug.Log("Common Enemy Get shot");
        AudioManager.Instance?.PlayFx("Blast");
        GameObject.Instantiate(explosionPrefab, transform.position, transform.rotation);
        //Destroy(GameObject.Instantiate(explosionPrefab, transform.position, transform.rotation));
        EnemyManager.Instance.RemoveEnemy(gameObject);
        Destroy(gameObject);
    }

    public void BulletBroken()
    {
        leftBulletCount++;
    }

    public void Freeze()
    {
        isFreeze = true;
    }

    public void UnFreeze()
    {
        isFreeze = false;
    }
}
