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
        UIManager.Instance.CloseUI(this);
    }

    public void OnClickSystemBtn()
    {
        var systemUI = new BaseUIData();
        UIManager.Instance.OpenUI<SystemUI>(systemUI);
        UIManager.Instance.CloseUI(this);
    }
}
