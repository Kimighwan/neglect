using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PoolManager : SingletonBehaviour<PoolManager>
{
    public List<QuestData> userQuestList { get; private set; } = new List<QuestData>();

    public List<int> userQuestIndex = new List<int>();      // 랜덤 의뢰 선택에서 중복 의뢰 보여주지 않기 위한 List
    public List<AdventureData> userAdventureList { get; private set; } = new List<AdventureData> { };

    public List<int> userAdventureIndex = new List<int>();  // 랜덤 모험가 선택에서 중복 모험가를 보여주지 않기 위한 List

    public List<int> usingQuestList = new List<int>();      // 현재 파견 중인 의뢰

    public List<int> usingAdventureList = new List<int>();  // 현재 파견 중인 모험가

    protected override void Init()
    {
        base.Init();
    }

    public void SetDetachQuestListData()
    {
        userQuestList.Clear();

        var questId = PlayerPrefs.GetString("QuestId");
        var questIds = questId.Split(',');

        if (questId == "") return;

        foreach (var item in questIds)
        {
            int questIdOfInt = Convert.ToInt32(item);
            if (usingQuestList.Contains(questIdOfInt)) continue;

            var data = DataTableManager.Instance.GetQuestData(questIdOfInt);

            userQuestList.Add(data);
        }
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

    public void SetDetachAdventureListData()
    {
        userAdventureList.Clear();

        var adventureId = PlayerPrefs.GetString("AdventureId");
        var adventureIds = adventureId.Split(',');

        var usingId = PlayerPrefs.GetString("UsingAdventure");
        var usingIds = usingId.Split(",");

        if (adventureId == "") return;

        foreach(var item in adventureIds)
        {
            int adventureIdOfInt = Convert.ToInt32(item);
            if (usingAdventureList.Contains(adventureIdOfInt)) continue;

            var data = DataTableManager.Instance.GetAdventureData(adventureIdOfInt);

            userAdventureList.Add(data);
        }
    }

    public void SetAdventureListData()
    {
        userAdventureList.Clear();

        var adventureId = PlayerPrefs.GetString("AdventureId");
        var adventureIds = adventureId.Split(',');

        if (adventureId == "") return;

        foreach (var item in adventureIds)
        {
            int adventureIdOfInt = Convert.ToInt32(item);
            var data = DataTableManager.Instance.GetAdventureData(adventureIdOfInt);

            userAdventureList.Add(data);
        }
    }

    public void UsingQuestData()   // 선택된 의뢰 사용중이라고 표현
    {
        usingQuestList.Add(QuestData.questSelectedId);

        //if (PlayerPrefs.GetString("UsingQuest") == "")
        //    PlayerPrefs.SetString("UsingQuest", QuestData.questSelectedId.ToString());  // 사용중인 의뢰 체크
        //else
        //{
        //    var tmp = PlayerPrefs.GetString("UsingQuest");
        //    PlayerPrefs.SetString("UsingQuest", tmp + "," + QuestData.questSelectedId.ToString());
        //}

        //var questId = PlayerPrefs.GetString("QuestId");
        //var questIds = questId.Split(',');

        //if (questId == "") return;
        //if (QuestData.questSelectedId == 0) return;

        //string addId = "";

        //foreach (var item in questIds)
        //{
        //    int questIdOfInt = Convert.ToInt32(item);

        //    if(questIdOfInt != QuestData.questSelectedId)
        //    {
        //        if(addId == "") 
        //            addId += questIdOfInt.ToString();
        //        else
        //            addId += "," + questIdOfInt.ToString();
        //    }
        //}

        //PlayerPrefs.SetString("QuestId", addId);
    }

    public void UsingAdventureData()   // 선택된 모험가 사용중이라고 표현
    {
        foreach(int i in AdventureData.adventureSelectId)
        {
            usingAdventureList.Add(i);
        }

        //string add = "";
        //foreach (var item in AdventureData.adventureSelectId)
        //{
        //    if (add == "")
        //    {
        //        add += item.ToString();
        //    }
        //    else
        //    {
        //        add += "," + item.ToString();
        //    }
        //}

        //if (PlayerPrefs.GetString("UsingAdventure") == "")
        //    PlayerPrefs.SetString("UsingAdventure", add);  // 사용중인 모험가 체크
        //else
        //{
        //    var tmp = PlayerPrefs.GetString("UsingAdventure");
        //    PlayerPrefs.SetString("UsingAdventure", tmp + "," + add);
        //}

        //var adventureId = PlayerPrefs.GetString("AdventureId");
        //var adventureIds = adventureId.Split(',');

        //if (adventureId == "") return;

        //string addId = "";

        //foreach (var item in adventureIds)
        //{
        //    int adventureIdOfInt = Convert.ToInt32(item);

        //    if (!AdventureData.adventureSelectId.Contains(adventureIdOfInt))
        //    {
        //        if (addId == "")
        //            addId += adventureIdOfInt.ToString();
        //        else
        //            addId += "," + adventureIdOfInt.ToString();
        //    }
        //}

        //PlayerPrefs.SetString("AdventureId", addId);
    }
}
