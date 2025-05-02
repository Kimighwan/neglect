using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AdventureListSlotItemOverRay : MouseDrag
{
    public string pos = "";
    public string m_class = "";
    public string type = "";
    public string state = "";
    public string rank = "";

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        var uiData = new MouseOverRayUIData();
        uiData.str = pos + m_class + type + state + rank;
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
