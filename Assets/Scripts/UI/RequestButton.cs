using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class RequestButton : MouseDrag
{
    public Animator anim;       // 삼각형 Animator

    public override void OnPointerDown(PointerEventData eventData)
    {
        OnClickBut();
    }

    public Transform cameraTransform; // 이동할 카메라
    public RectTransform requestBG;
    public GameObject backimage;        // 검은색 뒷 배경
    public RectTransform txt1;        // NPC 대화창 1
    public RectTransform txt2;        // NPC 대화창 2

    private Vector3 originalCamPos; // 원래 위치
    private Vector3 targetCamPos; // 목표 위치

    private Vector3 originalUIPos; // UI 원래 위치
    private Vector3 targetUIPos;

    // private bool tutorialOnce = false;
    private bool isMoved = false; // 이동 여부 체크
    private bool isMoving = false;
    private bool inRequest = false;

    void Start()
    {
        originalCamPos = cameraTransform.position;
        targetCamPos = originalCamPos + new Vector3(0, -5, 0);

        originalUIPos = requestBG.anchoredPosition;
        targetUIPos = originalUIPos + new Vector3(0, 320, 0);
    }

    public void OnClickBut()
    {
        if (isMoving) return;
        if (PoolManager.Instance.isNotTouch) return;
        if (PoolManager.Instance.isNotTouchUI) return;
        if (PoolManager.Instance.isNotTutorialTouch) return;
        isMoving = true;

        inRequest = isMoved ? false : true;

        // 검은 색 뒷 배경
        backimage.SetActive(!isMoved);

        // 삼격형 애니메이션
        anim.SetBool("isDown", !isMoved);

        StartCoroutine(MoveCameraAndUI(isMoved ? originalCamPos : targetCamPos, isMoved ? originalUIPos : targetUIPos));
        isMoved = !isMoved;
        // if (!tutorialOnce) {
        //     tutorialOnce = true;
        //     GameManager.gameManager.OpenTutorial(590006);
        // }
        return;
    }
    public bool GetInRequest() {
        return inRequest;
    }

    private IEnumerator MoveCameraAndUI(Vector3 camDestination, Vector3 uiDestination)
    {
        float elapsedTime = 0f;
        float duration = 0.6f; // 이동 시간 (1초)
        Vector3 camStartPos = cameraTransform.position;
        Vector3 uiStartPos = requestBG.anchoredPosition;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            t = t * t * (3f - 2f * t); // **SmoothStep 방식** (Ease In-Out)

            cameraTransform.position = Vector3.Lerp(camStartPos, camDestination, t);
            requestBG.anchoredPosition = Vector3.Lerp(uiStartPos, uiDestination, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 정확한 위치 보정
        cameraTransform.position = camDestination;
        requestBG.anchoredPosition = uiDestination;
        isMoving = false;
    }

    public bool GetIsMoved() {
        return isMoved;
    }
}
