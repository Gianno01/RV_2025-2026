using UnityEngine;

[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/QuestData")]
public class QuestData : ScriptableObject
{
    public string QuestName;
    public AudioClip hintAudio;
    public float SecToWait;
}