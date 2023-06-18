using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.FilePathAttribute;

public class TileGen
{
    public Vector3 TilePose;
    public GameObject TilePrefab;
    public int SquareSide;
    public bool isBase;
}

public class TileSpawnPoint : MonoBehaviour
{
    [Header("Tile Edge Settings")]
    [SerializeField] private float EdgeWidth;
    [SerializeField] private int EdgeCount => SettingData.EdgeCount;

    public static List<TileGen> tileGens;
    public static Vector3 Center;
    [SerializeField] float xRow = 0, zRow = 0;
    [SerializeField] TileCamera tileCamera;



    [Header("Tile Object")]
    //++ Tile Size Setting **
    [SerializeField] private GameObject RedSlot, GreenSlot, YellowSlot, BlueSlot;

    public void PointSet()
    {
        tileGens = new();
        xRow = 0;
        zRow = 0;       
        tileGens = new List<TileGen>();

        var tempPos = new Vector3();
        var tempTargetPos = new Vector3();

        int tileOrder=0;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < EdgeCount - 1; j++)
            {
                var tileData = new TileGen();
                var tempPointPos = new Vector3(xRow * EdgeWidth, 0, zRow * EdgeWidth);
                
                tileData.TilePose = tempPointPos;

                var avatar = SettingData.Avatar;

                GameObject tempObject = null;

                if (SettingData.PlayerNum < 3)
                {
                    if (xRow == 0 && zRow == 0) { tempObject =SettingData.Avatar[0]; }
                    else if (xRow == EdgeCount - 1 && zRow == EdgeCount - 1) { tempObject = SettingData.Avatar[1]; }
                }
                else if (SettingData.PlayerNum >= 3)
                {
                    if (xRow == 0 && zRow == 0) { tempObject = SettingData.Avatar[0]; }
                    else if (xRow == 0 && zRow == EdgeCount - 1) { tempObject = SettingData.Avatar[1]; }
                    else if (xRow == EdgeCount - 1 && zRow == EdgeCount - 1) { tempObject = SettingData.Avatar[2]; }

                    if (SettingData.PlayerNum > 3)
                    { if (xRow == EdgeCount - 1 && zRow == 0) { tempObject= SettingData.Avatar[3]; } }
                }

                if (tempObject != null)
                {
                    tileData.TilePrefab = (GameObject)Instantiate(tempObject, tempPointPos, Quaternion.identity);
                    Player tempPlayer = tileData.TilePrefab.GetComponent<Player>();
                    tempPlayer.OriginTile = tileOrder;
                    tempPlayer.CurrentTile = tileOrder;
                    GamePlayManage.m_Players.Add(tempPlayer);
                }
                tileData.isBase = (tempObject != null);

                int tempSide = 0;

                if (xRow == 0 && zRow == 0)
                { tempPos = tempPointPos; }
                else if (xRow == EdgeCount - 1 && zRow == EdgeCount - 1)
                { tempTargetPos = tempPointPos; }

                switch (i) 
                {
                        case 0: zRow++;  tempSide = 1; break;
                        case 1: xRow++; tempSide = 2; break;
                        case 2: zRow--;  tempSide = 3; break;
                        case 3: xRow--; tempSide =4; break;
                }

                tileData.SquareSide = tempSide;
                tileGens.Add(tileData);
                tileOrder++;
            }
        }

        Center = (tempPos+tempTargetPos)/2;
        tileCamera.SetCam(Center);
        StartGenerate();       

    }

    void StartGenerate()
    {
        TileCreat tileCreat = FindObjectOfType<TileCreat>();
        StartCoroutine(tileCreat?.CreateTile());
        GamePlayManage gamePlayManage = FindObjectOfType<GamePlayManage>();
        gamePlayManage?.GetPlayer();
    }
}
