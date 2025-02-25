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


        // 의뢰 종료시 모험가 다시 사용하게 Test
        foreach (var i in QuestManager.Instance.adventureDatas[resultIndex])
        {
            PoolManager.Instance.usingAdventureList.Remove(i.adventureId);
        }
        
        if (QuestManager.Instance.resultList[resultIndex] == 0)
        {
            txt.text = "의뢰 성공";
            receiptBtn.SetActive(true);

            GameInfo.gameInfo.ChangeGold(QuestManager.Instance.questData[resultIndex].questReward);
        }
        else if(QuestManager.Instance.resultList[resultIndex] == 1)
        {
            txt.text = "의뢰 대성공!!!";
            receiptBtn.SetActive(true);

            GameInfo.gameInfo.ChangeGold(QuestManager.Instance.questData[resultIndex].questReward * 2);
        }
        else if(QuestManager.Instance.resultList[resultIndex] == -1)
        {
            txt.text = "전멸...";
            receiptBtn.SetActive(false);

            // 전멸 시 모험가 삭제 Test
            foreach (var i in QuestManager.Instance.adventureDatas[resultIndex])
            {
                var adventureId = PlayerPrefs.GetString("AdventureId");
                var adventureIds = adventureId.Split(',');

                if (adventureId == "") yield break;

                string addId = "";

                foreach (var item in adventureIds)
                {
                    int adventureIdOfInt = Convert.ToInt32(item);

                    if (i.adventureId != adventureIdOfInt)
                    {
                        if (addId == "")
                            addId += adventureIdOfInt.ToString();
                        else
                            addId += "," + adventureIdOfInt.ToString();
                    }
                }

                PlayerPrefs.SetString("AdventureId", addId);
            }
        }
    }
}
