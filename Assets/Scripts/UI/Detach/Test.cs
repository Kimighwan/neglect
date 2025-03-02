using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public QuestManager questManagers;

    private bool adTutorialOnce = false;
    public GameObject image;                // 파견 진행 중 가린막 이미지
    public GameObject resultBtn;

    public GameObject gaugeObject;
    public GameObject awakeBtnObject;
    public RawImage gaugeImage;

    private int startDay;
    private int endDay;
    private int totalDay;

    private bool questStart;        // 파견 시작 : true

    private void Update()
    {
        if (questStart)
        {
            gaugeImage.texture = Resources.Load("Arts/Guage/Empty") as Texture2D;   // 빈칸

            if(totalDay == 0)
            {
                Debug.Log("0일 짜리 퀘스트 바로 종료");
                gaugeImage.texture = Resources.Load("Arts/Guage/Pull") as Texture2D;
                questStart = false;
                image.SetActive(false);
                resultBtn.SetActive(true);
            }
            else if(totalDay == 1)           // 하루 짜리 의뢰
            {
                if (GameInfo.gameInfo.Day == endDay)    // 꽉참
                {
                    gaugeImage.texture = Resources.Load("Arts/Guage/Pull") as Texture2D;
                    questStart = false;
                    image.SetActive(false);
                    resultBtn.SetActive(true);
                }
            }
            else if (totalDay == 2)     // 이틀 짜리 의뢰
            {
                if (GameInfo.gameInfo.Day == startDay + 1)    // OneOfTwo
                {
                    gaugeImage.texture = Resources.Load("Arts/Guage/OneOfTwo") as Texture2D;
                }
                else if (GameInfo.gameInfo.Day == endDay)    // 꽉참
                {
                    gaugeImage.texture = Resources.Load("Arts/Guage/Pull") as Texture2D;
                    questStart = false;
                    image.SetActive(false);
                    resultBtn.SetActive(true);
                }
            }
            else if (totalDay == 3)     // 삼일 짜리 의뢰
            {
                if (GameInfo.gameInfo.Day == startDay + 1)    // TwoOfTree
                {
                    gaugeImage.texture = Resources.Load("Arts/Guage/TwoOfTree") as Texture2D;
                }
                else if (GameInfo.gameInfo.Day == startDay + 2)    // OneOfThree
                {
                    gaugeImage.texture = Resources.Load("Arts/Guage/OneOfThree") as Texture2D;
                }
                else if (GameInfo.gameInfo.Day == endDay)    // 꽉참
                {
                    gaugeImage.texture = Resources.Load("Arts/Guage/Pull") as Texture2D;
                    questStart = false;
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
            GameManager.gameManager.OpenTutorial(590007);
        }
    }
    public void SetAdTutorialOnceTrue() {
        adTutorialOnce = true;
    }

    public void OnClickQuestResultTestBtn(int index)    // 결과 창 띄우기
    {
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
        if(questManagers.adventureBtn.interactable)
        {
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK;
            uiData.descTxt = "모험가를 선택하세요.";
            uiData.okBtnTxt = "확인";
            UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            return;
        }

        if(questManagers.questBtn.interactable)
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

            PoolManager.Instance.questTxt[index - 1].text = "의뢰 선택";
        }

        // Adventure Awake
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
        } 
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