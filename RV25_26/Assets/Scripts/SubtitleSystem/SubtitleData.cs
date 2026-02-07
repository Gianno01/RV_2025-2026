using UnityEngine;

/// <summary>
/// Una quest ha un nome, un hint come aiuto per completarla e dei secondi da attendere prima che venga proposto l'hint.
/// Se SecToWait = -1, allora l'hint sarà effetto di altre cause.
/// </summary>
[CreateAssetMenu(fileName = "SubtitleData", menuName = "Scriptable Objects/SubtitleData")]
public class SubtitleData : ScriptableObject
{
    [TextArea(10, 10)]
    public string text;
}