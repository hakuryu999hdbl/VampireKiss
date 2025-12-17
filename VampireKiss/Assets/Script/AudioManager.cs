using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    AudioSource audioS;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        audioS = GetComponent<AudioSource>();
    }

    public void AudioPlay(AudioClip clip)
    {
        if (audioS == null)
        {
            Debug.LogWarning("AudioSource is missing or destroyed.");
            return;  // 避免继续尝试播放已经销毁的音源
        }


        audioS.PlayOneShot(clip);
    }

    public void Stop()
    {
        audioS.Stop();
    }



    /// <summary>
    /// 声音
    /// </summary>
    #region

    [Header("效果音")]
    public AudioClip SE_Button;
    public AudioClip SE_Card;


    #endregion

}
