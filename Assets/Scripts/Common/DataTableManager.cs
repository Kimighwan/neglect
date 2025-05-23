using Gpm.Ui;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    private const string DATA_PATH = "DataTable";

    public int monsterDescId;
    public int systemDescId;
    public int questDetailId;

    public string page;

    protected override void Init()
    {
        isDestroyOnLoad = true;

        base.Init();

        LoadMonsterDataTable();
        LoadSystemDescDataTable();
        LoadAdventureDataTable();
        LoadQuestDataTable();
        LoadScriptDataTable();
    }

    #region Monster_Desc

    private const string MONSTER_DATA_TABLE = "mob_list";
    private List<MonsterData> MonsterDataTable = new List<MonsterData>();

    private void LoadMonsterDataTable()
    {
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{MONSTER_DATA_TABLE}");

        foreach (var data in parsedDataTable)
        {
            var monsterDescData = new MonsterData
            {
                monsterId = Convert.ToInt32(data["mob_id"]),
                monsterName = data["name"].ToString(),
                monsterTier = data["tier"].ToString(),
                monsterWeekness = data["weakness"].ToString(),
                monsterStrength = data["resist"].ToString(),
                monsterDesc = data["script1"].ToString(),
            };

            MonsterDataTable.Add(monsterDescData);
        }
    }

    public MonsterData GetMonsterData(int monsterId)
    {
        return MonsterDataTable.Where(item => item.monsterId == monsterId).FirstOrDefault();
        // 몬스터 ID에 맞는 정보들을 반환
        // 만약 데이터가 존재 하지 않는다면 null 반환
    }

    #endregion

    #region System_Desc

    private const string SYSTEM_DATA_TABLE = "ds_list";
    private List<SystemDescData> SystemDescDataTable = new List<SystemDescData>();

    private void LoadSystemDescDataTable()
    {
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{SYSTEM_DATA_TABLE}");

        foreach (var data in parsedDataTable)
        {
            var systemDescData = new SystemDescData
            {
                systemId = Convert.ToInt32(data["ds_id"]),
                systemName = data["name"].ToString(),
                systemScript1 = data["script1"].ToString(),
                systemScript2 = data["script2"].ToString(),
                systemScript3 = data["script3"].ToString(),
                systemScript4 = data["script4"].ToString(),
                systemScript5 = data["script5"].ToString(),
                endPage = Convert.ToInt32(data["count"]),
                page1 = data["page1"].ToString(),
                page2 = data["page2"].ToString(),
                page3 = data["page3"].ToString(),
                page4 = data["page4"].ToString(),
                page5 = data["page5"].ToString(),
            };

            SystemDescDataTable.Add(systemDescData);
        }
    }

    public SystemDescData GetSystemDescData(int systemId)
    {
        return SystemDescDataTable.Where(item => item.systemId == systemId).FirstOrDefault();
        // 시스템 ID에 맞는 정보들을 반환
        // 만약 데이터가 존재 하지 않는다면 null 반환
    }

    #endregion

    #region Adventure

    private const string ADVENTURE_DATA_TABLE = "char_list";
    private List<AdventureData> AdventureDataTable = new List<AdventureData>();

    private void LoadAdventureDataTable()
    {
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{ADVENTURE_DATA_TABLE}");

        foreach (var data in parsedDataTable)
        {
            var adventureData = new AdventureData
            {
                adventureId = Convert.ToInt32(data["char_id"]),
                adventureIndex = Convert.ToInt32(data["index"]),
                adventureSkil = Convert.ToInt32(data["skill"]),
                adventureName = data["name"].ToString(),
                adventureTier = data["tier"].ToString(),
                adventurePosition = data["position"].ToString(),
                adventureClass = data["class"].ToString(),
                adventureType = data["type"].ToString(),
            };

            AdventureDataTable.Add(adventureData);
        }
    }

    public AdventureData GetAdventureData(int adventureId)
    {
        return AdventureDataTable.Where(item => item.adventureId == adventureId).FirstOrDefault();
    }

    public AdventureData GetRandomAdventureData(int index)
    {
        return AdventureDataTable.Where(item => item.adventureIndex == index).FirstOrDefault();
    }

    #endregion

    #region Quest

    private const string QUEST_DATA_TABLE = "quest_list";
    private List<QuestData> QuestDataTable = new List<QuestData>();

    private void LoadQuestDataTable()
    {
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{QUEST_DATA_TABLE}");

        foreach (var data in parsedDataTable)
        {
            var questData = new QuestData
            {
                questId = Convert.ToInt32(data["quest_id"]),
                questName = data["name"].ToString(),
                questLevel = data["tier"].ToString(),
                questActive = Convert.ToInt32(data["active"]),
                questMonsterDescId = Convert.ToInt32(data["object_id"]),
                questMonster = data["object"].ToString(),
                questReward = Convert.ToInt32(data["reward"]),
                questTime = Convert.ToInt32(data["day"]),
                questIndex = Convert.ToInt32(data["index"]),
                questScript = data["script"].ToString(),
            };

            QuestDataTable.Add(questData);
        }
    }

    public QuestData GetQuestData(int questId)
    {
        return QuestDataTable.Where(item => item.questId == questId).FirstOrDefault();
    }

    public QuestData GetQuestDataUsingIndex(int questIndex)
    {
        return QuestDataTable.Where(item => item.questIndex == questIndex).FirstOrDefault();
    }

    #endregion

    #region Script

    private const string Script_DATA_TABLE = "scr_list";
    private List<ScriptData> ScriptDataTable = new List<ScriptData>();
    private void LoadScriptDataTable()
    {
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{Script_DATA_TABLE}");

        foreach (var data in parsedDataTable)
        {
            var scriptData = new ScriptData
            {
                scriptId = Convert.ToInt32(data["scr_id"]),
                scriptSpeaker = data["char"].ToString(),
                scriptLeftPos = data["left"].ToString(),
                scriptMiddlePos = data["middle"].ToString(),
                scriptRightPos = data["right"].ToString(),
                scriptLine = data["script"].ToString(),
                scriptIll = data["ill"].ToString(),
            };

            ScriptDataTable.Add(scriptData);
        }
    }
    public ScriptData GetScriptData(int scriptId)
    {
        return ScriptDataTable.Where(item => item.scriptId == scriptId).FirstOrDefault();
        // 시스템 ID에 맞는 정보들을 반환
        // 만약 데이터가 존재 하지 않는다면 null 반환
    }

    #endregion
}

