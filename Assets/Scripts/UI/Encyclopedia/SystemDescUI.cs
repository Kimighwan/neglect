using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SystemDescUI : SystemUI
{
    public SystemDescData systemDescData;

    private string monsterDesc;

    public TextMeshProUGUI txtDesc;

    private void Awake()
    {
        GetSystemDescData();
        InitData();
    }

    private void GetSystemDescData()
    {
        systemDescData = DataTableManager.Instance.GetSystemDescData(DataTableManager.Instance.systemDescId);

        this.monsterDesc = systemDescData.systemDesc;
    }

    private void InitData()
    {
        txtDesc.text = monsterDesc;
    }

    public void BackBtn()
    {
        UIManager.Instance.CloseUI(this);

        var systemUI = new BaseUIData();
        UIManager.Instance.OpenUI<SystemUI>(systemUI);
    }
}
