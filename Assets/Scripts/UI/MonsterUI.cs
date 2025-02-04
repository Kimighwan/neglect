using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterUI : BaseUI
{

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
    }

    public void OnClickAllUICloseBtn()
    {
        UIManager.Instance.CloseAllOpenUI();
    }

    public void OnClickSystemBtn()
    {
        var systemUI = new BaseUIData();
        CloseUI();
        UIManager.Instance.OpenUI<SystemUI>(systemUI);
    }
}
