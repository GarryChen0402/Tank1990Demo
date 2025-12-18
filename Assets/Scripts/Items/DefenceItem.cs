using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceItem : BasicItem
{
    protected override void Update()
    {
        base.Update();
    }

    public override void OnCollected()
    {
        BossBrickManager.Instance.SwitchToSteel();
        base.OnCollected();
    }
}
