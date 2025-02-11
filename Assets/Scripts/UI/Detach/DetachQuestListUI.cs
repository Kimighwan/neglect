using Gpm.Ui;
using System.Collections.Generic;
using UnityEngine;

public class DetachQuestListUI : BaseUI
{
    public InfiniteScroll infiniteScrollList;

    public override void Init(Transform anchor)
    {
        base.Init(anchor);
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
}
