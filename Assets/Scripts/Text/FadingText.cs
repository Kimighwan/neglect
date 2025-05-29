using UnityEngine;
using TMPro;
using System.Collections;

public class FadingText : MonoBehaviour
{
    [Tooltip("페이드 아웃 시키면서 Off시킬 오브젝트")]
    public GameObject destroyObject;
    [Tooltip("페이드 인→아웃 전체 주기(초)")]
    public float cycleDuration = 1f;

    private Color _originalColor;
    private float _halfCycle;
    private TextMeshProUGUI text;

    void Awake()
    {
        // 잘못된 if문 제거하고, 무조건 GetComponent로 할당
        text = GetComponent<TextMeshProUGUI>();
        if (text == null)
        {
            Debug.LogError($"[{nameof(FadingText)}] TextMeshProUGUI 컴포넌트를 찾을 수 없습니다.");
            enabled = false;
            return;
        }

        // 텍스트가 가진 원래 색상 저장
        _originalColor = text.color;
    }

    void Start()
    {
        _halfCycle = cycleDuration * 0.5f;
    }

    void Update()
    {
        if (_halfCycle <= 0f) return;

        float ping = Mathf.PingPong(Time.time, _halfCycle);
        float alpha = ping / _halfCycle;

        text.color = new Color(
            _originalColor.r,
            _originalColor.g,
            _originalColor.b,
            alpha
        );
    }

    public IEnumerator FadeOutRoutine(float duration)
    {
        if (text == null) yield break;

        float elapsed = 0f;
        Color startColor = text.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float a = Mathf.Lerp(startColor.a, 0f, elapsed / duration);
            text.color = new Color(startColor.r, startColor.g, startColor.b, a);
            yield return null;
        }

        text.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        destroyObject?.SetActive(false);
    }

    public void StartFadeOut(float duration)
    {
        StartCoroutine(FadeOutRoutine(duration));
    }
}
