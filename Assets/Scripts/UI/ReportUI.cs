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
    public GameObject clickNextTxt;
    public Image StampShadow;
    public Image Stamp;

    private float shadowDuration = 0.3f;
    private Vector3 startScale = new Vector3(0.1f, 0.1f, 0.1f);
    private Vector3 endScale = Vector3.one;
    private Vector2 tmpV;
    private Quaternion tmpQ;
    private Color startColor = new Color(0f, 0f, 0f, 0f);
    private Color endColor = new Color(0f, 0f, 0f, 0.9f);
    private bool stampOn = false;
    private bool inputLock = true; // 기본적으로 입력 잠금

    // 새 플래그 추가
    private bool isTyping = false;
    private bool skipTyping = false;

    private void OnEnable() {
        RandomizeStamp();
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
        // 타이핑 중이라면 입력을 감지해 skipTyping 플래그 설정
        if (isTyping && (Input.anyKeyDown || Input.GetMouseButtonDown(0))) {
            skipTyping = true;
        }
        // 타이핑이 끝난 후의 입력 처리 (스탬프 애니메이션 또는 닫기)
        else if (!isTyping && !inputLock && (Input.anyKeyDown || Input.GetMouseButtonDown(0))) {
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
        // 타이핑 시작 전에 플래그 초기화
        isTyping = true;
        skipTyping = false;
        inputLock = false; // 타이핑 중엔 입력을 받아서 건너뛰기 가능
        AudioManager.Instance.PlayBGM(BGM.TypeWriter);
        StartCoroutine(TypeDialog($"{GameInfo.gameInfo.Day}일차", 
            $"\n\n금일 점수 {GameInfo.gameInfo.TodayScore}점" +
            $"\n금일 번 골드 {GameInfo.gameInfo.TodayGold}G\n금일 객실 수입 {GameInfo.gameInfo.plusGold}G",
            $"현재 점수 {GameInfo.gameInfo.addGold}점", 
            $"\n현재 골드 {GameInfo.gameInfo.Gold}G"));
    }

    private IEnumerator TypeDialog(string s1, string s2, string s3, string s4)
    {
        // day 텍스트 타이핑
        day.text = "";
        foreach (char letter in s1)
        {
            if (skipTyping)
            {
                day.text = s1;
                break;
            }
            day.text += letter;
            yield return new WaitForSeconds(0.025f);
        }
        if (!skipTyping) yield return new WaitForSeconds(0.2f);

        // quest 텍스트 타이핑
        quest.text = "";
        foreach (char letter in s2)
        {
            if (skipTyping)
            {
                quest.text = s2;
                break;
            }
            quest.text += letter;
            yield return new WaitForSeconds(0.025f);
        }
        if (!skipTyping) yield return new WaitForSeconds(0.2f);

        // NowScore 텍스트 타이핑
        NowScore.text = "";
        foreach (char letter in s3)
        {
            if (skipTyping)
            {
                NowScore.text = s3;
                break;
            }
            NowScore.text += letter;
            yield return new WaitForSeconds(0.025f);
        }
        if (!skipTyping) yield return new WaitForSeconds(0.2f);

        // NextScore 텍스트 타이핑
        NextScore.text = "";
        foreach (char letter in s4)
        {
            if (skipTyping)
            {
                NextScore.text = s4;
                break;
            }
            NextScore.text += letter;
            yield return new WaitForSeconds(0.025f);
        }

        // 타이핑이 모두 끝났으므로 타이핑 플래그 해제 및 효과음 처리
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlaySFX(SFX.CoinDrop1);
        AudioManager.Instance.PlaySFX(SFX.CoinDrop2);
        isTyping = false;
        inputLock = false;

        clickNextTxt.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -440f, 0);
        clickNextTxt.SetActive(true);
    }

    IEnumerator AnimateStampShadow()
    {
        StampShadow.enabled = true;

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
            AudioManager.Instance.PlaySFX(SFX.Stamp);
            Stamp.enabled = true;
            stampOn = true;
            inputLock = false;
        }
    }

    private void RandomizeStamp()
    {
        if (Stamp == null)
            return;
        RectTransform stampRect = Stamp.GetComponent<RectTransform>();
        float randomX = Random.Range(-5f, 5f);
        float randomY = Random.Range(-5f, 5f);

        tmpV = stampRect.anchoredPosition;
        stampRect.anchoredPosition += new Vector2(randomX, randomY);
        
        float randomAngle = Random.Range(-60f, 60f);
        tmpQ = stampRect.localRotation;
        stampRect.localRotation = Quaternion.Euler(0f, 0f, randomAngle);
    }

    private void BackToOriginal() {
        RectTransform stampRect = Stamp.GetComponent<RectTransform>();
        stampRect.anchoredPosition = tmpV;
        stampRect.localRotation = tmpQ;
    }
}