using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System.Diagnostics;

public class PlayerInfo
{ 
    public string playerName;
    public GameObject playerAvatar;
    public bool isAI;
}

public class PlaySetting : MonoBehaviour
{
    public static int playerNum,life,diceNum,edgeNum,playerToStart,dotNum;
    public Slider playerBar,lifeBar,diceNumBar,edgeNumBar,dotNumBar;
    public List<Button> diceButtons, ColorButton = new();
    public GameObject defaultDice,UICanvas;
    public List<DotOwn> dotStore;

    public static GameObject dice;    

    [SerializeField] private TMP_Text playerNumText,lifeText,diceNumText,edgeNumText,dotNumText;
    [SerializeField] private GameObject[] PlayerSlot = new GameObject[4];
    [SerializeField] private List<Toggle> isAIToggle;
    [SerializeField] private List<TMP_InputField> PlayerName;

    private TileSpawnPoint tileSpawnPoint;

    void Start()
    {
        playerNum = 4;life = 9;diceNum = 1;edgeNum = 4;dotNum = 3;
        playerToStart = 1;
        dice = defaultDice;

        tileSpawnPoint = FindAnyObjectByType<TileSpawnPoint>();
    }

    public void SetPlayerNumber() 
    {
        playerNum = (int)playerBar.value;
        playerNumText.text = playerNum.ToString();

        for (int i = 0; i < PlayerSlot.Length; i++)
        { bool isActive = playerNum > i;
            PlayerSlot[i].SetActive(isActive);
        }
    }
    public void SetLifeNumber()
    {
        life = (int)lifeBar.value;
        lifeText.text = life.ToString();
    }
    public void SetDiceNumber()
    {
        diceNum = (int)diceNumBar.value;
        diceNumText.text = diceNum.ToString();
    }
    public void SetEdgeNumber()
    {      
        edgeNum = (int)edgeNumBar.value;
        edgeNumText.text = edgeNum.ToString();
    }
    public void SetDotNumber()
    {
        dotNum = (int)dotNumBar.value;
        dotNumText.text = dotNum.ToString();
    }

    public void ChooseDice(GameObject diceChosen)
    {
        dice = diceChosen;
    }
    public void RotateDice(GameObject diceRotate)
    {
        diceRotate.GetComponent<OrbitObject>().Spin();
    }
    public void ChangeButtonColor(Button button)
    {
        foreach (Button b in diceButtons)
        {
            Color bCol = b.GetComponent<Image>().color;
            if (b != button)
            { bCol = new Color(bCol.r, bCol.g, bCol.b, 0); }
            else
            { bCol = new Color(bCol.r, bCol.g, bCol.b, 0.25f); }
            b.GetComponent<Image>().color = bCol;
        }
    }

    public void ColorChose(bool isChose ,int buttonGroup, Color color,DotOwn colorOwn)
    {
        for (int i = 0; i < ColorButton.Count; i++)
        {
            float alpha = 0;
            PColorButton pColorButton = ColorButton[i].GetComponent<PColorButton>();
            if (isChose)
            {

            }
            else
            {
                Color tempCol = ColorButton[i].GetComponent<Image>().color;

                if (pColorButton.ButtonGroup == buttonGroup)
                {                   
                    if (pColorButton.ColorOwn == colorOwn)
                    {                       
                        pColorButton.isChose = true;
                        alpha = 0.45f;
                    }
                    else 
                    {
                        alpha = 0;                       
                        pColorButton.isChose = false;
                    }

                }
                else if (pColorButton.ColorOwn == colorOwn)
                {
                    if (pColorButton.ButtonGroup == buttonGroup)
                    {
                        alpha = 0.45f;
                        pColorButton.isChose = true;
                    }
                    else
                    {
                        alpha = 0;
                        pColorButton.isChose = false;
                    }
                }
                else { alpha = tempCol.a;}                
                ColorButton[i].GetComponent<Image>().color = new Color(tempCol.r, tempCol.g, tempCol.b, alpha);

            }
        }
    }

    public void StartGame()
    {
        SettingData.gameState = State.PlaySetting;
        DotOwn[] PColor = PlayerColor();

        for (int i = 0; i < playerNum; i++)
        { 
            PlayerInfo playerInfo = new();

            string playerName = PlayerName[i].text;
            SettingData.PlayerName.Add(playerName);
            SettingData.PlayerAi.Add(isAIToggle[i].isOn);
            SettingData.Avatar.Add(GetAvartar(PColor[i]));
        }
        UICanvas.SetActive(false);
        tileSpawnPoint.PointSet();
    }

    private DotOwn[] PlayerColor()
    {
        DotOwn[] playerColors = new DotOwn[playerNum];
        var tempDotStore = dotStore;

        for (int i = 0; i < playerColors.Length; i++)
        {
            playerColors[i] = DotOwn.None;
        }

        for (int i = 0; i < ColorButton.Count; i++)
        {
            PColorButton pColorButton = ColorButton[i].GetComponent<PColorButton>();
            if (pColorButton.ButtonGroup <= playerNum && pColorButton.isChose)
            {
                playerColors[pColorButton.ButtonGroup - 1] = pColorButton.ColorOwn;
                if (tempDotStore.Contains(pColorButton.ColorOwn))
                { tempDotStore.Remove(pColorButton.ColorOwn); }
            }
        }

        int availDotOrder = 0;
        for (int i = 0; i < playerColors.Length; i++)
        {
            if (playerColors[i] == DotOwn.None)
            { playerColors[i] = tempDotStore[availDotOrder]; availDotOrder++; }
        }
        return playerColors;
    }

    private GameObject GetAvartar(DotOwn dot)
    {         
        string path="";

        switch (dot)
        {
            case DotOwn.RedOwn: path = "Assets/Prefab/RedBase.prefab"; break;
                case DotOwn.GreenOwn:path = "Assets/Prefab/GreenBase.prefab"; break;
                case DotOwn.BlueOwn:path = "Assets/Prefab/BlueBase.prefab"; break;
                case DotOwn.YellowOwn:path = "Assets/Prefab/YellowBase.prefab"; break;
        }
        GameObject tempObj = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
        return tempObj;
    }
}
