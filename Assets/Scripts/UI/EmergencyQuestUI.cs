using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmergencyQuestUIData : BaseUIData
{
    public int index;

    public EmergencyQuestUIData(int i)
    {
        index = i;
    }
}

public class EmergencyQuestUI : BaseUI
{
    private int index;                       // 몇번 째 챕터인지 체크

    public QuestManager questManager;

    public RawImage mainImg;
    public RawImage subImg;
    public RawImage resultImg;

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

    private QuestData emergencyQuestData;


    private void OnEnable()
    {
        main.SetActive(true);
        Temp.SetActive(false);
        result.SetActive(false);
        PoolManager.Instance.ready = true;

        // GameManager.gameManager.PauseGame();

        questManager.adventureDatas.Clear();

        for (int idx = 0; idx < 5; idx++)
        {
            if (!PoolManager.Instance.adventureBtn[idx].interactable && !PoolManager.Instance.gaugeObject[idx].activeSelf)
            {
                PoolManager.Instance.testScripts[idx].AdventureAwake(idx + 1);
            }
        }

    }

    private void OnDisable()
    {
        // GameManager.gameManager.PauseGame();
    }

    // 초기 데이터 초기화
    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        var emergencyQuestUIData = uiData as EmergencyQuestUIData;
        index = emergencyQuestUIData.index;

        questManager = GetComponent<QuestManager>();
        questManager.detachIndex = index;

        if (index == 11)  // 챕터 1 - 슬라임 홍수
        {
            emergencyQuestId = 132901;
            mainImg.texture = Resources.Load("Arts/EmergencyQuest/slimePaper") as Texture2D;
            subImg.texture = Resources.Load("Arts/EmergencyQuest/slimePaper") as Texture2D;
            resultImg.texture = Resources.Load("Arts/EmergencyQuest/slimePaper") as Texture2D;
        }
        else if (index == 12)  // 챕터 2 - 고블린 어벤져스
        {
            emergencyQuestId = 133902;
            mainImg.texture = Resources.Load("Arts/EmergencyQuest/goblinPaper") as Texture2D;
            subImg.texture = Resources.Load("Arts/EmergencyQuest/goblinPaper") as Texture2D;
            resultImg.texture = Resources.Load("Arts/EmergencyQuest/goblinPaper") as Texture2D;
        }
        else if (index == 13)  // 챕터 3 - 드래곤
        {
            emergencyQuestId = 139999;
            mainImg.texture = Resources.Load("Arts/EmergencyQuest/dragonPaper") as Texture2D;
            subImg.texture = Resources.Load("Arts/EmergencyQuest/dragonPaper") as Texture2D;
            resultImg.texture = Resources.Load("Arts/EmergencyQuest/dragonPaper") as Texture2D;
        }

        // 퀘스트 ID로 퀘스트 정보 가져오기
        emergencyQuestData = DataTableManager.Instance.GetQuestData(emergencyQuestId);

        // UI Text들 값 할당
        nameTxt.text = emergencyQuestData.questName;
        rankTxt.text = "등급 : " + emergencyQuestData.questLevel;
        descTxt.text = $"{emergencyQuestData.questScript}";
        rewardTxt.text = "보상 : " + emergencyQuestData.questReward + "골드";

        // 보상 값 할당
        reward = emergencyQuestData.questReward;

        // QuestData 초기화 현재 시스템은 파견창 Index에 따라 QuestData가 있음
        // 긴급 의뢰는 파견창을 사용하지 않아서 존재 하지 않는 파견창 Index를 사용함
        // 챕터1 : 11, 챕터2 : 12, 챕터3 : 13 Index를 사용
        PoolManager.Instance.questData.Add(index, emergencyQuestData);
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
            AudioManager.Instance.PlaySFX(SFX.QuestFail);
            reward = 0;
        }
        else if (resultValue == 0)
        {
            resultTxt.text = "긴급 의뢰 성공!";
            resultBtnTxt.text = "+" + reward.ToString() + "G";
            AudioManager.Instance.PlaySFX(SFX.QuestSuccess);
        }
        else if(resultValue == 1)
        {
            resultTxt.text = "긴급 의뢰 대성공!!!";
            resultBtnTxt.text = "+" + (reward * 2).ToString() + "G";
            AudioManager.Instance.PlaySFX(SFX.QuestSuccess);
            reward *= 2;
        }

        Temp.SetActive(false);
        main.SetActive(false);
        result.SetActive(true);


        PoolManager.Instance.ready = false;

        // 해당 긴급 의뢰가 끝나면 QuestData해제 - 메모리 유지관리 목적
        PoolManager.Instance.questData.Remove(index);
    }

    public void OnClickResultBtn()
    {
        // 성공이면 골드 추가
        GameInfo.gameInfo.ChangeGold(reward);
        GameInfo.gameInfo.CalculateTodayGold(reward);
        GameInfo.gameInfo.addGold += reward;

        // UI 닫기
        UIManager.Instance.CloseUI(this);

        if(resultValue != -1)
        {
            // 긴급 의뢰(스토리 의뢰) 성공 및 대성공일 때 함수 실행
            ScriptHandler.scriptHandler.ScriptPlayQuestID(emergencyQuestId);
        }

        // 파견 보냈던 모험가 다시 풀기
        // 의뢰 종료시 모험가 다시 사용하게 Test
        foreach (var i in questManager.adventureDatas)
        {
            PoolManager.Instance.usingAdventureList.Remove(i.adventureId); // 파견 중이였던 걸 해제
        }

        if (resultValue == -1)
        {
            Fade.Instance.DoFade(Color.black, 0f, 1f, 1f, 1f, false, () =>
            {
                var uiData = new ConfirmUIData();
                uiData.confirmType = ConfirmType.OK;
                uiData.descTxt = "게임 오버";
                uiData.okBtnTxt = "종료";
                uiData.onClickOKBtn = () =>
                {
                    Application.Quit();
                };
                UIManager.Instance.OpenUI<ConfirmUI>(uiData);
                UIManager.Instance.GetActiveUI<ConfirmUI>().transform.SetParent(UIManager.Instance.fadeCanvasTrs);



            });
        }
    }
    
    public void OnClickGiveUpBtn()
    {
        var uiData = new ConfirmUIData();
        uiData.confirmType = ConfirmType.OK_CANCEL;
        uiData.descTxt = "포기시 게임오버 됩니다.";
        uiData.okBtnTxt = "포기";
        uiData.cancelBtnTxt = "취소";
        uiData.onClickOKBtn = () =>
        {
            Fade.Instance.DoFade(Color.black, 0f, 1f, 1f, 1f, false, () =>
            {
                var uiData = new ConfirmUIData();
                uiData.confirmType = ConfirmType.OK;
                uiData.descTxt = "게임 오버";
                uiData.okBtnTxt = "종료";
                uiData.onClickOKBtn = () =>
                {
                    Application.Quit();
                };
                UIManager.Instance.OpenUI<ConfirmUI>(uiData);
                UIManager.Instance.GetActiveUI<ConfirmUI>().transform.SetParent(UIManager.Instance.fadeCanvasTrs);

            });
        };
        UIManager.Instance.OpenUI<ConfirmUI>(uiData);

        
    }
}
