using UnityEngine.EventSystems;

public class Counter : MouseDrag
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        UIManager.Instance.OnClickCounter();
    }

    
}
