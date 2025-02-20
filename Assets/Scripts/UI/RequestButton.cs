using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class RequestButton : MouseDrag
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        OnClickBut();
    }
    public Transform cameraTransform; // 이동할 카메라
    public RectTransform requestBG;
    public float moveSpeed = 3f; // 이동 속도

    private Vector3 originalCamPos; // 원래 위치
    private Vector3 targetCamPos; // 목표 위치
    private Vector3 originalUIPos; // UI 원래 위치
    private Vector3 targetUIPos;

    private bool isMoved = false; // 이동 여부 체크

    protected override void Start()
    {
        originalCamPos = cameraTransform.position;
        targetCamPos = originalCamPos + new Vector3(0, -5, 0);

        originalUIPos = requestBG.anchoredPosition;
        targetUIPos = originalUIPos + new Vector3(0, 320, 0);
    }

    public void OnClickBut()
    {
        StopAllCoroutines(); // 기존 이동 중이라면 중단
        StartCoroutine(MoveCameraAndUI(isMoved ? originalCamPos : targetCamPos, isMoved ? originalUIPos : targetUIPos));
        isMoved = !isMoved;
    }

    IEnumerator MoveCameraAndUI(Vector3 camDestination, Vector3 uiDestination)
    {
        float elapsedTime = 0f;
        Vector3 camStartPos = cameraTransform.position;
        Vector3 uiStartPos = requestBG.anchoredPosition;

        while (elapsedTime < 1f) // 1초 동안 이동
        {
            cameraTransform.position = Vector3.Lerp(camStartPos, camDestination, elapsedTime);
            requestBG.anchoredPosition = Vector3.Lerp(uiStartPos, uiDestination, elapsedTime);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        // 정확한 위치로 설정
        cameraTransform.position = camDestination;
        requestBG.anchoredPosition = uiDestination;
    }
}
