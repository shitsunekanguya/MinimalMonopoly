using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public static void Restart()
    {
        
        GamePlayManage.m_Players = new();
        GamePlayManage.PlayerOver = new();
        SettingData.PlayerAi = new();
        SettingData.PlayerName = new();
        SettingData.Avatar = new();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
