using Gpm.Ui;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
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
    public TextMeshProUGUI countText;


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

        infiniteScrollList.layout.padding = new Vector2(16f, 700f);
    }

    private void OnEnable()
    {
        PoolManager.Instance.SetQuestListData();
        SetScroll();
        SetCountText();
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
                        orderBtnText.text = "DOWN";
                        compareResult = ((itemB.questId / 1000) % 10).CompareTo((itemA.questId / 1000) % 10);
                    }

                    // 오름차순
                    if (questOrderType == QuestOrderType.UP)
                    {
                        orderBtnText.text = "UP";
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
                break;
            case QuestOrderType.UP:
                questOrderType = QuestOrderType.DOWN;
                break;
            default:
                break;
        }

        SortQuest();
    }

    private int CheckCurrentQuestCount()
    {
        int tmp = 0;

        var a = PlayerPrefs.GetString("QuestId");
        foreach(var item in a.Split(','))
        {
            if (item != "")
                tmp++;
        }

        return tmp;
    }

    private int CheckMaxQuestCount()
    {
        int tmp = 0;

        if (GameInfo.gameInfo.Level == 1)
            tmp = 6;
        else if (GameInfo.gameInfo.Level == 2)
            tmp = 8;
        else if (GameInfo.gameInfo.Level == 3)
            tmp = 10;
        else if (GameInfo.gameInfo.Level == 4)
            tmp = 12;
        else if (GameInfo.gameInfo.Level == 5)
            tmp = 14;

        return tmp;
    }

    private void SetCountText()
    {
        countText.text = CheckCurrentQuestCount().ToString() + "/" + CheckMaxQuestCount().ToString();
    }
}
