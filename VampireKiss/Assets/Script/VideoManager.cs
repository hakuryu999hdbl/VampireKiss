using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoManager : MonoBehaviour
{
    [Header("Video")]
    public VideoPlayer player;
    public VideoClip[] clips;   // 直接在 Inspector 拖入

    void Awake()
    {
        if (player == null) player = GetComponent<VideoPlayer>();

        player.playOnAwake = false;
        player.isLooping = false;
        player.waitForFirstFrame = true;

        player.loopPointReached += OnFinished;
        player.errorReceived += OnError;
    }

    void Start()
    {
        // VideoScene 一进来就播（你从上一场景传了 nextVideoId）
        int index = GetIndexFromFlow();
        PlayByIndex(index);



    }

    int GetIndexFromFlow()
    {
        // 你现在存的是 "1"~"8"
        if (int.TryParse(GameFlowData.nextVideoId, out int n))
            return Mathf.Clamp(n - 1, 0, clips.Length - 1);

        return 0;
    }

    public void PlayByIndex(int index)
    {
        if (clips == null || clips.Length == 0)
        {
            Debug.LogError("No clips assigned!");
            return;
        }

        if (index < 0 || index >= clips.Length || clips[index] == null)
        {
            Debug.LogError($"Clip missing at index {index}");
            return;
        }

        player.source = VideoSource.VideoClip;
        player.clip = clips[index];
        player.Play();

        Debug.Log("Playing clip: " + clips[index].name);
    }

    void OnFinished(VideoPlayer vp)
    {
        Debug.Log("Video finished.");
        // TODO：播完回主菜单/分支选择
        // SceneManager.LoadScene("MenuScene");
    }

    void OnError(VideoPlayer vp, string msg)
    {
        Debug.LogError("Video error: " + msg);
    }





    /// <summary>
    /// 跳转场景
    /// </summary>
    #region
    [Header("加载场景")]
    public GameObject LoadingImage;




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


    #endregion
}
