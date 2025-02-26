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
        day.text = "";
        quest.text = "";
        NowScore.text = "";
        NextScore.text = "";
        DoFadeIn();
        Invoke("StartTyping", 0.5f);
        AudioManager.Instance.PlayBGM(BGM.CoinDrop1);
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
    private void StartTyping()
    {
        StartCoroutine(TypeDialog($"{GameInfo.gameInfo.Day}일차", $"<완료한 의뢰>\n브론즈 X 3 => 300\n실버 X 1 => 200\n금일 점수 500"
        , $"현재 점수 {GameInfo.gameInfo.PlayerScore}점", $"?차 목표까지 ?점\n기한까지 ?일"));
    }
    private IEnumerator TypeDialog(string s1, string s2, string s3, string s4)
    {
        day.text = "";
        foreach (char letter in s1)
        {
            day.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.3f);

        quest.text = "";
        foreach (char letter in s2)
        {
            quest.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.3f);

        NowScore.text = "";
        foreach (char letter in s3)
        {
            NowScore.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.3f);

        NextScore.text = "";
        foreach (char letter in s4)
        {
            NextScore.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlaySFX(SFX.CoinDrop2);
    }
}
