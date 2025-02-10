using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : SingletonBehaviour<Pool>
{
    public List<QuestData> userQuestList {  get; private set; } = new List<QuestData>();

    protected override void Init()
    {
        base.Init();
    }

    public void SetQuestListData()
    {
        userQuestList.Clear();

        var questId = PlayerPrefs.GetString("QuestId");
        var questIds = questId.Split(',');

        foreach(var item in questIds)
        {
            int questIdOfInt = Convert.ToInt32(item);
            var data = DataTableManager.Instance.GetQuestData(questIdOfInt);

            userQuestList.Add(data);
        }
    }
}
