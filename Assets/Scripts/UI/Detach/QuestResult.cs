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
    private int result;             // 결과 / -1 : 전멸 / 0 : 일반 성공 / 1 : 대성공

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

        //QuestManager.Instance.Calculation(resultIndex)????????????????
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
        Debug.Log($"0 : 성공 / 1 : 대성공 / -1 : 전멸 = {QuestManager.Instance.resultList[resultIndex]}");

        if(QuestManager.Instance.resultList[resultIndex] == 0)
        {
            txt.text = "의뢰 성공";
            receiptBtn.SetActive(true);
        }
        else if(QuestManager.Instance.resultList[resultIndex] == 1)
        {
            txt.text = "의뢰 대성공!!!";
            receiptBtn.SetActive(true);
        }
        else if(QuestManager.Instance.resultList[resultIndex] == -1)
        {
            txt.text = "전멸...";
            receiptBtn.SetActive(false);
        }
    }
}
