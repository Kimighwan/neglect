using UnityEngine;

public class ReceptionUI : BaseUI
{
    public override void Init(Transform anchor)
    {
        base.Init(anchor);

        var rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector3(0f, -58f, 0f);
        rectTransform.sizeDelta = new Vector2(1176.5f, 967f);
    }

    public void OnClickAdventrueListBtn()
    {
        var adventurerListUI = new BaseUIData();
        UIManager.Instance.CloseUI(this);
        UIManager.Instance.OpenUI<AdventurerListUI>(adventurerListUI);
    }

    public void OnClickQuestListBtn()
    {
        var questListUI = new BaseUIData();
        UIManager.Instance.CloseUI(this);
        UIManager.Instance.OpenUI<QuestListUI>(questListUI);
    }

    public void OnClickNewQuestListBtn()
    {
        var questListUI = new BaseUIData();
        UIManager.Instance.CloseUI(this);
        UIManager.Instance.OpenUI<TodayQuestUI>(questListUI);
    }
}
