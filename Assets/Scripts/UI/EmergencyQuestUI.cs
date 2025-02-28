using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmergencyQuestUI : BaseUI
{
    public int index;                       // 몇번 째 챕터인지 체크

    public QuestManager questManager;

    public TextMeshProUGUI nameTxt;         // 의뢰 이름
    public TextMeshProUGUI rankTxt;         // 의뢰 랭크
    public TextMeshProUGUI descTxt;         // 설명
    public TextMeshProUGUI rewardTxt;       // 보상
    public TextMeshProUGUI resultTxt;       // 결과 Text
    public TextMeshProUGUI resultBtnTxt;    // 결과 버튼 Text

    public GameObject Temp;                 // 진행 중을 알리는 UI
    public GameObject main;                 // 주 UI
    public GameObject result;               // 결과 UI

    private int emergencyQuestId;
    private int resultValue;                // -1 : 전멸, 0 : 일반 성공, 1 : 대성공
    private int reward;                     // 보상

    private QuestData emetgencyQuestData;

    

    // 초기 데이터 초기화
    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        questManager = GetComponent<QuestManager>();
        questManager.detachIndex = index;

        if (index == 11)  // 챕터 1 - 슬라임 홍수
        {
            emergencyQuestId = 132901;
        }
        else if (index == 12)  // 챕터 2 - 고블린 어벤져스
        {
            emergencyQuestId = 133902;
        }
        else if (index == 13)  // 챕터 3 - 드래곤
        {
            emergencyQuestId = 139999;
        }

        // 퀘스트 ID로 퀘스트 정보 가져오기
        emetgencyQuestData = DataTableManager.Instance.GetQuestData(emergencyQuestId);

        // UI Text들 값 할당
        nameTxt.text = emetgencyQuestData.questName;
        rankTxt.text = "등급 : " + emetgencyQuestData.questLevel;
        descTxt.text = emetgencyQuestData.questScript;
        rewardTxt.text = "보상 : " + emetgencyQuestData.questReward + "골드";

        // 보상 값 할당
        reward = emetgencyQuestData.questReward;

        // QuestData 초기화 현재 시스템은 파견창 Index에 따라 QuestData가 있음
        // 긴급 의뢰는 파견창을 사용하지 않아서 존재 하지 않는 파견창 Index를 사용함
        // 챕터1 : 11, 챕터2 : 12, 챕터3 : 13 Index를 사용
        PoolManager.Instance.questData.Add(index, emetgencyQuestData);
    }

    public void OnClickAdventureBtn()   // 모험가 선택 버튼
    {
        var detachAdventureUI = new AdventureIndexClass(index);
        UIManager.Instance.OpenUI<DetachAdventureListUI>(detachAdventureUI);
    }

    public void OnClickStartBtn()   // 시작 버튼
    {
        // 대충 긴급 의뢰 진행 중 UI 띄워주고
        Temp.SetActive(true);

        // 결과 보여주면 됨
        StartCoroutine(ShowResult());
    }

    private IEnumerator ShowResult()
    {
        yield return null;

        yield return new WaitForSeconds(2f);

        // 결과 가져오기
        resultValue = PoolManager.Instance.resultList[index];

        if (resultValue == -1)
        {
            resultTxt.text = "긴급 의뢰 실패";
            resultBtnTxt.text = "확인";
        }
        else if (resultValue == 0)
        {
            resultTxt.text = "긴급 의뢰 성공!";
            resultBtnTxt.text = "+" + reward.ToString() + "G";
        }
        else
        {
            resultTxt.text = "긴급 의뢰 대성공!!!";
            resultBtnTxt.text = "+" + reward.ToString() + "G";
        }

        Temp.SetActive(false);
        main.SetActive(false);
        result.SetActive(true);

        PoolManager.Instance.ready = false;

        // 해당 긴급 의뢰가 끝나면 QuestData해제 - 메모리 유지관리 목적
        PoolManager.Instance.questData.Remove(index);
    }
}
