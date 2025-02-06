using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterDescUI : MonsterUI
{
    public MonsterDescData monsterDescData;

    private string monsterClass;
    private string monsterArea;
    private string monsterWeekness;
    private string monsterStrength;
    private string monsterDesc;

    public TextMeshProUGUI txtClass;
    public TextMeshProUGUI txtArea;
    public TextMeshProUGUI txtWeekness;
    public TextMeshProUGUI txtStrength;
    public TextMeshProUGUI txtDesc;

    private void Awake()
    {
        GetMonsterDescData();
        InitData();
    }

    private void GetMonsterDescData()
    {
        monsterDescData = DataTableManager.Instance.GetMonsterDescData(DataTableManager.Instance.monsterDescId);

        this.monsterClass = monsterDescData.monsterClass;
        this.monsterArea = monsterDescData.monsterArea;
        this.monsterWeekness = monsterDescData.monsterWeekness;
        this.monsterStrength = monsterDescData.monsterStrength;
        this.monsterDesc = monsterDescData.monsterDesc;
    }

    private void InitData()
    {
        txtClass.text = monsterClass + " 등급";
        txtArea.text = "서식지 : " + monsterArea;
        txtWeekness.text = "약점 : " + monsterWeekness;
        txtStrength.text = "강점 : " + monsterStrength;
        txtDesc.text = monsterDesc;
    }
}
