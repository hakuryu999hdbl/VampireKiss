using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class language : MonoBehaviour
{

    Image image;
    public Sprite J, C_1,E;
    public void OnEnable()
    {
        image = GetComponent<Image>();

      
        // 根据保存的语言偏好设置文本
        switch (PlayerPrefs.GetInt("language"))
        {
            case 0:
                image.sprite = J;//日语
                break;
            case 1:
                image.sprite = C_1;//简中
                break;
            case 2:
                image.sprite = E;//英
                break;
        }

    

    }
}
