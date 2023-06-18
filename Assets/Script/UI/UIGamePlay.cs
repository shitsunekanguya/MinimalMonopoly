using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GamePlayManage;
using static PlayerDecision;
using Unity.VisualScripting;

[System.Serializable]
public class PlayerUI
{
    public GameObject PlayerWindow;
    public TMP_Text PlayerName,Life;
    public Color color;
    public bool isActive;
}

public class UIGamePlay : MonoBehaviour,INextTurn
{
    public List<PlayerUI> playerUIs = new();
    public List<TMP_Text> PDashBoardUIs = new();
    [SerializeField] private Vector3 MaxSize, MinSize;

    public GameObject DiceDrag, NextTurnNote,DiceResult,HPChangeNote,CheckingDice, WinningPanel,DashBoardObj;
    public TMP_Text WinnerName;
    public GamePlayManage gamePlayManage;

    public void NextTurn()
    {
        DiceDrag.SetActive(true);
        int PlayerTurn = GamePlayManage.currentPlayerOrder-1;
        for (int i = 0; i < playerUIs.Count; i++)
        {
            var newScale = i == PlayerTurn ? MaxSize : MinSize;
            var newColor = i == PlayerTurn ? GamePlayManage.m_Players[i].color: Color.white;
            if (playerUIs[i].isActive)
            {
                playerUIs[i].PlayerWindow.transform.localScale = newScale;
                playerUIs[i].PlayerName.color = newColor;
            }           
        }
        
    }
    public void SetPlayerStat()
    {
        for (int i = 0; i < GamePlayManage.m_Players.Count; i++)
        {
            playerUIs[i].PlayerWindow.SetActive(true);
            playerUIs[i].PlayerName.text = GamePlayManage.m_Players[i].PlayerName;
            playerUIs[i].Life.text = SettingData.Life.ToString();
        }
    }

    public void DiceNotificate(bool isActive)
    {
        if (currentPlayer.isAI)
        { DiceDrag.SetActive(false); }
        else
        { DiceDrag.SetActive(isActive); }
            
    }
    public void CheckingDiceNote(bool isActive)
    {
        CheckingDice.SetActive(isActive);
    }

    public void PlayerNotificate(bool isAvtive)
    {
        if (!isAvtive)
        { NextTurnNote.SetActive(isAvtive); return; }
        Player player = GamePlayManage.currentPlayer;
        NextTurnNote.GetComponent<TMP_Text>().text = player.PlayerName + " Turn";
        NextTurnNote.GetComponent<TMP_Text>().color = player.color;
        NextTurnNote.SetActive(isAvtive);
    }

    public void HPChanged(int value,bool isActive)
    {
        if (!isActive)
        { HPChangeNote.SetActive(isActive); }
        else
        {
            Player player = GamePlayManage.currentPlayer;
            string playerName = player.PlayerName;

            TMP_Text HPtext = HPChangeNote.GetComponentInChildren<TMP_Text>();

            if (value > 1)
            {
                HPtext.text = "!Super Heal!" + "\n" + playerName + "'s Life +" + value;
                HPtext.color = Color.green;
            }
            else if (value == 1)
            {
                HPtext.text = "Heal" + "\n" + playerName + "'s Life +" + value;
                HPtext.color = Color.green;
            }
            else if (value < 0)
            {
                HPtext.text = "!Damage!" + "\n" + playerName + "'s Life " + value;
                HPtext.color = Color.red;
            }

            else if (value == 0)
            {
                HPtext.text = playerName + "\n" + "GAME OVER";
                HPtext.color = Color.grey;

                //GameOver
                gamePlayManage.PlayerGameOver();
            }

            HPtext.fontSize =30;
            HPChangeNote.SetActive(isActive);
        }
      
    }

