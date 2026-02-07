using UnityEngine;

public class TextSpot : MonoBehaviour, ITextChangeable
{
    [SerializeField] private SubtitleData currentSubtitle;
    [SerializeField] private AppEventData _onSubtitleShow;
    public void ChangeText(SubtitleData text)
    {
        currentSubtitle = text;
    }

    public void ShowText()
    {
        _onSubtitleShow.RaiseWithParam(currentSubtitle);
    }
}