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

    public void OnClickSystemDescBtn(int id)
    {
        DataTableManager.Instance.systemDescId = id;
        var systemDescUI = new BaseUIData();
        UIManager.Instance.CloseUI(this);
        UIManager.Instance.OpenUI<SystemDescUI>(systemDescUI);
    }

    public void BackBtn()
    {
        UIManager.Instance.CloseUI(this);

        var encyclopediaUI = new BaseUIData();
        UIManager.Instance.OpenUI<EncyclopediaUI>(encyclopediaUI);
    }
}
