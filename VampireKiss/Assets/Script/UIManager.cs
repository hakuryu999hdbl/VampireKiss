using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("目前储存的语言" + PlayerPrefs.GetInt("language"));//0日语 1简体中文

        if (PlayerPrefs.GetInt("language") == 0)
        {
            Language_J.SetActive(true);
        }
        else
        {
            Language_C1.SetActive(true);
        }





        // 1) 默认值（你也可以改成 -15f 之类更舒服）
        float defaultV = -15f;

        // 2) 读取（没有就用默认）
        float v = PlayerPrefs.HasKey(KEY_BGM) ? PlayerPrefs.GetFloat(KEY_BGM) : defaultV;

        // 3) 先把 Mixer 和 Slider 同步到同一个值
        audioMixer.SetFloat(MIXER_PARAM, v);
        slider_BGM.SetValueWithoutNotify(v);

        // 4) 再绑定监听，避免 Start 时自己触发一次
        slider_BGM.onValueChanged.AddListener(SetVolume);

        // 5) 若第一次没有 Key，顺手存一次
        if (!PlayerPrefs.HasKey(KEY_BGM))
        {
            PlayerPrefs.SetFloat(KEY_BGM, v);
            PlayerPrefs.Save();
        }

        Debug.Log("BGM Volume Init: " + v);
    }

    /// <summary>
    /// 声音控制
    /// </summary>
    #region
    [Header("声音利用")]
    public AudioMixer audioMixer;

    public Slider slider_BGM;

    private const string KEY_BGM = "BGM_Volume";
    private const string MIXER_PARAM = "MainVolume";


    public void SetVolume(float value)
    {
        audioMixer.SetFloat(MIXER_PARAM, value);
        PlayerPrefs.SetFloat(KEY_BGM, value);
        PlayerPrefs.Save(); // 移动端更保险
        // Debug.Log("SetVolume: " + value);
    }


    #endregion


    /// <summary>
    /// 设置界面
    /// </summary>
    #region
    [Header("打开界面")]
    public GameObject Button_All;
    public GameObject Setting_Menu;
    public void OpenSetting() 
    {
        Button_All.SetActive(false);
        Setting_Menu.SetActive(true);
    }
    public void CloseSetting()
    {
        Button_All.SetActive(true);
        Setting_Menu.SetActive(false);
    }

    public void SettingLanguage() 
    {
        if (PlayerPrefs.GetInt("language")==0) 
        {
            PlayerPrefs.SetInt("language", 1);
        }
        else
        {
            PlayerPrefs.SetInt("language", 0);
        }

        LoadingScene_MenuScene();
    }

    public GameObject Language_J, Language_C1;

    #endregion


    /// <summary>
    /// 打开界面
    /// </summary>
    #region
    [Header("打开界面")]
    public GameObject Choose_Menu;
    public void OpenChoose()
    {
        Choose_Menu.SetActive(true);
    }
    public void CloseChoose()
    {
        Choose_Menu.SetActive(false);
    }


    public void ChooseVideo(int Number) 
    {
        switch (Number) 
        {
            case 1:
                GameFlowData.nextVideoId = "1";
                break;
            case 2:
                GameFlowData.nextVideoId = "2";
                break;
            case 3:
                GameFlowData.nextVideoId = "3";
                break;
            case 4:
                GameFlowData.nextVideoId = "4";
                break;
            case 5:
                GameFlowData.nextVideoId = "5";
                break;
            case 6:
                GameFlowData.nextVideoId = "6";
                break;
            case 7:
                GameFlowData.nextVideoId = "7";
                break;
            case 8:
                GameFlowData.nextVideoId = "8";
                break;
        }

        Debug.Log("播放动画" + GameFlowData.nextVideoId);
        LoadingScene_VideoScene();
    }


    #endregion


    /// <summary>
    /// 外部链接
    /// </summary>
    #region
    [Header("外部链接")]
    public GameObject Choose_Staff;
    public void OpenStaff()
    {
        Choose_Staff.SetActive(true);
    }
    public void CloseStaff()
    {
        Choose_Staff.SetActive(false);
    }


    public void ChooseStaff(int Number)
    {
        switch (Number)
        {
            case 1:
                Application.OpenURL("https://x.com/nekoujistudio");
                break;
            case 2:
                Application.OpenURL("https://x.com/MadDog1995_");
                break;
            case 3:
                Application.OpenURL("https://x.com/Detective_ye");
                break;
          
        }
    }


    #endregion


    /// <summary>
    /// 跳转场景
    /// </summary>
    #region
    [Header("加载场景")]
    public GameObject LoadingImage;

    public void LoadingScene_VideoScene()
    {
        Time.timeScale = 1f;
        LoadingImage.SetActive(true);

        Invoke("VideoScene", 1f);
    }
    void VideoScene()
    {
        SceneManager.LoadScene("VideoScene");
    }



    public void LoadingScene_MenuScene()
    {
        Time.timeScale = 1f;
        LoadingImage.SetActive(true);

        Invoke("MenuScene", 1f);
    }
    void MenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");

        Application.Quit();
    }



    #endregion
}
