public class SystemUI : BaseUI
{
    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
    }

    public void OnClickAllUICloseBtn()
    {
        AudioManager.Instance.PlaySFX(SFX.CloseBook);
        UIManager.Instance.CloseAllOpenUI();
    }

    public void OnClickMonsterBtn()
    {
        var mosterUI = new BaseUIData();
        AudioManager.Instance.PlaySFX(SFX.OpenBook);
        CloseUI();
        UIManager.Instance.OpenUI<MonsterUI>(mosterUI);
    }

    public void OnClickSystemDescBtn(int id)
    {
        DataTableManager.Instance.systemDescId = id;
        var systemDescUI = new BaseUIData();
        AudioManager.Instance.PlaySFX(SFX.BookFlip3);
        UIManager.Instance.CloseUI(this);
        UIManager.Instance.OpenUI<SystemDescUI>(systemDescUI);
    }

    public void BackBtn()
    {
        AudioManager.Instance.PlaySFX(SFX.CloseBook);
        UIManager.Instance.CloseUI(this);

        var encyclopediaUI = new BaseUIData();
        UIManager.Instance.OpenUI<EncyclopediaUI>(encyclopediaUI);
    }

    public override void OnClickCloseButton()
    {
        AudioManager.Instance.PlaySFX(SFX.CloseBook);
        base.OnClickCloseButton();
    }
}
