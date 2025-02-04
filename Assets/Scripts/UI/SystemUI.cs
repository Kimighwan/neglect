using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemUI : BaseUI
{
    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
    }

    public void OnClickAllUICloseBtn()
    {
        UIManager.Instance.CloseAllOpenUI();
    }

    public void OnClickMonsterBtn()
    {
        var mosterUI = new BaseUIData();
        CloseUI();
        UIManager.Instance.OpenUI<MonsterUI>(mosterUI);
    }
}
