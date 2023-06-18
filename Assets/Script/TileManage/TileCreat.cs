using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class TileCreat : MonoBehaviour, INextTurn
{
    private int EdgeCount => SettingData.EdgeCount;
    private Vector3 Center => TileSpawnPoint.Center;
    private GameObject dice => SettingData.dice;
    List<TileGen> TileGens => TileSpawnPoint.tileGens;
    List<GameObject> Tiles = new();

    public static List<GameObject> DiceInstance=new();

    [Header("Tile Object")]
    //++ Tile Size Setting **
    [SerializeField] private GameObject Slot, Ground;

    public IEnumerator CreateTile()
    {
        DiceInstance = new();

        yield return new WaitForSeconds(0.3f);
        foreach (var tileDatas in TileGens)
        {
            float SqureSide = tileDatas.SquareSide;
            float RotatateAngle = SqureSide==1? 90: SqureSide == 2 ? 180: SqureSide == 3 ? 270: 0;

            if(tileDatas.TilePrefab==null)
            tileDatas.TilePrefab = (GameObject)Instantiate(Slot, tileDatas.TilePose, Quaternion.Euler(0, RotatateAngle, 0));

            Tiles.Add(tileDatas.TilePrefab);
            yield return new WaitForSeconds(0.1f);
        }
        CreateFloor();
        
        yield return new WaitForSeconds(0.5f);

        SettingData.gameState = State.MapCreating;
        GamePlayManage gamePlayManage = FindObjectOfType<GamePlayManage>();
        gamePlayManage.AllNextTurn();
        yield return null;
    }

    private void CreateFloor()
    {
        var FloorPos = Center - new Vector3(0, 0.3f, 0);
        GameObject Floor = (GameObject)Instantiate(Ground, FloorPos, Quaternion.identity);
        Floor.transform.localScale = Center*2+new Vector3(0,0.1f,0);
    }

    public void NextTurn()
    {
        float range = (EdgeCount - 2.5f )/ 3;
        int x = 1;
        int z = 0;

        for(int i = 0; i < SettingData.DiceNum; i++)
        {
            
            if (i <= 2) {z = 0; }
            else if (i > 2 && i<=5 ) {z = 1; }
            else{ z = -1; }

            if (i % 3 == 0)
            { x = 0; }
            else if (i % 2 == 0)
            {
                x = -1;
            }
            else { x = 1; }

            var diceInstanted = (GameObject)Instantiate(dice, Center + new Vector3(range*x, 2f, range*z), Quaternion.Euler(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180)));

            float stabilize = SettingData.DiceNum > 4 ? (5 * 0.3f) : (3 * 0.3f);
            diceInstanted.transform.localScale = (Vector3.one / 30) * (EdgeCount + 2) / stabilize;
            DiceInstance.Add(diceInstanted);
        }

    }

}
