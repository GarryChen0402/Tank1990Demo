using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleImage : MonoBehaviour
{
    public TMP_Text info;

    private void Awake()
    {
        info.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("GameScene");
        }else if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }

    public void DisplayInfo()
    {
        info.gameObject.SetActive(true);
    }
}
