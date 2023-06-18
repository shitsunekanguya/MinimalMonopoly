using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class DiceDetectFloor : MonoBehaviour
{
    [SerializeField] private DicePlay dicePlay;

    private void Start()
    {
        dicePlay = transform.parent.GetComponent<DicePlay>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Floor")
        { dicePlay.Side = gameObject.name;}
    }
}
