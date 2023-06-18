using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotData
{
    public GameObject Dots;
    public DotOwn Owner;
}
public class SlotPlay : MonoBehaviour
{
    public List<GameObject> PointDots;
    public List<DotData> DotDatas = new();
    public static Material DefaultColor;

    void Start()
    {
        DefaultColor = DefaultColor ? DefaultColor : GetComponent<MeshRenderer>().material;
        for (int i = 0; i < PointDots.Count; i++)
        {
            if (i < SettingData.DotNum)
            {
                PointDots[i].SetActive(true);
                DotData dotData = new DotData();
                dotData.Dots = PointDots[i];
                dotData.Owner = DotOwn.None;
                DotDatas.Add(dotData);
            }
            else {PointDots[i].SetActive(false); }
            

        }
    }

    public void SetMaterial()
    { 
        
    }

}
