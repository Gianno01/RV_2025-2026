using UnityEngine;
using TMPro;
using System.Collections;

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
        string[] periods = text.Split(".");
        
        foreach(string s in periods)
        {
            textBox.text = periods[0];
            yield return new WaitForSeconds(timeToWaitBetweenPeriod);
        }

        textBox.text = "";
        canvasGroup.alpha = 0;
    }
}