using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Trigger : MonoBehaviour
{
    Button myButton;
    void Start()
    {
        myButton = GetComponent<Button>();
        if (myButton != null)
        {
            myButton.onClick.AddListener(OnButtonClick);
        }
    }
    void OnButtonClick()
    {
        if (myButton != null)
        {
            myButton.OnDeselect(null);
            //Debug.Log("按钮被点击！");
        }

    }
    public void ButtonVoice()
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.SE_Button);//手动SE音频替换，卡片声音
    }
    public void CardVoice()
    {
        AudioManager.instance.AudioPlay(AudioManager.instance.SE_Card);//手动SE音频替换，卡片声音
    }


    public void SetActiveFalse()
    {
        gameObject.SetActive(false);

    }
}
