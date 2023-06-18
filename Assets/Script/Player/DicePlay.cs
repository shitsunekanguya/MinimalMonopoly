using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class DicePlay : MonoBehaviour
{
    private float Speed = 2;
    [SerializeField] Vector3 DiceVelocity;

    public Rigidbody rb;

    private bool isRole;

    public string Side;
    public int result = 0;
    private static GameObject Cam;
    public GamePlayManage gamePlayManage;

    private Collider m_Collider;
    private void Awake()
    {
        gamePlayManage = FindObjectOfType<GamePlayManage>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePosition;
        DiceVelocity = rb.velocity;
        Cam = GameObject.Find("Main Camera");
        isRole = false;
        TryGetComponent<Collider>(out Collider collider);
        m_Collider = collider ? collider : null; if (m_Collider != null) { m_Collider.enabled = false; }
        
    }

    // Update is called once per frame
    void Update()
    {       
        if (SettingData.gameState== State.RollingDice&&!isRole)
        {           
            if (Input.GetMouseButton(0))
            {
                RotateDice(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));               
            }
            if (Input.GetMouseButtonUp(0))
            {
                ReleaseDice();
                gamePlayManage.CheckDice();
            }           
        }
    }

    public void RotateDice(float inputX,float inputY)
    {
        var value = GetRotateValue(inputX, inputY);
        transform.Rotate(value[0]*Speed, value[1] * Speed, value[2] * Speed);
        rb.AddTorque(value[0], value[1], value[2]);
    }

    public float[] GetRotateValue(float rangeX,float rangeY)
    {
        var x = rangeX < 0 ? -5 : rangeX > 0 ? 5 : 0;
        var y = rangeY < 0 ? -5 : rangeY > 0 ? 5 : 0;
        var z = 5;
        var tempValues = new float[] { x,y,z};

        return tempValues;
    }

    public void ReleaseDice()
    {
        if (m_Collider != null) { m_Collider.enabled = true; }
        rb.AddForce(transform.up * Speed * 8);
        rb.constraints = RigidbodyConstraints.None;
        StartCoroutine(CheckDice());
        isRole = true;
    }

    IEnumerator CheckDice()
    { 
        yield return new WaitForSeconds(8);
        { DetectDice(GetDiceSide()); }
    }

    private List<GameObject> GetDiceSide()
    {
        var DiceSide = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.tag == "DiceFace")
            {
                DiceSide.Add(child.gameObject);
            }
        }
        return DiceSide;
    }

    public void DetectDice(List<GameObject> DiceSide)
    {
        if (DiceSide.Count == 6 || DiceSide.Count == 12)
        { GetNumber(Side); }
        else
        {
            var AllDist = new List<float>();
            var tempObj = DiceSide;

            float LowestDist = Mathf.Infinity;
            int lowestIndex = 12;
            for (int i = 0; i < DiceSide.Count; i++)
            {
                float dist = Vector3.Distance(Cam.transform.position, DiceSide[i].transform.position);
                if (dist < LowestDist)
                {
                    LowestDist = dist;
                    lowestIndex = i;
                }
            }
            GetNumber(DiceSide[lowestIndex].name);
        }

        gamePlayManage.GetDiceResult();

    }


    private void GetNumber(string SideName)
    {
        string[] Splited = SideName.Split(":");
        foreach (string str in Splited)
        {
            int.TryParse(str, out result);
            if (result != 0) { break; }
        }
    }
}
