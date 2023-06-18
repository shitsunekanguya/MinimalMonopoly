using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GamePlayManage : MonoBehaviour
{
    public delegate void MyDelegate(bool m);
    public delegate void DelegateGameplay(int i,bool m);

    public static List<Player> m_Players=new();
    public static List<Player>PlayerOver=new();
    public static Player currentPlayer,WinnerPlayer;
    public static int currentPlayerOrder; //-1
    public static int DiceResult;

    public static UIGamePlay uIGamePlay;
    private UIDecision uIDecision;
    private TileCamera tileCamera;

    [SerializeField] private AIDiceRolling aIDiceRolling;

    //error prevent
    private int diceReady;

    void Start()
    {
        currentPlayerOrder = SettingData.PlayerToStart-1;  
        uIGamePlay = FindAnyObjectByType<UIGamePlay>();
        uIDecision = FindAnyObjectByType<UIDecision>();
        tileCamera = FindAnyObjectByType<TileCamera>();
    }
    public void GetPlayer()
    {
        for (int i = 0; i < m_Players.Count; i++)
        {
            m_Players[i].PlayerOrder = i + 1;            
            m_Players[i].isAI = SettingData.PlayerAi[i];

            string playerName = SettingData.PlayerName[i]!=""? SettingData.PlayerName[i] : m_Players[i].isAI ? 
                "Player " + m_Players[i].PlayerOrder.ToString()+ " (AI)" : "Player " + m_Players[i].PlayerOrder.ToString();
            m_Players[i].PlayerName = playerName;
        }
        uIDecision.SetUI();
    }

    public void AllNextTurn()
    {
        if (SettingData.gameState == State.MapCreating)
        {           
            uIGamePlay.SetPlayerStat();
        }
        ClearDice();
        diceReady = 0;
        StartCoroutine(uIGamePlay.NoteTimer(0.1f, 3f, uIGamePlay.PlayerNotificate));
        

        var NextTurnObj = FindAllINextTurn();

        currentPlayerOrder++;
        if (currentPlayerOrder > m_Players.Count)
        { currentPlayerOrder = 1; }

        while (m_Players[currentPlayerOrder-1] == null)
        { currentPlayerOrder++;
            if (currentPlayerOrder > m_Players.Count)
            { currentPlayerOrder = 1; }
        }

        currentPlayer = m_Players[currentPlayerOrder-1];

        foreach (INextTurn obj in NextTurnObj)
        { 
            obj.NextTurn();
        }

        if (currentPlayer.isAI)
        { SettingData.gameState = State.AIPlaying; aIDiceRolling.enabled = true; }
        else
        { SettingData.gameState = State.RollingDice; }

        StartCoroutine(uIGamePlay.NoteTimer(0.1f, 5f, uIGamePlay.DiceNotificate));
    }
    private List<INextTurn> FindAllINextTurn()
    {
        IEnumerable<INextTurn> NextTurnObj = FindObjectsOfType<MonoBehaviour>().OfType<INextTurn>();
        return new List<INextTurn>(NextTurnObj);
    }

    public void CheckDice()
    { StartCoroutine(uIGamePlay.NoteTimer(0.1f, 7.5f, uIGamePlay.CheckingDiceNote)); }

    public void GetDiceResult()
    {
        diceReady++;
        if (diceReady == TileCreat.DiceInstance .Count && SettingData.gameState != State.Walking)
        { 
            DiceResult = 0;
            foreach (GameObject dice in TileCreat.DiceInstance)
            {
                DiceResult += dice.GetComponent<DicePlay>().result;                                            
            }
            
            StartCoroutine(uIGamePlay.NoteTimer(0.1f, 4f, uIGamePlay.ShowDiceResult));
            tileCamera.MoveToPlayer();
            SettingData.gameState = State.Walking;
            PlayerWalk();
        }                   
    }

    void ClearDice()
    {
        foreach (GameObject dice in TileCreat.DiceInstance)
        {
            Destroy(dice);
        }
        TileCreat.DiceInstance.Clear();

    }

    public void PlayerWalk()
    {
        currentPlayer.StartWalk();
    }

    public void PlayerGameOver()
    {
        uIGamePlay.PlayerStatOver(currentPlayerOrder - 1);
        var GameOverObj = FindAllIGameOver();
        foreach (IGameOver obj in GameOverObj)
        {
            obj.GameOverOn(currentPlayer.m_chess);
        }
        PlayerOver.Add(currentPlayer);
        m_Players[currentPlayerOrder - 1] = null;
        currentPlayer.enabled = false;

        int playerRemain = 0;
        string remainName = "";
        foreach (Player p in m_Players)
        { 
            if (p != null) 
            { 
                playerRemain++; remainName = p.PlayerName;
                WinnerPlayer = p;
            } 
        }

        if (playerRemain > 1)
        { AllNextTurn(); }
        else 
        {
            SettingData.gameState = State.Winning;
            PlayerOver.Add(WinnerPlayer);
        }
    }
    private List<IGameOver> FindAllIGameOver()
    {
        IEnumerable<IGameOver> GameOverObj = FindObjectsOfType<MonoBehaviour>().OfType<IGameOver>();
        return new List<IGameOver>(GameOverObj);
    }
}
