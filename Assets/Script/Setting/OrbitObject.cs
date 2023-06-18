using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OrbitObject : MonoBehaviour
{
    private float spinVal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, 0.3f, 0);
    }
    public void Spin()
    {
        transform.DORotate(new Vector3(360.0f, 360.0f, 0.0f), 3.0f,RotateMode.FastBeyond360);
    }
}
