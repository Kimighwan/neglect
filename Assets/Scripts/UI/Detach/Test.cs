using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public QuestManager questManagers;

    public TextMeshProUGUI leftTime; 

    public GameObject image;                // 파견 진행 중 가린막 이미지
    public GameObject resultBtn;
    public GameObject gaugeObject;
    public GameObject awakeBtnObject;

    public RawImage gaugeImage;

    private int startDay;
    private int endDay;
    private int totalDay;

    private bool questStart;        // 파견 시작 : true
    private bool adTutorialOnce = false;

    private void Update()
    {
        if (questStart)
        {
            if(totalDay == 0)
            {
                Debug.Log("0일 짜리 퀘스트 바로 종료");
                leftTime.text = "파견 완료";
                gaugeImage.texture = Resources.Load("Arts/Guage/Pull") as Texture2D;
                //questStart = false;
                image.SetActive(false);
                resultBtn.SetActive(true);
            }
            else if(totalDay == 1)           // 하루 짜리 의뢰
            {
                leftTime.text = "남은 일 : 1일";

                if (GameInfo.gameInfo.Day == endDay)    // 꽉참
                {
                    leftTime.text = "파견 완료";
                    gaugeImage.texture = Resources.Load("Arts/Guage/Pull") as Texture2D;
                    //questStart = false;
                    image.SetActive(false);
                    resultBtn.SetActive(true);
                }
            }
            else if (totalDay == 2)     // 이틀 짜리 의뢰
            {
                leftTime.text = "남은 일 : 2일";

                if (GameInfo.gameInfo.Day == startDay + 1)    // OneOfTwo
                {
                    leftTime.text = "남은 일 : 1일";
                    gaugeImage.texture = Resources.Load("Arts/Guage/OneOfTwo") as Texture2D;
                }
                else if (GameInfo.gameInfo.Day == endDay)    // 꽉참
                {
                    leftTime.text = "파견 완료";
                    gaugeImage.texture = Resources.Load("Arts/Guage/Pull") as Texture2D;
                    //questStart = false;
                    image.SetActive(false);
                    resultBtn.SetActive(true);
                }
            }
            else if (totalDay == 3)     // 삼일 짜리 의뢰
            {
                leftTime.text = "남은 일 : 3일";

                if (GameInfo.gameInfo.Day == startDay + 1)    // TwoOfTree
                {
                    leftTime.text = "남은 일 : 2일";
                    gaugeImage.texture = Resources.Load("Arts/Guage/TwoOfTree") as Texture2D;
                }
                else if (GameInfo.gameInfo.Day == startDay + 2)    // OneOfThree
                {
                    leftTime.text = "남은 일 : 1일";
                    gaugeImage.texture = Resources.Load("Arts/Guage/OneOfThree") as Texture2D;
                }
                else if (GameInfo.gameInfo.Day == endDay)    // 꽉참
                {
                    leftTime.text = "파견 완료";
                    gaugeImage.texture = Resources.Load("Arts/Guage/Pull") as Texture2D;
                    //questStart = false;
                    image.SetActive(false);
                    resultBtn.SetActive(true);
                }
            }
        }
    }

    public void OnClickQuestTestBtn(int index)
    {
        var detachQuestUI = new QuestIndex(index);
        UIManager.Instance.OpenUI<DetachQuestListUI>(detachQuestUI);
    }

    public void OnClickAdventureTestBtn(int index)
    {
        var detachAdventureUI = new AdventureIndexClass(index);
        UIManager.Instance.OpenUI<DetachAdventureListUI>(detachAdventureUI);
        if (!adTutorialOnce) {
            adTutorialOnce = true;
            transform.parent.parent.gameObject.GetComponent<Request>().ClearAdTutorial();
            //GameManager.gameManager.OpenTutorial(590007);
        }
    }
    public void SetAdTutorialOnceTrue() {
        adTutorialOnce = true;
    }

    public void OnClickQuestResultTestBtn(int index)    // 결과 창 띄우기
    {
        questStart = false;

        if (((PoolManager.Instance.questData[index].questId / 100) % 10) == 8)    // 특수 의뢰라면...
        {
            // 근데 성공시에만 체크
            if (PoolManager.Instance.resultList[questManagers.detachIndex] != -1)
            {
                if (CheckMaxAdventure() >= GameInfo.gameInfo.GetMaxAdventurerCounts())   // 모험가 꽉 참.
                {
                    // 모험가가 가득차 보상을 받을 수 없습니다.
                    var uiData = new ConfirmUIData();
                    uiData.confirmType = ConfirmType.OK;
                    uiData.descTxt = "모험가가 가득차 보상을 받을 수 없습니다.";
                    uiData.okBtnTxt = "확인";
                    UIManager.Instance.OpenUI<ConfirmUI>(uiData);
                    return;
                }
                else
                {
                    PoolManager.Instance.specialAdventureAdd = true;    // 특수 모험가 합류할 예정
                    var questResult = new QuestResultIndex(index);
                    UIManager.Instance.OpenUI<QuestResult>(questResult);
                }
            }
            else
            {
                PoolManager.Instance.specialAdventureAdd = false;    // 의뢰 실패로 모험가 도망
                var questResult = new QuestResultIndex(index);
                UIManager.Instance.OpenUI<QuestResult>(questResult);
            }
        }
        else
        {
            var questResult = new QuestResultIndex(index);
            UIManager.Instance.OpenUI<QuestResult>(questResult);
        }
        
    }

    private int CheckMaxAdventure()
    {
        int tmpCount = 0;
        string adventrueHave = PlayerPrefs.GetString("AdventureId");
        foreach (var i in adventrueHave.Split(","))
        {
            if (i != "")
                tmpCount++;
        }

        return tmpCount;
    }

    public void OnClickQuestStart(int index)    // 파견 시작 버튼
    {
        // if (GameInfo.gameInfo.Day % 5 == 0 && GameInfo.gameInfo.Timer < 120f)
        // {
        //     var uiData = new ConfirmUIData();
        //     uiData.confirmType = ConfirmType.OK_CANCEL;
        //     uiData.descTxt = "오늘은 무슨 일이\n일어날지 모릅니다...\n\n정말로 의뢰를\n수행하시겠습니까?";
        //     uiData.okBtnTxt = "네";
        //     uiData.cancelBtnTxt = "취소";
        //     uiData.onClickOKBtn = () => { QuestStart(index); };
        //     uiData.onClickCancelBtn = () => {
        //         OnClickAwakeBtn(index);
        //         return; 
        //     };
        //     UIManager.Instance.OpenUI<ConfirmUI>(uiData);
        // }
        if (questManagers.nomalRate < 100f && PlayerPrefs.GetInt("ReSelectConfirm") == 0)
        {
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK_CANCEL;
            uiData.descTxt = "성공 확률이\n100% 미만입니다.\n다시 선택하시겠습니까?";
            uiData.okBtnTxt = "재선택";
            uiData.cancelBtnTxt = "아니오";
            uiData.onClickOKBtn = () => { OnClickAwakeBtn(index); return; };
            uiData.onClickCancelBtn = () => { QuestStart(index); };
            UIManager.Instance.OpenUI<ConfirmUIVerson2>(uiData);
        }
        else
        {
            QuestStart(index);
        }
    }

    private void QuestStart(int index)
    {


        if (questManagers.adventureBtn.interactable)
        {
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK;
            uiData.descTxt = "모험가를 선택하세요.";
            uiData.okBtnTxt = "확인";
            UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            return;
        }

        if (questManagers.questBtn.interactable)
        {
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK;
            uiData.descTxt = "의뢰를 선택하세요.";
            uiData.okBtnTxt = "확인";
            UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            return;
        }

        

        image.SetActive(true);
        StartCoroutine(testt());

        gaugeImage.texture = Resources.Load("Arts/Guage/Empty") as Texture2D;   // 빈칸

        questStart = true;

        startDay = GameInfo.gameInfo.Day;
        endDay = startDay + PoolManager.Instance.questData[index].questTime; // 파견 결과 나오는 날...
        totalDay = PoolManager.Instance.questData[index].questTime;

        gaugeObject.SetActive(true);
        awakeBtnObject.SetActive(false);
    }

    private IEnumerator testt()
    {
        yield return new WaitForSeconds(1.5f);
    }

    public void OnClickAwakeBtn(int index)
    {
        // Quest Awake
        if (PoolManager.Instance.usingQuestList.Count != 0)
        {
            PoolManager.Instance.usingQuestList.Remove(PoolManager.Instance.questData[index].questId);
            QuestData.questSelectedId = 0;

            PoolManager.Instance.questData.Remove(index);

            PoolManager.Instance.resultList.Remove(index);  // 의뢰 결과 삭제

            PoolManager.Instance.questTxt[index - 1].text = "의뢰 선택";

            PoolManager.Instance.questBtn[index - 1].interactable = true;
        }

        // Adventure Awake
        AdventureAwake(index);
    }

    public void AdventureAwake(int index)
    {
        if (PoolManager.Instance.usingAdventureList.Count != 0)
        {
            // 의뢰 종료시 모험가 다시 사용하게 Test
            foreach (var i in questManagers.adventureDatas)
            {
                PoolManager.Instance.usingAdventureList.Remove(i.adventureId); // 파견 중이였던 걸 해제
            }


            AdventureData.adventureSelectId.Clear();

            questManagers.adventureDatas.Clear();

            PoolManager.Instance.adventureTxt[index - 1].text = "모험가 선택";

            PoolManager.Instance.adventureBtn[index - 1].interactable = true;
        }
    }

    public void GiveUpBtn(int index) // 1 ~ 5
    {
        var cData = new ConfirmUIData();
        cData.confirmType = ConfirmType.OK_CANCEL;
        cData.descTxt = "파견을 포기하겠습니까?";
        cData.okBtnTxt = "네";
        cData.cancelBtnTxt = "아니요";
        cData.onClickOKBtn = () =>
        {
            GiveUp(index);
        };
        UIManager.Instance.OpenUI<ConfirmUI>(cData);
    }

    private void GiveUp(int index)
    {
        //// 모험가 파견 상태 해제
        //foreach (var i in PoolManager.Instance.questManagers[index - 1].adventureDatas)
        //{
        //    PoolManager.Instance.usingAdventureList.Remove(i.adventureId); 
        //}

        //// 파견창에 맞는 모험가 데이터 삭제
        //PoolManager.Instance.questManagers[index - 1].adventureDatas.Clear();


        // 의뢰 제거
        int deleteId = PoolManager.Instance.questData[index].questId;

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


        OnClickAwakeBtn(index);

        // 각 파견창의 게이지 비활성화
        PoolManager.Instance.gaugeObject[index - 1].SetActive(false);

        // 초기화 버튼 활성화
        PoolManager.Instance.awakeBtn[index - 1].SetActive(true);

        // 결과 확인 버튼 비활성화
        PoolManager.Instance.resultBtn[index - 1].gameObject.SetActive(false);

        // 버튼 다시 활성화
        PoolManager.Instance.BtnActive(index);

        // 버튼 Text 다시 설정
        PoolManager.Instance.questTxt[index - 1].text = "의뢰 선택";
        PoolManager.Instance.adventureTxt[index - 1].text = "모험가 선택";

        image.SetActive(false);
    }
}


public class AdventureIndexClass : BaseUIData
{
    public int index;

    public AdventureIndexClass(int i)
    {
        index = i;
    }
}

public class QuestIndex : BaseUIData
{
    public int index;

    public QuestIndex(int i)
    {
        index = i;
    }
}

public class QuestResultIndex : BaseUIData
{
    public int index;

    public QuestResultIndex(int i)
    {
        index = i;
    }
}

public class AdventurerUIWithDesk : BaseUIData {
    public Desk desk;
    public AdventurerUIWithDesk(Desk d) {
        desk = d;
    }
}

public class StringInfo : BaseUIData {
    public string str;
    public StringInfo(string s) {
        str = s;
    }
}