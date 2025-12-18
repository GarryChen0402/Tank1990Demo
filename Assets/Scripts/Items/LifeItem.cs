using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeItem : BasicItem
{
    protected override void Update()
    {
        base.Update();
    }

    public override void OnCollected()
    {
        PlayerTank.Instance.AddLife();
        base.OnCollected();
    }
}
