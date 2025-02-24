using System.Collections;
using TMPro;
using UnityEngine;
public class SimpleInfoUI : BaseUI
{
    public TextMeshProUGUI info;
    public float animationDuration = 0.5f; // 애니메이션 지속 시간
    private Vector3 targetScale = Vector3.one; // 최종 스케일 (원래 크기)

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        var infoData = uiData as StringInfo;
        info.text = infoData.str;
    }
    void OnEnable()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(ScaleUp());
    }

    IEnumerator ScaleUp()
    {
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            // 0~1 사이의 진행률을 구하고 SmoothStep을 통해 부드러운 효과 적용
            float t = elapsedTime / animationDuration;
            float scaleFactor = Mathf.SmoothStep(0f, 1f, t);
            transform.localScale = targetScale * scaleFactor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 정확히 최종 스케일로 설정
        transform.localScale = targetScale;
    }

    public override void OnClickCloseButton()
    {
        base.OnClickCloseButton();
        //Destroy(this);
    }
}
