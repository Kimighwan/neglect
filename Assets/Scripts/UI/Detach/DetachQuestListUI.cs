using Gpm.Ui;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class DetachQuestListUI : BaseUI
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

    private void OnEnable()
    {
        PoolManager.Instance.SetQuestListData();
        SetScroll();
    }

    private void SetScroll()
    {
        infiniteScrollList.Clear();

        foreach (var questData in PoolManager.Instance.userQuestList)
        {
            var slotData = new QuestData();

            slotData.questId = questData.questId;
            slotData.questName = questData.questName;
            slotData.questLevel = questData.questLevel;
            slotData.questTime = questData.questTime;
            slotData.questReward = questData.questReward;
            slotData.questMonsterDescId = questData.questMonsterDescId;
            slotData.questMonster = questData.questMonster;

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
                    if (questOrderType == QuestOrderType.DOWN)
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
}
