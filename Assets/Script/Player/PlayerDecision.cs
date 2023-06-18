using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;
using static GamePlayManage;

public class PlayerDecision : MonoBehaviour
{
    public delegate void TileDelegate(int i);
    List<TileGen> tileGens => TileSpawnPoint.tileGens;

    public Player player;
    private UIGamePlay uIGamePlay => GamePlayManage.uIGamePlay;
    private UIDecision uIDecision;
    private AIDecision aIDecision;
    public SlotPlay slotPlay;

    void Start()
    {
        uIDecision = FindObjectOfType<UIDecision>();
        aIDecision = FindObjectOfType<AIDecision>();
    }

    public void WalkComplete(int TargetTileNum)
    {
        player = GamePlayManage.currentPlayer;
        if (tileGens[TargetTileNum].isBase)
        {
            StartCoroutine(DelayFunction(0.5f, Heal, TargetTileNum));
        }
        else
        { StartCoroutine(DelayFunction(0.5f, CheckSlotDot, TargetTileNum)); }
    }
    
    private void Heal(int TargetTileNum)
    {
        int healing = 0;
        if (TargetTileNum == player.OriginTile)
        {
            healing = 3;
        }
        else
        { healing = 1; }

        player.Life += healing;
        StartCoroutine(uIGamePlay.NoteTimerValue(0.1f, 2.5f, uIGamePlay.HPChanged, healing));
        EndDecision(3f, 2.5f);
    }

    private void CheckSlotDot(int TargetTileNum) 
    {
        slotPlay = tileGens[TargetTileNum].TilePrefab.GetComponent<SlotPlay>();

        int damage = 0;
        int emptyDot = 0;
        int life = player.Life;

        for (int i = slotPlay.DotDatas.Count-1; i >=0; i--)
        {
            if (slotPlay.DotDatas[i].Owner != DotOwn.None && slotPlay.DotDatas[i].Owner != player.dotOwn
                && life - damage > 0)
            {
                damage++;
                slotPlay.DotDatas[i].Owner = DotOwn.None;
                slotPlay.DotDatas[i].Dots.GetComponent<MeshRenderer>().material = SlotPlay.DefaultColor;
            }
            else if (slotPlay.DotDatas[i].Owner == DotOwn.None) { emptyDot++; }
            
        }

        SettingData.fixDotMode = (emptyDot == slotPlay.DotDatas.Count);

        if (damage > 0)
        { DamagePlayer(damage); }
        else
        {
            if (GamePlayManage.currentPlayer.isAI)
            { 
               aIDecision.Decision(this);
            }
            else { StartCoroutine(DelayFunction(0.5f, DecisionPanel, 1)); }
        }
    }
    private void DamagePlayer(int damage)
    {
        SlotStep step = slotPlay.gameObject.GetComponentInChildren<SlotStep>();
        step.ChangeColor(step.DefColor);
      
        player.Life-=damage;
        if(player.Life <= 0)
        {
            StartCoroutine(uIGamePlay.GameOverTimer(0.1f, 2.5f, player.PlayerName));
        }
        else
        { 
            StartCoroutine(uIGamePlay.NoteTimerValue(0.1f, 2.5f, uIGamePlay.HPChanged, -1 * damage));
            EndDecision(0.1f, 2.5f);
        }
        
    }

    private IEnumerator DelayFunction(float timer, TileDelegate tempDelegate, int i)
    {
        yield return new WaitForSeconds(timer);
        tempDelegate(i);
    }

    private void DecisionPanel(int none)
    {
        uIDecision.GetPlayer(this);
        
    }

    public void EndDecision(float Starttimer,float endTimer)
    {
        StartCoroutine(uIGamePlay.NoteTimer(Starttimer, endTimer, uIGamePlay.EndTurn));
    }
    
}
