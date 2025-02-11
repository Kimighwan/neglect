using Gpm.Ui;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    private const string DATA_PATH = "DataTable";

    public int monsterDescId;
    public int systemDescId;
    public int questDetailId;

    public string page;

    protected override void Init()
    {
        base.Init();

        LoadMonsterDescDataTable();
        LoadSystemDescDataTable();
        LoadAdventureDataTable();
        LoadQuestDataTable();
    }

    #region Monster_Desc

    private const string MONSTER_DATA_TABLE = "dm_list";
    private List<MonsterDescData> MonsterDescDataTable = new List<MonsterDescData>();

    private void LoadMonsterDescDataTable()
    {
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{MONSTER_DATA_TABLE}");

        foreach (var data in parsedDataTable)
        {
            var monsterDescData = new MonsterDescData
            {
                monsterId = Convert.ToInt32(data["dm_id"]),
                monsterName = data["name"].ToString(),
                monsterTier = data["tier"].ToString(),
                monsterWeekness = data["weak"].ToString(),
                monsterStrength = data["strong"].ToString(),
                monsterDesc = data["script1"].ToString(),
            };

            MonsterDescDataTable.Add(monsterDescData);
        }
    }

    public MonsterDescData GetMonsterDescData(int monsterId)
    {
        return MonsterDescDataTable.Where(item => item.monsterId == monsterId).FirstOrDefault();
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
        // 시스템 ID에 맞는 정보들을 반환
        // 만약 데이터가 존재 하지 않는다면 null 반환
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
                questMonster = data["object"].ToString(),
                questReward = Convert.ToInt32(data["reward"]),
                questTime = Convert.ToInt32(data["day"]),
                questMonsterDescId = Convert.ToInt32(data["object_id"]),
            };

            QuestDataTable.Add(questData);
        }
    }

    public QuestData GetQuestData(int questId)
    {
        return QuestDataTable.Where(item => item.questId == questId).FirstOrDefault();
        // 시스템 ID에 맞는 정보들을 반환
        // 만약 데이터가 존재 하지 않는다면 null 반환
    }

    #endregion
}

public class MonsterDescData : BaseUIData
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
}

public class AdventureData : InfiniteScrollData // BaseUIData
{
    public int adventureId;
    public string adventureName;
    public string adventurePosition;
    public string adventureClass;
    public string adventureType;
    public string adventureTier;
}

public class QuestData : InfiniteScrollData //BaseUIData
{
    public int questId;
    public int questTime;
    public int questReward;
    public int questMonsterDescId;

    public string questName;
    public string questLevel;
    public string questMonster;

    public static int questSelectedId;
}