using UnityEngine;

public class EncyclopediaUI : BaseUI
{
    void OnEnable()
    {
        //AudioManager.Instance.PlaySFX(SFX.OpenBook);
    }
    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        var rectTransform = GetComponent<RectTransform>();

        rectTransform.localScale = new Vector2(1.2f, 1.2f);
    }

    public void OnClickMonsterBtn()
    {
        var mosterUI = new BaseUIData();
        AudioManager.Instance.PlaySFX(SFX.OpenBook);
        UIManager.Instance.CloseUI(this);
        UIManager.Instance.OpenUI<MonsterUI>(mosterUI);
    }

    public void OnClickSystemBtn()
    {
        var systemUI = new BaseUIData();
        AudioManager.Instance.PlaySFX(SFX.OpenBook);
        UIManager.Instance.CloseUI(this);
        UIManager.Instance.OpenUI<SystemUI>(systemUI);
    }
}
