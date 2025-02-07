using UnityEngine.EventSystems;

public class Desk : MouseDrag
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        UIManager.uIManager.OnClickAdventureTable();
    }
}
