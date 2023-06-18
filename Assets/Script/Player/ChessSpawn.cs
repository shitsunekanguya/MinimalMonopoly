using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessSpawn : MonoBehaviour
{
    float fallSpan=2;
    int PlayerOrder;
    private GameObject Chess;
    // Start is called before the first frame update
    void Awake()
    {
        
        PlayerOrder = gameObject.GetComponent<Player>().PlayerOrder;
        Chess = transform.GetChild(0).gameObject;
        Chess.SetActive(false);
    }
    private void Start()
    {
        SpawnChess();
    }
    public void SpawnChess()
    {
        Chess.SetActive(true);
        float degree = PlayerOrder == 1 || PlayerOrder == 4 ? 0 : 180;

        Chess.transform.position = this.transform.position + new Vector3(0,fallSpan,0);
        Chess.transform.Rotate(0,degree,0);
        Chess.SetActive(true);
    }
}
