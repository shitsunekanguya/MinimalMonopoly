using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StandPoint
{
    public GameObject PointToStand;
    public bool taken;
}
public class Slot : MonoBehaviour
{
    public List<StandPoint> standPoints = new List<StandPoint>();
    public Vector3 CenterPose;

    void Start()
    {
        for (int i = 0; i < standPoints.Count; i++)
        { standPoints[i].taken = false; }
        var tempVec = this.transform.position;
        CenterPose = new Vector3(tempVec.x, 0, tempVec.z);
    }

    void Update()
    {
        
    }
    public void PointTaken(int order)
    {
        standPoints[order].taken = true;
    }
}
