using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

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
    public RectTransform txt1;        // NPC 대화창 1
    public RectTransform txt2;        // NPC 대화창 2

    private Vector3 originalCamPos; // 원래 위치
    private Vector3 targetCamPos; // 목표 위치

    private Vector3 originalUIPos; // UI 원래 위치
    private Vector3 targetUIPos;

    private Vector3 originalTxt1Pos;
    private Vector3 originalTxt2Pos;
    private Vector3 targetTxt1Pos;
    private Vector3 targetTxt2Pos;

    private bool tutorialOnce = false;
    private bool isMoved = false; // 이동 여부 체크
    private bool inRequest = false;

    void Start()
    {
        originalCamPos = cameraTransform.position;
        targetCamPos = originalCamPos + new Vector3(0, -5, 0);

        originalUIPos = requestBG.anchoredPosition;
        targetUIPos = originalUIPos + new Vector3(0, 320, 0);

        originalTxt1Pos = txt1.anchoredPosition;
        targetTxt1Pos = originalTxt1Pos + new Vector3(0, 336.225f, 0);

        originalTxt2Pos = txt2.anchoredPosition;
        targetTxt2Pos = originalTxt2Pos + new Vector3(0, 336.225f, 0);
    }

    public void OnClickBut()
    {
        inRequest = isMoved ? false : true;
        this.GetComponent<Image>().raycastTarget = false;
        StartCoroutine(MoveCameraAndUI(isMoved ? originalCamPos : targetCamPos, isMoved ? originalUIPos : targetUIPos, isMoved ? originalTxt1Pos : targetTxt1Pos, isMoved ? originalTxt2Pos : targetTxt2Pos));
        isMoved = !isMoved;
        if (!tutorialOnce) {
            tutorialOnce = true;
            GameManager.gameManager.OpenTutorial(590006);
        }
    }
    public bool GetInRequest() {
        return inRequest;
    }

    private IEnumerator MoveCameraAndUI(Vector3 camDestination, Vector3 uiDestination, Vector3 txt1Des, Vector3 txt2Des)
    {
        
        float elapsedTime = 0f;
        float duration = 0.6f; // 이동 시간 (1초)
        Vector3 camStartPos = cameraTransform.position;
        Vector3 uiStartPos = requestBG.anchoredPosition;
        Vector3 txt1StartPos = txt1.anchoredPosition;
        Vector3 txt2StartPos = txt2.anchoredPosition;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            t = t * t * (3f - 2f * t); // **SmoothStep 방식** (Ease In-Out)

            cameraTransform.position = Vector3.Lerp(camStartPos, camDestination, t);
            requestBG.anchoredPosition = Vector3.Lerp(uiStartPos, uiDestination, t);
            txt1.anchoredPosition = Vector3.Lerp(txt1StartPos, txt1Des, t);
            txt2.anchoredPosition = Vector3.Lerp(txt2StartPos, txt2Des, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 정확한 위치 보정
        cameraTransform.position = camDestination;
        requestBG.anchoredPosition = uiDestination;
        txt1.anchoredPosition = txt1Des;
        txt2.anchoredPosition = txt2Des;
        this.GetComponent<Image>().raycastTarget = true;
    }
}
