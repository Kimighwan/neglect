
using UnityEngine;

public class QuestListUI : BaseUI
{
    public override void Init(Transform anchor)
    {
        base.Init(anchor);

        var rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector3(0f, -58f, 0f);
        rectTransform.sizeDelta = new Vector2(1176.5f, 967f);
    }

    public void OnClickBackOfQuestList()
    {
        UIManager.Instance.CloseUI(this);

        var receptionUI = new BaseUIData();
        UIManager.Instance.OpenUI<ReceptionUI>(receptionUI);
    }
}
