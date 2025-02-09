using UnityEngine;

public class QuestDetailUI : BaseUI
{
    public override void Init(Transform anchor)
    {
        base.Init(anchor);

        var rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector3(0f, -58f, 0f);
        rectTransform.sizeDelta = new Vector2(1176.5f, 967f);
    }

    public void OnClickBackOfQuestDetailList()
    {
        UIManager.Instance.CloseUI(this);

        var questListUI = new BaseUIData();
        UIManager.Instance.OpenUI<QuestListUI>(questListUI);
    }
}
