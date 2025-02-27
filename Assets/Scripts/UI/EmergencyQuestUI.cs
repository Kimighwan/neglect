using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmergencyQuestUI : BaseUI
{
    public int index;                   // 몇번 째 챕터인지 체크

    public TextMeshProUGUI titleName;   // 의뢰 이름
    public TextMeshProUGUI rank;        // 의뢰 랭크
    public TextMeshProUGUI desc;        // 설명
    public TextMeshProUGUI reward;      // 보상

    public Button adventureBtn;


    private int emergencyQuestId;

    private QuestData emetgencyQuestData;

    // 초기 데이터 초기화
    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        if(index == 1)  // 챕터 1 - 슬라임 홍수
        {
            emergencyQuestId = 132901;
        }
        else if (index == 2)  // 챕터 2 - 고블린 어벤져스
        {
            emergencyQuestId = 133902;
        }
        else if (index == 3)  // 챕터 3 - 드래곤
        {
            emergencyQuestId = 139999;
        }

        // 퀘스트 ID로 퀘스트 정보 가져오기
        emetgencyQuestData = DataTableManager.Instance.GetQuestData(emergencyQuestId);

        // UI Text들 값 할당
        titleName.text = emetgencyQuestData.questName;
        rank.text = "등급 : " + emetgencyQuestData.questLevel;
        desc.text = emetgencyQuestData.questScript;
        reward.text = "보상 : " + emetgencyQuestData.questReward + "골드";
    }

    public void OnClickAdventureBtn()   // 모험가 선택 버튼
    {


        // 모험가 선택을 완료했다면 버튼 비활성화
        adventureBtn.interactable = false;
    }

    public void OnClickStartBtn()   // 시작 버튼
    {
        // 모험가를 선택하지 않았다면 시작 못 함
        if (adventureBtn.interactable)
        {
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK;
            uiData.descTxt = "모험가를 선택하세요.";
            uiData.okBtnTxt = "확인";
            UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            return;
        }
    }
}
