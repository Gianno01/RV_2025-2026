using UnityEngine;
using TMPro;
using System.Collections;
using System.Text.RegularExpressions;

public class SubtitleUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private float timeToWaitBetweenPeriod;
    public void Show(string text)
    {
        StartCoroutine(ShowText(text));
    }

    private IEnumerator ShowText(string text)
    {
        canvasGroup.alpha = 1;
        string[] periods = Regex.Split(text, @"(?<=[.;!?]+)");
        
        foreach(string s in periods)
        {
            textBox.text = s;
            yield return new WaitForSeconds(timeToWaitBetweenPeriod);
        }

        textBox.text = "";
        canvasGroup.alpha = 0;
    }
}