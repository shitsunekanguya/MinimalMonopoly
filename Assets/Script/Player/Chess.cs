using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class Chess : MonoBehaviour
{
    public Player player;
    public Animator animator;
    public bool isDodge=false;

    public bool isMove=false;
    public Vector3 targetPos;
    private float speed = 10f;
    public StandPoint currentStandPoint;

    private void Awake()
    {
        isMove = false;
        isDodge = false;
    }
    void Start()
    {
        player = transform.parent.GetComponent<Player>();
        animator = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        { MoveChess(); }
        
    }

    //chess dodging
    public void StartWalk(bool isWalk)
    { 
        animator.SetBool("isWalk", isWalk);
    }
    private void MoveChess()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, GetDirection(targetPos, transform.position), speed * 10 * Time.deltaTime);

        if (Round(transform.position) == Round(targetPos))
        {
            isMove = false;
            StartWalk(false);
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
   
    private Quaternion GetDirection(Vector3 target, Vector3 origin)
    {
        var dir = (target - origin).normalized;
        var quaternion = Quaternion.LookRotation(dir);
        return quaternion;
    }

}
