using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncyclopediaUI : BaseUI
{
    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);


    }

    public void OnClickMonsterBtn()
    {
        var mosterUI = new BaseUIData();
        UIManager.Instance.OpenUI<MonsterUI>(mosterUI);
    }

    public void OnClickSystemBtn()
    {
        var systemUI = new BaseUIData();
        UIManager.Instance.OpenUI<SystemUI>(systemUI);
    }
}
