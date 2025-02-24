using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPressed : MouseDrag
{
    public Sprite defaultImg;
    public Sprite pressedImg;
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (this.GetComponent<Image>() != null) this.GetComponent<Image>().sprite = pressedImg;
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (this.GetComponent<Image>() != null) this.GetComponent<Image>().sprite = defaultImg;
    }
}
