using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPressedAndOn : MouseDrag
{
    private bool onUI = false;
    public Sprite defaultImg;
    public Sprite onImg;
    public Sprite pressedImg;

    private void OnEnable()
    {
        this.GetComponent<Image>().sprite = defaultImg;
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        onUI = true;
        this.GetComponent<Image>().sprite = onImg;
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        onUI = false;
        this.GetComponent<Image>().sprite = defaultImg;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        this.GetComponent<Image>().sprite = pressedImg;
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onUI) this.GetComponent<Image>().sprite = onImg;
        else this.GetComponent<Image>().sprite = defaultImg;
    }
}
