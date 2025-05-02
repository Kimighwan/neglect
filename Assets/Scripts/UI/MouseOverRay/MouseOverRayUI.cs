using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MouseOverRayUIData : BaseUIData
{
    public string str;
}

public class MouseOverRayUI : BaseUI
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private RectTransform imageRT;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        MouseOverRayUIData data = uiData as MouseOverRayUIData;
        text.text = data.str;
    }

    public override void Init(Transform anchor, RectTransform canvasRT = null)
    {
        base.Init(anchor);

        Vector2 localPos;   // 변환된 Canvas 내 좌표

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, Input.mousePosition, null, out localPos); // 마우스 위치 가져오기

        localPos = new Vector2(localPos.x + 150, localPos.y);   // 오버레이 위치 조절
        var rectTransform = GetComponent<RectTransform>();
        rectTransform.localPosition = localPos;                 // 오버레이 위치 설정

        float size = 100 + text.text.Length * 25;            // 오버레이 크기 조절
        imageRT.sizeDelta = new Vector2(150, 150);              // 오버레이 크기 설정 # 일단은 고정 수치로 설정
    }
}
