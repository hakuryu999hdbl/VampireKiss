using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SrtSubtitlePlayer : MonoBehaviour
{
    [Header("Bind")]
    public VideoPlayer videoPlayer;
    public Text subtitleText;

    private class Cue
    {
        public double start;
        public double end;
        public string text;
    }

    private List<Cue> cues = new List<Cue>();
    private int currentIndex = -1;

    void Awake()
    {
        if (subtitleText != null) subtitleText.text = "";
    }

    void Update()
    {
        if (videoPlayer == null) return;
        if (subtitleText == null) return;
        if (!videoPlayer.isPlaying) return;

        double t = videoPlayer.time;

        int idx = FindCueIndex(t);
        if (idx != currentIndex)
        {
            currentIndex = idx;

            if (idx >= 0 && idx < cues.Count)
            {
                subtitleText.text = cues[idx].text;
            }
            else
            {
                subtitleText.text = "";
            }
        }
    }

    // ✅ 给 VideoManager 调用：把 SubtitleBank 拿到的 TextAsset 传进来
    public void LoadFromTextAsset(TextAsset srtTextAsset)
    {
        cues.Clear();
        currentIndex = -1;

        if (subtitleText != null) subtitleText.text = "";

        if (srtTextAsset == null)
        {
            Debug.LogWarning("LoadFromTextAsset: subtitle TextAsset is NULL");
            return;
        }

        if (string.IsNullOrEmpty(srtTextAsset.text))
        {
            Debug.LogWarning("LoadFromTextAsset: subtitle text is empty: " + srtTextAsset.name);
            return;
        }

        cues = ParseSrt(srtTextAsset.text);
        Debug.Log("Subtitle loaded: " + srtTextAsset.name + " cues=" + cues.Count);
    }

    private List<Cue> ParseSrt(string srt)
    {
        List<Cue> list = new List<Cue>();

        srt = srt.Replace("\r\n", "\n").Replace("\r", "\n");

        string[] blocks = srt.Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

        for (int b = 0; b < blocks.Length; b++)
        {
            string block = blocks[b];
            string[] lines = block.Split('\n');

            if (lines.Length < 2) continue;

            // lines[0] 是序号，lines[1] 是时间轴
            string timeLine = lines[1].Trim();

            string[] timeParts = timeLine.Split(new string[] { " --> " }, StringSplitOptions.None);
            if (timeParts.Length != 2) continue;

            double start = ParseTime(timeParts[0].Trim());
            double end = ParseTime(timeParts[1].Trim());

            string text = "";
            for (int i = 2; i < lines.Length; i++)
            {
                if (i > 2) text += "\n";
                text += lines[i];
            }

            Cue cue = new Cue();
            cue.start = start;
            cue.end = end;
            cue.text = text;

            list.Add(cue);
        }

        return list;
    }

    private double ParseTime(string time)
    {
        // SRT: 00:00:01,234
        time = time.Replace(',', '.');

        TimeSpan ts;
        bool ok = TimeSpan.TryParseExact(time, @"hh\:mm\:ss\.fff", CultureInfo.InvariantCulture, out ts);

        if (!ok) return 0;

        return ts.TotalSeconds;
    }

    private int FindCueIndex(double t)
    {
        if (cues == null || cues.Count == 0) return -1;

        for (int i = 0; i < cues.Count; i++)
        {
            if (t >= cues[i].start && t <= cues[i].end)
            {
                return i;
            }
        }

        return -1;
    }
}