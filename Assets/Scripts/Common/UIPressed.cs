using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPressed : MouseDrag
{
    public Sprite defaultImg;
    public Sprite pressedImg;
    public override void OnPointerDown(PointerEventData eventData)
    {
        this.GetComponent<Image>().sprite = pressedImg;
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        this.GetComponent<Image>().sprite = defaultImg;
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        this.GetComponent<Image>().sprite = defaultImg;
    }
}
