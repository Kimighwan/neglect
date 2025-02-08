using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SystemDescUI : SystemUI
{
    public SystemDescData systemDescData;

    private string script1;
    private string script2;
    private string script3;
    private string script4;

    public TextMeshProUGUI txtDesc;

    private void Awake()
    {
        GetSystemDescData();
        InitData();
    }

    private void GetSystemDescData()    // 데이터 가져오기
    {
        systemDescData = DataTableManager.Instance.GetSystemDescData(DataTableManager.Instance.systemDescId);

        this.script1 = systemDescData.systemScript1;
        this.script2 = systemDescData.systemScript2;
        this.script3 = systemDescData.systemScript3;
        this.script4 = systemDescData.systemScript4;
    }

    private void InitData() // 가져온 데이터로 초기화
    {
        txtDesc.text = script1;
    }

    public void BackBtn()   // 뒤로가기 버튼
    {
        UIManager.Instance.CloseUI(this);

        var systemUI = new BaseUIData();
        UIManager.Instance.OpenUI<SystemUI>(systemUI);
    }

    public void OnClickSetScriptOfPage(int page)
    {
        if(page == 1)
        {
            txtDesc.text = script1;
        }
        else if(page == 2)
        {
            txtDesc.text = script2;
        }
        else if (page == 3)
        {
            txtDesc.text = script3;
        }
        else if ( page == 4)
        {
            txtDesc.text = script4;
        }
    }
}
