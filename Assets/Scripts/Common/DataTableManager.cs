using System;
using System.Collections.Generic;
using System.Linq;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    private const string DATA_PATH = "DataTable";
    public int monsterDescId;

    protected override void Init()
    {
        base.Init();

        LoadMonsterDescDataTable();
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