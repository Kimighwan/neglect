using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    private const string DATA_PATH = "DataTable";

    public int monsterDescId;
    public int systemDescId;

    protected override void Init()
    {
        base.Init();

        LoadMonsterDescDataTable();
        LoadSystemDescDataTable();
        LoadAdventureDataTable();
    }

    #region Monster_Desc

    private const string MONSTER_DATA_TABLE = "Test3";
    private List<MonsterDescData> MonsterDescDataTable = new List<MonsterDescData>();

    private void LoadMonsterDescDataTable()
    {
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{MONSTER_DATA_TABLE}");

        foreach (var data in parsedDataTable)
        {
            var monsterDescData = new MonsterDescData
            {
                monsterId = Convert.ToInt32(data["monster_id"]),
                monsterClass = data["class"].ToString(),
                monsterArea = data["area"].ToString(),
                monsterWeekness = data["weekness"].ToString(),
                monsterStrength = data["strength"].ToString(),
                monsterDesc = data["desc"].ToString(),
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

    private const string SYSTEM_DATA_TABLE = "SystemTest";
    private List<SystemDescData> SystemDescDataTable = new List<SystemDescData>();

    private void LoadSystemDescDataTable()
    {
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{SYSTEM_DATA_TABLE}");

        foreach (var data in parsedDataTable)
        {
            var systemDescData = new SystemDescData
            {
                systemId = Convert.ToInt32(data["system_id"]),
                systemDesc = data["desc"].ToString(),
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
}

public class MonsterDescData : BaseUIData
{
    public int monsterId;
    public string monsterClass;
    public string monsterArea;
    public string monsterWeekness;
    public string monsterStrength;
    public string monsterDesc;
}

public class SystemDescData : BaseUIData
{
    public int systemId;
    public string systemDesc;
}

public class AdventureData : BaseUIData
{
    public int adventureId;
    public string adventureName;
    public string adventurePosition;
    public string adventureClass;
    public string adventureType;
    public string adventureTier;
}