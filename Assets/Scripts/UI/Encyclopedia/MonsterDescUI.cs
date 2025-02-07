using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterDescUI : MonsterUI
{
    public MonsterDescData monsterDescData;

    private string monsterName;
    private string monsterTier;
    private string monsterWeekness;
    private string monsterStrength;
    private string monsterDesc;

    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtTier;
    public TextMeshProUGUI txtWeekness;
    public TextMeshProUGUI txtStrength;
    public TextMeshProUGUI txtDesc;

    private void Awake()
    {
        GetMonsterDescData();
        InitData();
    }

    private void OnEnable()
    {
        GetMonsterDescData();
        InitData();
    }

    private void GetMonsterDescData()
    {
        monsterDescData = DataTableManager.Instance.GetMonsterDescData(DataTableManager.Instance.monsterDescId);

        this.monsterName = monsterDescData.monsterName;
        this.monsterTier = monsterDescData.monsterTier;
        this.monsterWeekness = monsterDescData.monsterWeekness;
        this.monsterStrength = monsterDescData.monsterStrength;
        this.monsterDesc = monsterDescData.monsterDesc;
    }

    private void InitData()
    {
        txtName.text = monsterName;
        txtTier.text = monsterTier + " 등급";
        txtWeekness.text = "약점 : " + monsterWeekness;
        txtStrength.text = "강점 : " + monsterStrength;
        txtDesc.text = monsterDesc;
    }

    public void BackBtn()
    {
        UIManager.Instance.CloseUI(this);

        var monsterUI = new BaseUIData();
        UIManager.Instance.OpenUI<MonsterUI>(monsterUI);
    }
}
