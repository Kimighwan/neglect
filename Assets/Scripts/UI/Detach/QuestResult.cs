using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestResult : BaseUI
{
    public TextMeshProUGUI txt;
    public TextMeshProUGUI rewardTxt;

    public GameObject receiptBtn;

    private int resultIndex;        // 파견창 인덱스
    private int result;             // 결과 / -1 : 전멸 / 0 : 일반 성공 / 1 : 대성공
    private int reward;

    private void Start()
    {
        reward = 0;
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
        rectTransform.sizeDelta = new Vector2(300f, 200f);
        rectTransform.localScale = new Vector3(2, 2, 2);

        //QuestManager.Instance.Calculation(resultIndex)????????????????
    }

    public void OnClickReceiptBtn()
    {
        // 골드 추가하고
        GameInfo.gameInfo.ChangeGold(reward);

        // 모험가 다시 풀기

        UIManager.Instance.CloseUI(this);
    }

    private IEnumerator UpdateResultCo()
    {
        txt.text = "의뢰 확인 중...";

        yield return new WaitForSeconds(2f);

        rewardTxt.text = "+G " + QuestManager.Instance.questData[resultIndex].questReward.ToString();

        // 버튼 Text 다시 설정
        QuestManager.Instance.questTxt[resultIndex - 1].text = "의뢰 선택";
        QuestManager.Instance.adventureTxt[resultIndex - 1].text = "모험가 선택";

        // 버튼 다시 활성화
        QuestManager.Instance.BtnActive(resultIndex);

        // 결과 확인 버튼 비활성화
        QuestManager.Instance.resultBtn[resultIndex - 1].gameObject.SetActive(false);

        // 의뢰 종료시 모험가 다시 사용하게 Test
        foreach (var i in QuestManager.Instance.adventureDatas[resultIndex])
        {
            PoolManager.Instance.usingAdventureList.Remove(i.adventureId);
        }
        
        if (QuestManager.Instance.resultList[resultIndex] == 0)
        {
            AudioManager.Instance.PlaySFX(SFX.QuestSuccess);
            txt.text = "의뢰 성공";
            receiptBtn.SetActive(true);
            SetMonsterPlayerPrefs();

            reward = QuestManager.Instance.questData[resultIndex].questReward;
            GameInfo.gameInfo.CheckSuccessTier(QuestManager.Instance.questData[resultIndex].questLevel);
            ScriptDialogHandler.handler.ConditionalScriptPlay(QuestManager.Instance.questData[resultIndex].questId);
        }
        else if(QuestManager.Instance.resultList[resultIndex] == 1)
        {
            AudioManager.Instance.PlaySFX(SFX.QuestSuccess);
            txt.text = "의뢰 대성공!!!";
            receiptBtn.SetActive(true);
            SetMonsterPlayerPrefs();

            reward = QuestManager.Instance.questData[resultIndex].questReward * 2;
            GameInfo.gameInfo.CheckSuccessTier(QuestManager.Instance.questData[resultIndex].questLevel);
            ScriptDialogHandler.handler.ConditionalScriptPlay(QuestManager.Instance.questData[resultIndex].questId);
        }
        else if(QuestManager.Instance.resultList[resultIndex] == -1)
        {
            AudioManager.Instance.PlaySFX(SFX.QuestFail);
            txt.text = "전멸...";
            receiptBtn.SetActive(false);

            reward = 0;

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

        QuestManager.Instance.adventureDatas[resultIndex].Clear(); // 파견창에 맞는 모험가 데이터 삭제
    }

    private void SetMonsterPlayerPrefs()
    {
        PlayerPrefs.SetInt($"{QuestManager.Instance.questData[resultIndex].questMonster}", 1);
    }
}
