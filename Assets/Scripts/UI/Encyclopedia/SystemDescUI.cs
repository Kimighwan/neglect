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

    private void GetSystemDescData()    // 데이터 가져오기
    {
        systemDescData = DataTableManager.Instance.GetSystemDescData(DataTableManager.Instance.systemDescId);

        this.monsterDesc = systemDescData.systemDesc;
    }

    private void InitData() // 가져온 데이터로 초기화
    {
        txtDesc.text = monsterDesc;
    }

    public void BackBtn()   // 뒤로가기 버튼
    {
        UIManager.Instance.CloseUI(this);

        var systemUI = new BaseUIData();
        UIManager.Instance.OpenUI<SystemUI>(systemUI);
    }
}
