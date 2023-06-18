using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class SettingData
{
    public static int EdgeCount => PlaySetting.edgeNum+2;
    public static State gameState = State.MapCreating;
    public static GameObject dice => PlaySetting.dice;
    public static int Life => PlaySetting.life;
    public static int PlayerNum => PlaySetting.playerNum;
    public static int DotNum => PlaySetting.dotNum;
    public static int DiceNum => PlaySetting.diceNum;
    public static int PlayerToStart => PlaySetting.playerToStart;

    public static bool fixDotMode;

    public static List<bool> PlayerAi=new();
    public static List<string> PlayerName=new();
    public static List<GameObject> Avatar = new();
}

