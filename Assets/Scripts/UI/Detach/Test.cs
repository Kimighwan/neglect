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
    public GameObject image;
    public GameObject resultBtn;

    public GameObject gaugeObject;
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

            
            if(totalDay == 1)           // 하루 짜리 의뢰
            {
                if (GameInfo.gameInfo.Day == endDay)    // 꽉참
                {
                    gaugeImage.texture = Resources.Load("Arts/Guage/Pull") as Texture2D;
                    questStart = false;
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

    public void OnClickQuestStart(int index)
    {
        image.SetActive(true);
        StartCoroutine(testt());

        questStart = true;

        startDay = GameInfo.gameInfo.Day;
        endDay = startDay + PoolManager.Instance.questData[index].questTime; // 파견 결과 나오는 날...
        totalDay = PoolManager.Instance.questData[index].questTime; 
        
        if (totalDay != 0)   // 스토리 의뢰가 아니면 게이지 이미지 활성화
        {
            gaugeObject.SetActive(true);
        }
    }

    private IEnumerator testt()
    {
        yield return new WaitForSeconds(1.5f);
        image.SetActive(false);
        resultBtn.SetActive(true);
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
            PoolManager.Instance.usingAdventureList.Clear();

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