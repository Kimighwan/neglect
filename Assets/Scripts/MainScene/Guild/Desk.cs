using UnityEngine;
// using System.Collections;
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
    // public Transform cameraTransform; // 이동할 카메라
    // public float moveSpeed = 2f; // 이동 속도

    // private Vector3 originalCamPos; // 원래 위치
    // private Vector3 targetCamPos; // 목표 위치


    //private bool isMoved = false; // 이동 여부 체크

    // void Start()
    // {
    //     originalCamPos = cameraTransform.position;
    //     targetCamPos = originalCamPos + new Vector3(-12, 0, 0);
    // }

    // public void OnClickBut()
    // {
    //     this.GetComponent<BoxCollider2D>().enabled = false;
    //     StartCoroutine(MoveCamera(isMoved ? originalCamPos : targetCamPos));
    //     this.GetComponent<BoxCollider2D>().enabled = true;
    // }

    // private IEnumerator MoveCamera(Vector3 camDestination)
    // {
    //     float elapsedTime = 0f;
    //     float duration = 0.8f; // 이동 시간 (1초)
    //     Vector3 camStartPos = cameraTransform.position;

    //     while (elapsedTime < duration)
    //     {
    //         float t = elapsedTime / duration;
    //         t = t * t * (3f - 2f * t); // **SmoothStep 방식** (Ease In-Out)

    //         cameraTransform.position = Vector3.Lerp(camStartPos, camDestination, t);

    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }

    //     // 정확한 위치 보정
    //     cameraTransform.position = camDestination;
    //     this.GetComponent<BoxCollider2D>().enabled = true;
    // }
}
