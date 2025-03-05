using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestResult : BaseUI
{
    public TextMeshProUGUI txt;
    public TextMeshProUGUI rewardTxt;

    public GameObject receiptBtn;
    public GameObject diaOKBtn;     // 전멸시 사용할 버튼

    [SerializeField] public int resultIndex;        // 파견창 인덱스
    private int result;             // 결과 / -1 : 전멸 / 0 : 일반 성공 / 1 : 대성공
    private int reward;

    private void OnEnable()
    {
        reward = 0;
        receiptBtn.SetActive(false);
        diaOKBtn.SetActive(false);
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

    public void OnClickReceiptBtn()     // 파견 결과에 따른 골드 받기 버튼
    {
        // 골드 추가하고
        GameInfo.gameInfo.ChangeGold(reward);
        GameInfo.gameInfo.CalculateTodayGold(reward);
        GameInfo.gameInfo.addGold += reward;

        SetCommon();

        UIManager.Instance.CloseUI(this);

        if (PoolManager.Instance.specialAdventureAdd)
        {
            // 특수 모험가 합류 예정
            var adventureId = PlayerPrefs.GetString("AdventureId");
            var adventureIds = adventureId.Split(',');

            int tmpId = 0;
            string name = "";

            switch (PoolManager.Instance.questData[resultIndex].questMonster)
            {
                case "설녀":
                    tmpId = 128013232;
                    name = "설녀";
                    PlayerPrefs.SetInt("설녀", 1);
                    break;
                case "호문쿨루스":
                    tmpId = 128022121;
                    name = "호문쿨루스";
                    PlayerPrefs.SetInt("호문쿨루스", 1);
                    break;
                case "헤츨링":
                    tmpId = 128031313;
                    name = "헤츨링";
                    PlayerPrefs.SetInt("헤츨링", 1);
                    break;
            }

            string addId = tmpId.ToString();

            foreach (var item in adventureIds)
            {
                int adventureIdOfInt = Convert.ToInt32(item);

                addId += "," + adventureIdOfInt.ToString();
            }

            PlayerPrefs.SetString("AdventureId", addId);

            // 모험가가 가득차 보상을 받을 수 없습니다.
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK;
            uiData.descTxt = $"새로운 모험가 합류!!\n-{name}-";
            uiData.okBtnTxt = "확인";
            uiData.onClickOKBtn = () =>
            {
                PoolManager.Instance.specialAdventureAdd = false;
            };
            UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            return;
        }
    }

    public void OnClickDieOKBtn()     // 전멸 확인 버튼
    {
        SetCommon();


        UIManager.Instance.CloseUI(this);
    }

    private IEnumerator UpdateResultCo()
    {
        txt.text = "의뢰 확인 중...";

        yield return new WaitForSeconds(1.5f);      

        // 의뢰 종료시 모험가 다시 사용하게 Test
        foreach (var i in PoolManager.Instance.questManagers[resultIndex - 1].adventureDatas)
        {
            PoolManager.Instance.usingAdventureList.Remove(i.adventureId); // 파견 중이였던 걸 해제
        }
        
        if (PoolManager.Instance.resultList[resultIndex] == 0)
        {
            AudioManager.Instance.PlaySFX(SFX.QuestSuccess);
            txt.text = "의뢰 성공";
            rewardTxt.text = "+G " + PoolManager.Instance.questData[resultIndex].questReward.ToString();
            receiptBtn.SetActive(true);
            diaOKBtn.SetActive(false);
            SetMonsterPlayerPrefs();

            reward = PoolManager.Instance.questData[resultIndex].questReward;
            GameInfo.gameInfo.CheckSuccessTier(PoolManager.Instance.questData[resultIndex].questLevel);
            ScriptDialogHandler.handler.ConditionalScriptPlay(PoolManager.Instance.questData[resultIndex].questId);
        }
        else if(PoolManager.Instance.resultList[resultIndex] == 1)
        {
            AudioManager.Instance.PlaySFX(SFX.QuestSuccess);
            txt.text = "의뢰 대성공!!!";
            rewardTxt.text = "+G " + (PoolManager.Instance.questData[resultIndex].questReward * 2).ToString();
            receiptBtn.SetActive(true);
            diaOKBtn.SetActive(false);
            SetMonsterPlayerPrefs();

            reward = PoolManager.Instance.questData[resultIndex].questReward * 2;
            GameInfo.gameInfo.CheckSuccessTier(PoolManager.Instance.questData[resultIndex].questLevel);
            ScriptDialogHandler.handler.ConditionalScriptPlay(PoolManager.Instance.questData[resultIndex].questId);
        }
        else if(PoolManager.Instance.resultList[resultIndex] == -1)
        {
            AudioManager.Instance.PlaySFX(SFX.QuestFail);
            txt.text = "전멸...";
            receiptBtn.SetActive(false);
            diaOKBtn.SetActive(true);

            reward = 0;

            // 전멸 시 모험가 삭제 Test
            foreach (var i in PoolManager.Instance.questManagers[resultIndex - 1].adventureDatas)
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

        // 모험가 다시 풀기
        PoolManager.Instance.questManagers[resultIndex - 1].adventureDatas.Clear(); // 파견창에 맞는 모험가 데이터 삭제
        DeleteQuest();
    }

    private void SetMonsterPlayerPrefs()
    {
        PlayerPrefs.SetInt($"{PoolManager.Instance.questData[resultIndex].questMonster}", 1);
    }

    private void SetCommon()
    {
        // 각 파견창의 게이지 비활성화
        PoolManager.Instance.gaugeObject[resultIndex - 1].SetActive(false);

        // 초기화 버튼 활성화
        PoolManager.Instance.awakeBtn[resultIndex - 1].SetActive(true);

        // 결과 확인 버튼 비활성화
        PoolManager.Instance.resultBtn[resultIndex - 1].gameObject.SetActive(false);

        // 버튼 다시 활성화
        PoolManager.Instance.BtnActive(resultIndex);

        // 버튼 Text 다시 설정
        PoolManager.Instance.questTxt[resultIndex - 1].text = "의뢰 선택";
        PoolManager.Instance.adventureTxt[resultIndex - 1].text = "모험가 선택";

        PoolManager.Instance.usingQuestList.Remove(PoolManager.Instance.questData[resultIndex].questId);

        PoolManager.Instance.resultList.Remove(resultIndex);
    }

    private void DeleteQuest() // 의뢰 목록에서 의뢰 제거
    {
        int deleteId = PoolManager.Instance.questData[resultIndex].questId;

        var questId = PlayerPrefs.GetString("QuestId");
        var questIds = questId.Split(',');

        string addId = "";

        foreach (var item in questIds)
        {
            int questIdOfInt = Convert.ToInt32(item);

            if (deleteId != questIdOfInt)
            {
                if (addId == "")
                    addId += questIdOfInt.ToString();
                else
                    addId += "," + questIdOfInt.ToString();
            }
        }

        PlayerPrefs.SetString("QuestId", addId);
    }
}
