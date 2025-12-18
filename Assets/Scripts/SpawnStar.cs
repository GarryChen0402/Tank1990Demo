using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStar : MonoBehaviour
{
    public void OnAnimEnd()
    {
        EnemyManager.Instance.SpawnSingleEnemy(transform.position);
        Destroy(gameObject);
    }
}
