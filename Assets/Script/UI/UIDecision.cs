 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDecision : MonoBehaviour
{
    public List<Image> dot;
    public List<Image> Activedot;
    public GameObject DecisionUIObj, FixDotUI, NotFixUI;
    public TMP_Text DotSeized,LifeRemain,Description;
    public string DefaultDescrip,OutOfLife,OutOfDot;

    private PlayerDecision playerDecision;

    private int dotToSeize,tempLife;
    private List<Image> PanelDotAvailable=new();

    public void SetUI()
    {
        Activedot.Clear();
        for (int i = 0; i < dot.Count; i++)
        {
            bool isActive = i < SettingData.DotNum ? true : false;
            dot[i].enabled = isActive;
            if (isActive)
            { Activedot.Add(dot[i]); }
        }
        
    }

    public void GetPlayer(PlayerDecision player)
    { 
        playerDecision = player;

        FixDotUI.SetActive(SettingData.fixDotMode); NotFixUI.SetActive(!SettingData.fixDotMode);

        DecisionPanel();
    }
    private void DecisionPanel()
    {       
        dotToSeize = 0;
        tempLife = playerDecision.player.Life;
        PanelDotAvailable.Clear();

        Description.text = "<color=yellow>" + DefaultDescrip;
        DotSeized.text = dotToSeize.ToString();
        LifeRemain.text = tempLife.ToString();

        DecisionUIObj.SetActive(true);

        for (int i = 0; i < Activedot.Count; i++)
        {
            if (playerDecision.slotPlay.DotDatas[i].Owner == playerDecision.player.dotOwn)
            {
                dot[i].color = playerDecision.player.color;
            }
            else
            { PanelDotAvailable.Add(dot[i]); dot[i].color = Color.white; }
        }
    }

    //on button
    public void ChangeValue(int value)
    {
        if (dotToSeize < 1 && value < 0)
        { return; }
        else if (dotToSeize >= PanelDotAvailable.Count && value > 0)
        {
            Description.text = "<color=red>" + OutOfDot; return;
        }
        else if (tempLife <= 1 && value > 0)
        {
            Description.text = "<color=red>" + OutOfLife; return;
        }

        dotToSeize += value;
        tempLife -= value;
        Description.text = "<color=yellow>" + DefaultDescrip;

        int DotTaken = Activedot.Count - PanelDotAvailable.Count;
        for (int i = DotTaken; i < Activedot.Count; i++)
        {
            if (i < dotToSeize+ DotTaken)
            { dot[i].color = playerDecision.player.M_PlayerColor.color;}
            else
            { dot[i].color = Color.white;}


        }
        LifeRemain.text = tempLife.ToString();
        DotSeized.text=dotToSeize.ToString();
    }

    //on button
    public void Seize()
    {
        if (SettingData.fixDotMode)
        {
            if (playerDecision.player.Life > 1)
            {
                dotToSeize = 1;
                tempLife = playerDecision.player.Life - 1;

                SlotStep step = playerDecision.slotPlay.gameObject.GetComponentInChildren<SlotStep>();
                step.ChangeColor(playerDecision.player.M_PlayerColor.color);
            }
            else
            {
                Description.text = "<color=yellow>" + OutOfLife;
                return;
            }
        }
        int DotTaken = Activedot.Count - PanelDotAvailable.Count;
        for (int i = 0; i < playerDecision.slotPlay?.DotDatas.Count; i++)
        {
            if (i<dotToSeize + DotTaken)
            {
                playerDecision.slotPlay.DotDatas[i].Owner = playerDecision.player.dotOwn;
                playerDecision.slotPlay.DotDatas[i].Dots.GetComponent<MeshRenderer>().material = playerDecision.player.M_PlayerColor;
            }
        }
        PassSeize();
    }
    public void PassSeize()
    {        
        playerDecision.player.Life = tempLife;
        DecisionUIObj.SetActive(false);
        playerDecision.EndDecision(0.1f, 2.5f);
    }

}
