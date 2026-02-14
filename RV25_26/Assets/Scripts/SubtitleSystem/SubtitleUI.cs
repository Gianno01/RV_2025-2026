using UnityEngine;
using TMPro;
using System.Collections;
using System.Text.RegularExpressions;

public class SubtitleUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _textBox;
    [SerializeField] private float _timeToWaitBetweenPeriod;
    //[SerializeField] private AppEventData _onTextEnd;

    public void OpenUI()
    {
        _canvasGroup.alpha = 1;
    }
    public void Show(string text)
    {
        /*StopCoroutine("ShowText");
        StartCoroutine(ShowText(text));*/
        _textBox.text = text;
    }

    public void CloseUI()
    {
        _textBox.text = "";
        _canvasGroup.alpha = 0;
        //_onTextEnd.Raise();
    }

    /*private IEnumerator ShowText(string text)
    {
        _canvasGroup.alpha = 1;
        string[] periods = Regex.Split(text, "QQ");
        
        foreach(string s in periods)
        {
            _textBox.text = s;
            yield return new WaitForSeconds(_timeToWaitBetweenPeriod);
        }

        _textBox.text = "";
        _canvasGroup.alpha = 0;
        _onTextEnd.Raise();
    }*/
}