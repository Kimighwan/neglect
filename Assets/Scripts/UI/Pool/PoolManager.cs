using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonBehaviour<PoolManager>
{
    public List<QuestData> userQuestList { get; private set; } = new List<QuestData>();
    public List<AdventureData> userAdventureList { get; private set; } = new List<AdventureData> { };

    protected override void Init()
    {
        base.Init();
    }

    public void SetQuestListData()
    {
        userQuestList.Clear();

        var questId = PlayerPrefs.GetString("QuestId");
        var questIds = questId.Split(',');

        if (questId == "") return;

        foreach (var item in questIds)
        {
            int questIdOfInt = Convert.ToInt32(item);
            var data = DataTableManager.Instance.GetQuestData(questIdOfInt);

            userQuestList.Add(data);
        }
    }
}
