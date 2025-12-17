using UnityEngine;

public class SubtitleBank : MonoBehaviour
{
    [System.Serializable]
    public class Entry
    {
        public string sceneId;     // SC_12
        public TextAsset jp;
        public TextAsset cn;
        public TextAsset en;
    }

    public Entry[] entries;

    public TextAsset GetSubtitle(string sceneId, int language)
    {
        if (entries == null) return null;

        for (int i = 0; i < entries.Length; i++)
        {
            if (entries[i] == null) continue;
            if (entries[i].sceneId != sceneId) continue;

            // 0=JP 1=CN 2=EN
            if (language == 0) return entries[i].jp;
            if (language == 1) return entries[i].cn;
            if (language == 2) return entries[i].en;

            return entries[i].jp;
        }

        return null;
    }
}