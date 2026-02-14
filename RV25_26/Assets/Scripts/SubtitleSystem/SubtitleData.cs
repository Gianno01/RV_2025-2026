using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SubtitleChunk
{
    [TextArea(5, 10)]
    public string text;
    public float startTime;
    public float endTime;
}

[CreateAssetMenu(fileName = "SubtitleData", menuName = "Scriptable Objects/SubtitleData")]
public class SubtitleData : ScriptableObject
{
    public List<SubtitleChunk> clipSubtitleChunks;
}