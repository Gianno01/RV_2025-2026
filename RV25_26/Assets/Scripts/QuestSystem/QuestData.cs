using UnityEngine;

/// <summary>
/// Una quest ha un nome, un hint come aiuto per completarla e dei secondi da attendere prima che venga proposto l'hint.
/// Se SecToWait = -1, allora l'hint sarà effetto di altre cause.
/// </summary>
[CreateAssetMenu(fileName = "QuestData", menuName = "Scriptable Objects/QuestData")]
public class QuestData : ScriptableObject
{
    public string QuestName;
    public AudioClip hintAudio;
    public float SecToWait;
}