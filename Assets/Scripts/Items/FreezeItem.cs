using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeItem : BasicItem
{

    protected override void Update()
    {
        base.Update();
    }

    public override void OnCollected()
    {
        EnemyManager.Instance.isFreeze = true;
        base.OnCollected();
    }
}
