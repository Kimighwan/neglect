using UnityEngine;
using UnityEngine.EventSystems;

public class Counter : MouseDrag
{
    private bool tutorialOnce = false;
    void Start()
    {
        original = this.GetComponent<SpriteRenderer>().sprite;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        UIManager.Instance.OnClickCounter();
        if (!tutorialOnce) {
            tutorialOnce = true;
            GameManager.gameManager.OpenTutorial(590002);
        }
    }

    private Sprite original;
    public Sprite whiteLine;
    void OnMouseOver()
    {
        if (UIManager.Instance.ExistOpenUI() || GameInfo.gameInfo.CheckInRequest()) return;
        this.GetComponent<SpriteRenderer>().sprite = whiteLine;
    }
    void OnMouseExit()
    {
        this.GetComponent<SpriteRenderer>().sprite = original;
    }
}
