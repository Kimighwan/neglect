using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class ReportUI : BaseUI
{
    public Image image;
    public TextMeshProUGUI day;
    public TextMeshProUGUI quest;
    public TextMeshProUGUI NowScore;
    public TextMeshProUGUI NextScore;

    private void OnEnable() {
        day.text = $"{GameInfo.gameInfo.Day}일차";
        quest.text = $"<완료한 의뢰>\n브론즈 X 3 => 300\n실버 X 1 => 200\n금일 점수 500";
        DoFadeIn();
    }
    public void OnClickCloseBut()
    {
        image.gameObject.SetActive(true);
        StartCoroutine(FadeOut(1f, 0f, 0f, 1f));
    }

    private void DoFadeIn() {
        StartCoroutine(FadeIn(1f, 0f, 1f, 0f));
    }

    private IEnumerator FadeIn(float duration, float startDelay, float startAlpha, float endAlpha)
    {
        yield return new WaitForSeconds(startDelay);
        image.color = new Color(0f, 0f, 0f, startAlpha);

        var startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < duration)
        {
            image.color = new Color(0f, 0f, 0f, Mathf.Lerp(startAlpha, endAlpha, (Time.realtimeSinceStartup - startTime) / duration));
            yield return null;
        }

        image.color = new Color(0f, 0f, 0f, endAlpha);
        image.gameObject.SetActive(false);
    }

    private IEnumerator FadeOut(float duration, float startDelay, float startAlpha, float endAlpha)
    {
        yield return new WaitForSeconds(startDelay);
        image.color = new Color(0f, 0f, 0f, startAlpha);

        var startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < duration)
        {
            image.color = new Color(0f, 0f, 0f, Mathf.Lerp(startAlpha, endAlpha, (Time.realtimeSinceStartup - startTime) / duration));
            yield return null;
        }

        image.color = new Color(0f, 0f, 0f, endAlpha);
        GameInfo.gameInfo.ComeMorning();
        CloseUI();
    }
}
