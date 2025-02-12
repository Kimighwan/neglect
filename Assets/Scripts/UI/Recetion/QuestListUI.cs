using Gpm.Ui;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum QuestSortType
{
    Level,
}

public enum QuestOrderType
{
    DOWN,
    UP,
}

public class QuestListUI : BaseUI
{
    public InfiniteScroll infiniteScrollList;

    public TextMeshProUGUI sortBtnText;
    public TextMeshProUGUI orderBtnText;


    // private List<int> questId = new List<int>(); // 스크롤 쓰기 전 변수
    private QuestSortType questSortType = QuestSortType.Level;
    private QuestOrderType questOrderType = QuestOrderType.DOWN;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        SortQuest();
    }

    public override void Init(Transform anchor)
    {
        base.Init(anchor);

        var rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector3(0f, -58f, 0f);
        rectTransform.sizeDelta = new Vector2(1176.5f, 967f);
    }

    private void OnEnable()
    {
        PoolManager.Instance.SetQuestListData();
        SetScroll();
        // CheckMyQuest();
    }

    public void OnClickBackOfQuestList()    // 뒤로가기
    {
        UIManager.Instance.CloseUI(this);

        var receptionUI = new BaseUIData();
        UIManager.Instance.OpenUI<ReceptionUI>(receptionUI);
    }

    private void SetScroll()
    {
        infiniteScrollList.Clear();

        
        foreach(var questData in PoolManager.Instance.userQuestList)
        {
            var slotData = new QuestData();

            slotData.questId = questData.questId;
            slotData.questName = questData.questName;
            slotData.questLevel = questData.questLevel;
            slotData.questReward = questData.questReward;
            slotData.questTime = questData.questTime;
            slotData.questMonster = questData.questMonster;
            slotData.questMonsterDescId = questData.questMonsterDescId;

            infiniteScrollList.InsertData(slotData);
        }
    }

    private void SortQuest()
    {
        switch (questSortType)
        {
            case QuestSortType.Level:
                sortBtnText.text = "LEVEL";

                infiniteScrollList.SortDataList((a, b) =>
                {
                    int compareResult = 0;

                    var itemA = a.data as QuestData;
                    var itemB = b.data as QuestData;

                    // 내림차순
                    if(questOrderType == QuestOrderType.DOWN)
                    {
                        compareResult = ((itemB.questId / 1000) % 10).CompareTo((itemA.questId / 1000) % 10);
                    }

                    // 오름차순
                    if (questOrderType == QuestOrderType.UP)
                    {
                        compareResult = ((itemA.questId / 1000) % 10).CompareTo((itemB.questId / 1000) % 10);
                    }

                    if (compareResult == 0)
                    {
                        var itemAId = itemA.questId;

                        var itemBId = itemB.questId;

                        compareResult = itemAId.CompareTo((itemBId));
                    }

                    return compareResult;
                });
                break;
            default:
                break;
        }
    }

    // 정렬 버튼
    public void OnClickSortBtn()
    {
        switch (questSortType)
        {
            case QuestSortType.Level:
                questSortType = QuestSortType.Level;
                break;
            default: 
                break;
        }

        SortQuest();
    }

    // 순서 선택 버튼
    public void OnClickOrderBtn()
    {
        switch (questOrderType)
        {
            case QuestOrderType.DOWN:
                questOrderType = QuestOrderType.UP;
                orderBtnText.text = "UP";
                break;
            case QuestOrderType.UP:
                questOrderType = QuestOrderType.DOWN;
                orderBtnText.text = "DOWN";
                break;
            default:
                break;
        }

        SortQuest();
    }


    // 스크롤 쓰기전 함수
    //private void CheckMyQuest() // 가지고 있는 의뢰 체크
    //{
    //    questId.Clear();

    //    string myQuestOfString = PlayerPrefs.GetString("QuestId");
    //    string[] myQuestOfstrings = myQuestOfString.Split(',');

    //    if (myQuestOfString == "") return;

    //    foreach (string str in myQuestOfstrings)
    //    {
    //        questId.Add(Convert.ToInt32(str));
    //        InstantiateQuestList(Convert.ToInt32(str));
    //    }
    //}

    //private void InstantiateQuestList(int id)   // 의뢰 UI 인스턴스화
    //{
    //    var item = Instantiate(Resources.Load("UI/QuestSelectedUI") as GameObject);
    //    item.transform.SetParent(pos);

    //    //item.GetComponent<QuestSelectedUI>().questId = id;
    //}
}
