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

    public SubtitleBank subtitleBank;
    public SrtSubtitlePlayer subtitlePlayer;

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


        if (pauseOnStart)
        {
            PauseVideo();
        }
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

        string sceneId = ExtractSceneId(clips[index].name);
        int language = PlayerPrefs.GetInt("language", 0);

        TextAsset srt = subtitleBank.GetSubtitle(sceneId, language);
        subtitlePlayer.LoadFromTextAsset(srt);

        player.Play();

        Debug.Log("Playing clip: " + clips[index].name);
    }

    private string ExtractSceneId(string clipName)
    {
        // clip: SC_15_Nude_batch / SC_15_Mosaic_batch
        if (string.IsNullOrEmpty(clipName)) return "SC_12";

        string[] parts = clipName.Split('_');

        if (parts.Length >= 2)
        {
            return parts[0] + "_" + parts[1]; // SC_15
        }

        return "SC_12";
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
    /// 暂停继续快进倒退
    /// </summary>
    #region
    [Header("Seek Settings")]
    public double seekStepSeconds = 5.0;   // 每次快进/倒退多少秒（你可改 3/5/10）
    public bool pauseOnStart = false;      // 如果想进场先暂停再让玩家按继续
    public void PauseVideo()
    {
        if (player == null) return;

        // VideoPlayer 的 Pause 比 Stop 更适合“暂停继续”
        if (player.isPlaying)
        {
            player.Pause();
            Debug.Log("Video paused");
        }
    }

    public void ResumeVideo()
    {
        if (player == null) return;

        // 若还没准备好，先 Prepare（有些平台/大视频必须）
        if (!player.isPrepared)
        {
            Debug.Log("Video not prepared yet, preparing...");
            player.Prepare();
            return;
        }

        // isPlaying 为 false 时 Play() 会从当前 time 继续
        player.Play();
        Debug.Log("Video resumed");
    }

    public void FastForward()
    {
        SeekBy(+seekStepSeconds);
    }

    public void Rewind()
    {
        SeekBy(-seekStepSeconds);
    }
    private void SeekBy(double deltaSeconds)
    {
        if (player == null) return;

        // 还没准备好时，先 Prepare（尤其移动端）
        if (!player.isPrepared)
        {
            Debug.LogWarning("Seek ignored: Video not prepared yet.");
            return;
        }

        double length = player.length; // 秒
        if (length <= 0)
        {
            Debug.LogWarning("Seek ignored: Video length is 0 (not ready?)");
            return;
        }

        double target = player.time + deltaSeconds;

        if (target < 0) target = 0;

        // 注意：跳到 >= length 可能直接触发 loopPointReached 或黑屏
        // 所以给一点余量
        double safeEnd = System.Math.Max(0, length - 0.05);
        if (target > safeEnd) target = safeEnd;

        bool wasPlaying = player.isPlaying;

        // 关键：先暂停再设 time，某些平台更稳
        player.Pause();
        player.time = target;

        // 强制刷新一帧（可选，但对某些机器更稳）
        player.StepForward();

        if (wasPlaying)
        {
            player.Play();
        }

        Debug.Log("Seek to: " + target.ToString("F2") + " / " + length.ToString("F2"));
    }


    void Update()
    {
        // 先用键盘测试：空格暂停/继续，→ 快进，← 倒退
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (player != null && player.isPlaying) PauseVideo();
            else ResumeVideo();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            FastForward();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Rewind();
        }
    }
    #endregion




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
