using UnityEngine;
using UnityEngine.EventSystems;

public class Desk : MouseDrag
{
    private bool tutorialOnce = false;
    void Start()
    {
        original = this.GetComponent<SpriteRenderer>().sprite;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        UIManager.Instance.OnClickAdventureTable(this);
        if (!tutorialOnce) {
            tutorialOnce = true;
            GameManager.gameManager.OpenTutorial(590004);
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
