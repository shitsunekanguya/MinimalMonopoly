using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIDiceRolling : MonoBehaviour
{
    private List<GameObject> DiceInstance => TileCreat.DiceInstance;
    private List<DicePlay> DicePlays = new();

    private float inputX, inputY;
    private bool isRoll;
    private GamePlayManage gamePlayManage;

    // Start is called before the first frame update
    void Start()
    {
        gamePlayManage = FindObjectOfType<GamePlayManage>();
    }
    void OnEnable()
    {
        isRoll = true;
        DicePlays.Clear();
        foreach (GameObject dice in DiceInstance)
        {
            DicePlays.Add(dice.GetComponent<DicePlay>());
        }
        inputX = Random.Range(-1.0f, 1.0f);
        inputY = Random.Range(-1.0f, 1.0f);
        float timer = Random.Range(2, 4);

        StartCoroutine(RotateTime(timer));
    }

    // Update is called once per frame
    void Update()
    {
        if (SettingData.gameState == State.AIPlaying && isRoll)
        {
            foreach (DicePlay dicePlay in DicePlays)
            {
                dicePlay.RotateDice(inputX, inputY);
            }
        }
    }

    IEnumerator RotateTime(float timer)
    { 
        yield return new WaitForSeconds(timer);
        foreach (DicePlay dicePlay in DicePlays)
        {
            dicePlay.ReleaseDice();
        }
        gamePlayManage.CheckDice();
        isRoll = false;
        this.enabled = false;

    }
}
