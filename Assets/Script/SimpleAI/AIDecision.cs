using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDecision : MonoBehaviour
{
    public delegate void DelegateAi(int i, int j,int k,int l);

    private SlotPlay slotPlay;
    private PlayerDecision playerDecision;

    public void Decision(PlayerDecision PlayerDsc)
    {
        playerDecision = PlayerDsc;
        slotPlay = playerDecision.slotPlay;
        int lifeRemain = GamePlayManage.currentPlayer.Life;

        int slotDots = SettingData.DotNum;
        int takenDot = GetTakenDots();
        int availDots = slotDots - takenDot;
        int dotToSeize = 0;
        int seizable = GetSeizable(lifeRemain,availDots);

        if (lifeRemain > 1)
        {
            if (!SettingData.fixDotMode)
            {
                if (lifeRemain <= 4)
                { dotToSeize = Random.Range(0, seizable - 1); }
                else if (lifeRemain > 4 && lifeRemain <= 8)
                { dotToSeize = Random.Range(0, seizable); }
                else { dotToSeize = Random.Range(0, seizable + 1);  }
            }
            
            else
            {
                if (seizable > 0)
                { 
                    var stabilize = Random.Range(0, 20);
                    if (stabilize > 3) { dotToSeize = 1; }
                    else { dotToSeize = 0; }

                    if (dotToSeize == 1) 
                    {
                        SlotStep step = slotPlay.gameObject.GetComponentInChildren<SlotStep>();
                        step.ChangeColor(playerDecision.player.M_PlayerColor.color);
                    } 
                }                   
            }
        }

        //error prevent
        while (lifeRemain <= dotToSeize || availDots < dotToSeize)
        { dotToSeize--; }

        lifeRemain -= dotToSeize;

        Seize(slotDots, takenDot, dotToSeize,lifeRemain);
    }
    private int GetSeizable(int lifeRemain, int availDots)
    {       
        while (availDots > lifeRemain)
        { availDots--; }

        int seizable = availDots;
        return seizable;
    }

    private int GetTakenDots()
    {
        int takenDot = 0;
        DotOwn dotOwn = GamePlayManage.currentPlayer.dotOwn;

        for (int i = 0; i < slotPlay.DotDatas.Count; i++)
        {
            if (slotPlay.DotDatas[i].Owner != DotOwn.None)
            {
                takenDot++;
            }

        }
        return takenDot;
    }

    private void Seize(int slotDots,int takenDot,int dotToSeize,int lifeRemain)
    {
        
        for (int i = 0; i < slotDots; i++)
        {
            if (i < dotToSeize + takenDot)
            {
                slotPlay.DotDatas[i].Owner = playerDecision.player.dotOwn;
                slotPlay.DotDatas[i].Dots.GetComponent<MeshRenderer>().material = playerDecision.player.M_PlayerColor;
            }
        }
        playerDecision.player.Life = lifeRemain;
        playerDecision.EndDecision(0.1f, 2.5f);
    }

}
