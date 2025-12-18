using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BossWallsState
{
    Brick,
    Steel
}

public class BossBrickManager : SingletonClass<BossBrickManager>
{
    public List<GameObject> brickWalls;
    public List<GameObject> steelWalls;
    private float steelTime;
    private float steelTimer;
    private BossWallsState state;

    protected override void Awake()
    {
        base.Awake();
        steelTime = 10;
        steelTimer = 0;
    }

    private void Update()
    {
        switch (state)
        {
            case BossWallsState.Brick:
                BrickUpdate();
                break;
            case BossWallsState.Steel:
                SteelUpdate();
                break;
        }
    }

    private void BrickUpdate()
    {

    }

    private void SteelUpdate()
    {
        steelTimer += Time.deltaTime;
        if (steelTimer >= steelTime)
        {
            steelTimer = 0;
            SwitchToBrick();
        }
    }

    public void SwitchToSteel()
    {
        foreach(GameObject brick in  brickWalls) brick.SetActive(false);
        foreach(GameObject steel in steelWalls) steel.SetActive(true);
        state = BossWallsState.Steel;
    }

    public void SwitchToBrick()
    {
        foreach (GameObject brick in brickWalls) brick.SetActive(true);
        foreach (GameObject steel in steelWalls) steel.SetActive(false);
        state = BossWallsState.Brick;
    }
}
