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
    public Image StampShadow;
    public Image Stamp;

    private float shadowDuration = 0.7f;
    private Vector3 startScale = new Vector3(0.1f, 0.1f, 0.1f);
    private Vector3 endScale = Vector3.one;
    private Color startColor = new Color(0f, 0f, 0f, 0f);
    private Color endColor = new Color(0f, 0f, 0f, 0.9f);
    private bool stampOn = false;
    private bool inputLock = false;

    private void OnEnable() {
        inputLock = true;
        day.text = "";
        quest.text = "";
        NowScore.text = "";
        NextScore.text = "";
        if (Stamp != null) {
            Stamp.enabled = false;
            stampOn = false;
        }
        if (StampShadow != null) {
            StampShadow.rectTransform.localScale = startScale;
            StampShadow.color = startColor;
        }
        StartCoroutine(FadeIn(1f, 0f, 1f, 0f));
        Invoke("StartTyping", 0.3f);
    }

    void Update()
    {
        if (!inputLock && (Input.anyKeyDown || Input.GetMouseButtonDown(0))) {
            inputLock = true;
            if (!stampOn) StartCoroutine(AnimateStampShadow());
            else OnClickCloseBut();
        }
    }

    public void OnClickCloseBut()
    {
        image.gameObject.SetActive(true);
        StartCoroutine(FadeOut(1f, 0f, 0f, 1f));
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
        AudioManager.Instance.PlayBGM(BGM.TypeWriter);
        StartCoroutine(TypeDialog($"{GameInfo.gameInfo.Day}일차", $"<완료한 의뢰>\n브론즈 X 3 => 300\n실버 X 1 => 200\n금일 점수 500"
        , $"현재 점수 {GameInfo.gameInfo.PlayerScore}점", $"?차 목표까지 ?점\n기한까지 ?일"));
    }
    private IEnumerator TypeDialog(string s1, string s2, string s3, string s4)
    {
        day.text = "";
        foreach (char letter in s1)
        {
            day.text += letter;
            yield return new WaitForSeconds(0.025f);
        }

        yield return new WaitForSeconds(0.2f);

        quest.text = "";
        foreach (char letter in s2)
        {
            quest.text += letter;
            yield return new WaitForSeconds(0.025f);
        }

        yield return new WaitForSeconds(0.2f);

        NowScore.text = "";
        foreach (char letter in s3)
        {
            NowScore.text += letter;
            yield return new WaitForSeconds(0.025f);
        }

        yield return new WaitForSeconds(0.2f);

        NextScore.text = "";
        foreach (char letter in s4)
        {
            NextScore.text += letter;
            yield return new WaitForSeconds(0.025f);
        }

        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlaySFX(SFX.CoinDrop1);
        AudioManager.Instance.PlaySFX(SFX.CoinDrop2);
        inputLock = false;
    }

    IEnumerator AnimateStampShadow()
    {
        AudioManager.Instance.PlaySFX(SFX.Stamp);
        float elapsed = 0f;
        while (elapsed < shadowDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / shadowDuration);

            // 스케일 및 색상 선형 보간
            if (StampShadow != null)
            {
                StampShadow.rectTransform.localScale = Vector3.Lerp(startScale, endScale, t);
                StampShadow.color = Color.Lerp(startColor, endColor, t);
            }

            yield return null;
        }
        // 애니메이션 완료 시 스탬프 이미지 활성화
        if (Stamp != null) {
            StampShadow.enabled = false;
            Stamp.enabled = true;
            stampOn = true;
            inputLock = false;
        }
    }
}
