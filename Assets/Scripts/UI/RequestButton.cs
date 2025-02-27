using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RequestButton : MouseDrag
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        backimage.SetActive(!backimage.activeSelf);
        OnClickBut();
    }
    public Transform cameraTransform; // 이동할 카메라
    public RectTransform requestBG;
    public GameObject backimage;        // 검은색 뒷 배경

    private Vector3 originalCamPos; // 원래 위치
    private Vector3 targetCamPos; // 목표 위치
    private Vector3 originalUIPos; // UI 원래 위치
    private Vector3 targetUIPos;
    private bool tutorialOnce = false;
    private bool isMoved = false; // 이동 여부 체크
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
        inRequest = isMoved ? false : true;
        this.GetComponent<Image>().raycastTarget = false;
        StartCoroutine(MoveCameraAndUI(isMoved ? originalCamPos : targetCamPos, isMoved ? originalUIPos : targetUIPos));
        isMoved = !isMoved;
        if (!tutorialOnce) {
            tutorialOnce = true;
            GameManager.gameManager.OpenTutorial(590006);
        }
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
        this.GetComponent<Image>().raycastTarget = true;
    }
}
