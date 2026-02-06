using UnityEngine;

public class TextSpot : MonoBehaviour, ITextChangeable
{
    [SerializeField] private SubtitleData currentSubtitle;
    public void ChangeText(SubtitleData text)
    {
        currentSubtitle = text;
    }
}