public class MonsterData : BaseUIData
{
    public int monsterId;
    public string monsterName;
    public string monsterTier;
    public string monsterWeekness;
    public string monsterStrength;
    public string monsterDesc;
}

public class SystemDescData : BaseUIData
{
    public int systemId;
    public string systemName;
    public string systemScript1;
    public string systemScript2;
    public string systemScript3;
    public string systemScript4;
    public string systemScript5;
    public int endPage;
    public string page1;
    public string page2;
    public string page3;
    public string page4;
    public string page5;
}

public class AdventureData : InfiniteScrollData // BaseUIData
{
    public int adventureId;
    public int adventureIndex;
    public int adventureSkil;
    public string adventureName;
    public string adventurePosition;
    public string adventureClass;
    public string adventureType;
    public string adventureTier;

    public static List<int> adventureSelectId = new List<int>();

}

public class QuestData : InfiniteScrollData // BaseUIData
{
    public int questId;
    public int questTime;
    public int questReward;
    public int questMonsterDescId;
    public int questIndex;
    public int questActive;

    public string questName;
    public string questLevel;
    public string questMonster;
    public string questScript;

    public static int questSelectedId;
}

public class ScriptData : BaseUIData {
    public int scriptId;
    public string scriptSpeaker;
    public string scriptLeftPos;
    public string scriptMiddlePos;
    public string scriptRightPos;
    public string scriptLine;
    public string scriptIll;
}