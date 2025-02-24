using UnityEngine;
using UnityEngine.EventSystems;

public class Counter : MouseDrag
{
    private bool tutorialOnce = false;
    public override void OnPointerDown(PointerEventData eventData)
    {
        UIManager.Instance.OnClickCounter();
        if (!tutorialOnce) {
            tutorialOnce = true;
            GameManager.gameManager.OpenTutorial(590002);
        }
    }
}
