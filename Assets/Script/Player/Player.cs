using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Setting Status")]
    public int PlayerOrder;
    public string PlayerName;
    public bool isAI;    
    public int Life;

    [Header("Player Writable Status")]
    public GameObject m_chess;    
    public DotOwn dotOwn;
    public bool isWalk = false;
    public Material M_PlayerColor;
    public Color color;

    private readonly float speed = 1.5f;
    private Chess chess;

    //Tile
    List<TileGen> TileGens => TileSpawnPoint.tileGens;
    private int TargetTile, NextTile,Step;
    public int OriginTile, CurrentTile;
    private Vector3 NextTilePos;

    private PlayerDecision playerDecision;


    // Start is called before the first frame update
    void Start()
    {
        playerDecision = FindObjectOfType<PlayerDecision>();
        
        PlayerName = PlayerName==null||PlayerName=="" ? "Player" + PlayerOrder:PlayerName;

        Life = SettingData.Life;
        chess = m_chess.GetComponent<Chess>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalk)
        {
            Move();
        }
    }

    public void StartWalk()
    {
        Step = GamePlayManage.DiceResult;
        TargetTile = CurrentTile + Step;
        TargetTile = GetTileRound(TargetTile);
        NextTile = CurrentTile + 1;
        NextTile = GetTileRound(NextTile);
        
        isWalk = true;
        chess.StartWalk(isWalk);
    }
    void Move()
    {       
        
        if (chess.isDodge)
        {
            var CenterPos = new Vector3(TileGens[CurrentTile].TilePose.x, m_chess.transform.position.y, TileGens[CurrentTile].TilePose.z);
            m_chess.transform.position = Vector3.MoveTowards(m_chess.transform.position, CenterPos, speed * Time.deltaTime);
            m_chess.transform.rotation = Quaternion.Lerp(m_chess.transform.rotation, GetDirection(CenterPos, m_chess.transform.position), speed * 10 * Time.deltaTime);

            if (Round(m_chess.transform.position) == Round(CenterPos))
            {
                chess.isDodge = false;
            }
        }
        else
        {
            if (Step > 0)
            {
                NextTilePos = new Vector3(TileGens[NextTile].TilePose.x, m_chess.transform.position.y, TileGens[NextTile].TilePose.z);
                //
                m_chess.transform.position = Vector3.MoveTowards(m_chess.transform.position, NextTilePos, speed * Time.deltaTime);
                m_chess.transform.rotation = Quaternion.Lerp(m_chess.transform.rotation, GetDegree(CurrentTile), speed * 10 * Time.deltaTime);

                if (Round(m_chess.transform.position) == Round(NextTilePos))
                {
                    Step--;
                    CurrentTile = NextTile;
                    NextTile++;
                    NextTile = GetTileRound(NextTile);

                }
            }
            else
            {
                isWalk = false;
                TileGens[CurrentTile].TilePrefab.GetComponentInChildren<SlotStep>().MoveChess(m_chess);
                m_chess.GetComponent<Chess>().StartWalk(isWalk);
                SettingData.gameState = State.Thinking;

                playerDecision.WalkComplete(CurrentTile);

            }
        }
    }

    private Vector3 Round(Vector3 vectorToRound)
    {
       var x = Mathf.Round(vectorToRound.x * 10.0f) * 0.1f;
       var y = Mathf.Round(vectorToRound.y * 10.0f) * 0.1f;
       var z = Mathf.Round(vectorToRound.z * 10.0f) * 0.1f;
       var Rounded = new Vector3(x, y, z);
        return Rounded;
    }
    private int GetTileRound(int Tile)
    {
        if (Tile >= TileGens.Count)
        { Tile = Tile - TileGens.Count; }
        return Tile;
    }

    private Quaternion GetDegree(int TileToTurn)
    { 
        float degree = 0;
        int SquareSide = TileGens[TileToTurn].SquareSide;
        switch (SquareSide)
        {
            case 1: degree = 0; break;
            case 2: degree = 90; break;
            case 3: degree = 180; break;
            case 4: degree = 270; break;
        }
        Quaternion toTurn = Quaternion.Euler(0, degree, 0);
        return toTurn;
    }

    private Quaternion GetDirection(Vector3 target,Vector3 origin)
    { 
        var dir = (target-origin).normalized;
        var quaternion = Quaternion.LookRotation(dir);
        return quaternion;
    }

}
