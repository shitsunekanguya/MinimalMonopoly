using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class TileCamera : MonoBehaviour,INextTurn
{
    [SerializeField] private Vector3 defPos,maxPose,origin,targetZoom;
    [SerializeField] private Quaternion originRotate,playerCamRotate;
    [SerializeField] private int defEdge,maxEdge;

    [SerializeField] private bool isMoveToPlayer,isMoveBack;
    [SerializeField] private Toggle camLock;
    private float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        isMoveToPlayer = false; isMoveBack = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (isMoveBack)
        {
            OnMoveToOrigin();
        }
        if (isMoveToPlayer)
        {
            FollowPlayer();
        }
    }

    public void NextTurn()
    {
        MoveToOrigin();
    }

    public void SetCam(Vector3 center)
    {
        camLock.gameObject.SetActive(true);
        int tempPlayerNum = PlaySetting.edgeNum;

        float x = center.x;
        Vector3 newPose=new();

        //Get Stabilizer
        float stabilizeY = (maxPose.y - defPos.y) / (maxEdge - defEdge);
        float stabilizeZ = ( maxPose.z - defPos.z) / (maxEdge - defEdge);

        newPose.x = x;
        newPose.y = defPos.y + (stabilizeY * (tempPlayerNum - defEdge));
        newPose.z = defPos.z + (stabilizeZ * (tempPlayerNum - defEdge));

        transform.position = newPose;

        transform.rotation = GetDirection(TileSpawnPoint.Center,transform.position);

        origin = transform.position;
        originRotate = transform.rotation;
    }

    public void MoveToPlayer()
    {
        if (!camLock.isOn)
        {
            isMoveToPlayer = true;
            isMoveBack = false;
        }
    }

    private void FollowPlayer()
    {        
        var targatVector = GamePlayManage.currentPlayer.m_chess.transform.position;
        var target = new Vector3(targatVector.x, targatVector.y + 1.2f, targatVector.z - 2.3f);
        transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime * 0.5f);

        var rotateTo = Quaternion.Euler(20, transform.rotation.y, transform.rotation.z);
        transform.rotation = Quaternion.Lerp(transform.rotation,rotateTo,speed*Time.deltaTime);
    }

    public void MoveToOrigin()
    {
        if (!camLock.isOn)
        {
            isMoveToPlayer = false;
            isMoveBack = true;
        }
        
    }

    private void OnMoveToOrigin()
    {
        transform.position = Vector3.Lerp(transform.position, origin, speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, originRotate, speed * Time.deltaTime);

        if (Round(transform.position) == Round(origin))
        {
            isMoveBack = false;
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
