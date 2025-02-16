using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestResult : BaseUI
{
    public TextMeshProUGUI txt;
    public GameObject receiptBtn;

    private int resultIndex;        // 파견창 인덱스
    private int resul;              // 결과 / -1 : 전멸 / 0 : 일반 성공 / 1 : 대성공

    private void Start()
    {
        receiptBtn.SetActive(false);
        StartCoroutine(UpdateResultCo());
    }

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        var questResultIndex = uiData as QuestResultIndex;
        resultIndex = questResultIndex.index;
    }

    public override void Init(Transform anchor)
    {
        base.Init(anchor);

        var rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(800f, 450f);

        QuestManager.Instance.Calculation(resultIndex);
        resul = QuestManager.Instance.resultList[resultIndex];
    }

    public void OnClickReceiptBtn()
    {
        // 골드 추가하고
        // 모험가 다시 풀기

        UIManager.Instance.CloseUI(this);
    }

    private IEnumerator UpdateResultCo()
    {
        txt.text = "의뢰 확인 중...";

        yield return new WaitForSeconds(2f);

        txt.text = "의뢰 성공";
        receiptBtn.SetActive(true);
    }
}