    public void UIGameOver(string name,bool isActive)
    {
        if (!isActive)
        { 
            HPChangeNote.SetActive(isActive);
            gamePlayManage.PlayerGameOver();
            return;
        }
       
        TMP_Text HPtext = HPChangeNote.GetComponentInChildren<TMP_Text>();

        HPtext.text = name + "\n" + "GAME OVER";
        HPtext.color = Color.grey;
        HPChangeNote.SetActive(isActive);
       
    }
    private void Winning(bool isActive)
    {
        WinnerName.text = GamePlayManage.WinnerPlayer.PlayerName;
        WinnerName.color = GamePlayManage.WinnerPlayer.color;
        WinningPanel.SetActive(isActive);
        if (!isActive)
        {
            DashBoard();
        }
    }

    private void DashBoard()
    {
        DashBoardObj.SetActive(true);
        for (int i = 0; i < PDashBoardUIs.Count; i++)
        {
            var tempPlayer = GamePlayManage.PlayerOver;
            PDashBoardUIs[i].enabled = i < tempPlayer.Count;          
            if (i < tempPlayer.Count)
            {
                string name = (i+1).ToString() + " " + tempPlayer[i].PlayerName;
                
                if (i == tempPlayer.Count - 1)
                {
                    PDashBoardUIs[i].color = Color.yellow;
                    name += "  (! WINNER !)";
                }
                PDashBoardUIs[i].text = name;
            }           
        }
    }

    public void DoReStartGame()
    {
        RestartGame.Restart();
    }

    public void ShowDiceResult(bool isActive)
    {
        string resultText = "";
        for (int i = 0; i < TileCreat.DiceInstance.Count; i++)
        { 
            resultText += "Dice" + (i+1) + " result = " + TileCreat.DiceInstance[i].GetComponent<DicePlay>().result + "\n";
        }
        resultText += "<color=yellow><b>" + "Total = " + GamePlayManage.DiceResult;
        DiceResult.GetComponent<TMP_Text>().text = resultText;
        DiceResult.SetActive(isActive);
    }

    public void EndTurn(bool isActive)
    {
        playerUIs[GamePlayManage.currentPlayerOrder - 1].Life.text = GamePlayManage.currentPlayer.Life.ToString();

        if (!isActive)
        { NextTurnNote.SetActive(isActive); gamePlayManage.AllNextTurn(); return; }

        NextTurnNote.GetComponent<TMP_Text>().text = "End Turn " + GamePlayManage.currentPlayer.PlayerName;
        NextTurnNote.GetComponent<TMP_Text>().color = Color.white;
        NextTurnNote.SetActive(isActive);
                
    }

    public void PlayerStatOver(int order)
    {
        playerUIs[order].PlayerWindow.transform.localScale = MinSize;
        playerUIs[order].PlayerName.GetComponent<TMP_Text>().color = Color.grey;
        playerUIs[order].Life.GetComponent<TMP_Text>().color= Color.grey;
        playerUIs[order].Life.GetComponent<TMP_Text>().GetComponent<TMP_Text>().text = "0"; 
        playerUIs[order].isActive = false; 
    }

    public IEnumerator NoteTimer(float startTime,float endTime, MyDelegate tempDelegate)
    {
        yield return new WaitForSeconds(startTime);
        tempDelegate(true);
        yield return new WaitForSeconds(endTime);
        tempDelegate(false);
    }
    public IEnumerator NoteTimerValue(float startTime, float endTime, DelegateGameplay tempDelegate,int value)
    {
        yield return new WaitForSeconds(startTime);
        tempDelegate(value,true);
        yield return new WaitForSeconds(endTime);
        tempDelegate(value,false);
    }
    public IEnumerator GameOverTimer(float startTime, float endTime,string name)
    {
        yield return new WaitForSeconds(startTime);
        UIGameOver(name,true);
        yield return new WaitForSeconds(endTime);
        UIGameOver(name,false);
        yield return new WaitForSeconds(2.5f);
        if (SettingData.gameState == State.Winning)
        {
            StartCoroutine(NoteTimer(0.1f, 3f, Winning));
        }
    }

}
