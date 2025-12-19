using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

enum GameState
{
    Gaming,
    Waiting,
    Win,
    Loose,

}

public class GameManager : SingletonClass<GameManager>
{
    [SerializeField] private GameState gameState;

    public GameObject LooseImage;

    private float waitTimer;
    private float waitTime;

    [Header("Level Setting")]
    public List<GameObject> levelMapList;
    [SerializeField] private int currentLevel;

    [Header("Waiting State Resouces")]
    public GameObject waitingImage;
    public TMP_Text waitingImageText;

    protected override void Awake()
    {
        base.Awake();
        //AudioManager.Instance.PlayFx("Start");
        waitTime = 2f;
        currentLevel = 0;
    }

    private void Start()
    {
        //PlayStartMusic();
        //LevelStart();
        SwitchToWaiting();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        switch (gameState)
        {
            case GameState.Gaming:
                GamingUpdate();
                break;
            case GameState.Win:
                WinUpdate();
                break;
            case GameState.Loose:
                LooseUpdate();
                break;
            case GameState.Waiting:
                WaitingUpdate();
                break;
        }
    }

    private void GamingUpdate()
    {
        if (EnemyManager.Instance.isAllEnemySpawned && EnemyManager.Instance.isAllEnemyCleared) LevelComplete();
    }

    private void LooseUpdate()
    {
        if (Input.anyKeyDown)
        {
            //Debug.Log("Quit");
            //Application.Quit();
            SceneManager.LoadScene("TitleScene");
        }
    }

    private void WaitingUpdate()
    {
        waitTimer += Time.deltaTime;
        if(waitTimer >= waitTime)
        {
            waitingImage.SetActive(false);
            LevelStart();
            waitTimer = 0;
        }
    }

    private void WinUpdate()
    {
        if (Input.anyKeyDown)
        {
            //Debug.Log("Quit");
            SceneManager.LoadScene("TitleScene");
            //Application.Quit();
        }
    }

    public void SwitchToLoose()
    {
        gameState = GameState.Loose;
        LooseImage.SetActive(true);
        EnemyManager.Instance.DisableAllEnemy();
        EnemyManager.Instance.CanSpawnEnemy = false;
    }

    public void SwitchToGaming()
    {
        gameState = GameState.Gaming;
    }

    public void SwitchToWaiting()
    {
        waitingImageText.text = $"Stage {currentLevel + 1}";
        waitingImage.SetActive(true);
        gameState = GameState.Waiting;
        waitTimer = 0;
    }

    public void SwitchToWin()
    {
        gameState = GameState.Win;
        waitingImageText.text = "All stage cleared, you win!!\n Congratulations.\n Press any key to return.";
        waitingImage.SetActive(true);
    }

    private void PlayStartMusic()
    {
        AudioManager.Instance.PlayFx("Start");
    }

    private void LevelStart()
    {
        SwitchToGaming();


        PlayerTank.Instance.ResetState();
        PlayerTank.Instance.CanControl();
        if(currentLevel > 0)levelMapList[currentLevel - 1].SetActive(false);
        levelMapList[currentLevel].SetActive(true);

        EnemyManager.Instance.ResetState();
        ItemManager.Instance.StartSpawnItem();
        PlayStartMusic();
        //EnemyManager.Instance.
    }

    private void LevelComplete()
    {
        EnemyManager.Instance.CanSpawnEnemy = false;
        ItemManager.Instance.StopSpawnItem();
        //ItemManager.Instance.ClearAllItems();
        currentLevel++;
        if (currentLevel >= levelMapList.Count) SwitchToWin();
        else SwitchToWaiting();
    }
}
