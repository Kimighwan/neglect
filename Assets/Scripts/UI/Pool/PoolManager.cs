using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonBehaviour<PoolManager>
{
    public List<QuestData> userQuestList { get; private set; } = new List<QuestData>();
    public List<int> userQuestIndex = new List<int>();
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

    public void SetAdventureListData()
    {
        userAdventureList.Clear();

        var adventureId = PlayerPrefs.GetString("AdventureId");
        var adventureIds = adventureId.Split(',');

        if (adventureId == "") return;

        foreach(var item in adventureIds)
        {
            int adventureIdOfInt = Convert.ToInt32(item);
            var data = DataTableManager.Instance.GetAdventureData(adventureIdOfInt);

            userAdventureList.Add(data);
        }
    }

    public void DeleteQuestData()
    {
        var questId = PlayerPrefs.GetString("QuestId");
        var questIds = questId.Split(',');

        if (questId == "") return;
        if (QuestData.questSelectedId == 0) return;

        string addId = "";

        foreach (var item in questIds)
        {
            int questIdOfInt = Convert.ToInt32(item);

            if(questIdOfInt != QuestData.questSelectedId)
            {
                if(addId == "") 
                    addId += questIdOfInt.ToString();
                else
                    addId += "," + questIdOfInt.ToString();
            }
        }

        PlayerPrefs.SetString("QuestId", addId);
    }

    public void DeleteAdventureData()
    {
        var adventureId = PlayerPrefs.GetString("AdventureId");
        var adventureIds = adventureId.Split(',');

        if (adventureId == "") return;

        string addId = "";

        foreach (var item in adventureIds)
        {
            int adventureIdOfInt = Convert.ToInt32(item);

            if (!AdventureData.adventureSelectId.Contains(adventureIdOfInt))
            {
                if (addId == "")
                    addId += adventureIdOfInt.ToString();
                else
                    addId += "," + adventureIdOfInt.ToString();
            }
        }
        
        PlayerPrefs.SetString("AdventureId", addId);
    }
}
