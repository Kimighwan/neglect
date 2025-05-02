using UnityEngine;
using UnityEngine.EventSystems;

public class QuestListItemOverRay : MouseDrag
{
    public string rank = "";

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        var uiData = new MouseOverRayUIData();
        uiData.str = rank;
        UIManager.Instance.OpenUI<MouseOverRayUI>(uiData);

        Debug.Log("오버레이 띄우기");
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        UIManager.Instance.CloseUI(UIManager.Instance.GetActiveUI<MouseOverRayUI>());
        Debug.Log("오버레이 닫기");
    }
}
