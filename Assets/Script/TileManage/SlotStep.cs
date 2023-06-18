using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlotStep : MonoBehaviour,IGameOver
{
    public Color DefColor;
    public Color OwnedColor;
    public Color StepedColor;

    List<Collider> PlayerStep = new();
    List<GameObject> playerChesses = new();

    [SerializeField] private Slot slot;


    private void Start()
    {
        slot = GetComponentInParent<Slot>();
        OwnedColor = DefColor;
        StepedColor = new Color(OwnedColor.r - 0.1f, OwnedColor.g - 0.1f, OwnedColor.b - 0.1f, OwnedColor.a);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!PlayerStep.Contains(other))
            { 
                PlayerStep.Add(other);
                playerChesses.Add(other.gameObject);
            }    
            CheckStep();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (PlayerStep.Contains(other))
            { 
                PlayerStep.Remove(other);
                if (playerChesses.Contains(other.gameObject))
                { playerChesses.Remove(other.gameObject); }
            }
            CheckStep();
        }
    }

    private void CheckStep()
    {

        if (PlayerStep == null || PlayerStep.Count == 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = OwnedColor;
        }
        else
        { gameObject.GetComponent<SpriteRenderer>().color = StepedColor; }
        
        for (int i = 0; i < playerChesses.Count; i++)
        {
            Chess chess=playerChesses[i].GetComponent<Chess>();
            if (!chess.player.isWalk)
            {
                MoveChess(playerChesses[i]);

                if (playerChesses.Count == 1 && chess.isDodge)
                {
                    chess.isDodge = false;
                    var target = new Vector3(slot.CenterPose.x, playerChesses[i].transform.position.y, slot.CenterPose.z);
                    chess.targetPos = target; chess.currentStandPoint.taken = false;
                    chess.StartWalk(true);
                    chess.isMove = true;

                }
            }
            
           
        }
    }

    public void MoveChess(GameObject playerChess)
    {
        Chess chess = playerChess.GetComponent<Chess>();
        if (playerChesses.Count > 1||(!playerChesses.Contains(playerChess)&& playerChesses.Count > 0))
        {
            if (!chess.isDodge)
            {
                foreach (StandPoint s in slot.standPoints)
                {
                    if (!s.taken)
                    {
                        var sPos = s.PointToStand.transform.position;
                        var target = new Vector3(sPos.x, playerChess.transform.position.y, sPos.z);
                        chess.targetPos = target; chess.isMove = true; chess.isDodge = true; chess.currentStandPoint = s;
                        chess.StartWalk(true);
                        s.taken = true;
                        break;
                    }
                }
            }

        }            
    }

    public void GameOverOn(GameObject chessOver)
    {
        Collider chessColl = chessOver.GetComponent<Collider>();

        if (PlayerStep.Contains(chessColl))
        {
            PlayerStep.Remove(chessColl);
            if (playerChesses.Contains(chessOver))
            { playerChesses.Remove(chessOver); }
            Destroy(chessOver);
            CheckStep();
        }
        
    }

    public void ChangeColor(Color color)
    {
        SpriteRenderer colorSquare = gameObject.GetComponent<SpriteRenderer>();

        OwnedColor = color;
        StepedColor = new Color(OwnedColor.r - 0.1f, OwnedColor.g - 0.1f, OwnedColor.b - 0.1f, OwnedColor.a);

        colorSquare.color = StepedColor;
    }
}
