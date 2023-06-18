using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PColorButton : MonoBehaviour
{
    public int ButtonGroup;
    public Color ButtonColor;
    public DotOwn ColorOwn;
    public bool isChose;
    public Image ColorImage;

    private PlaySetting playSetting;

    private void Start()
    {
        ColorImage=transform.parent.GetComponent<Image>();
        Button thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(TaskOnClick);

        playSetting = FindObjectOfType<PlaySetting>();

    }
    void TaskOnClick()
    {
        playSetting.ColorChose(isChose,ButtonGroup,ButtonColor,ColorOwn);
    }
}
