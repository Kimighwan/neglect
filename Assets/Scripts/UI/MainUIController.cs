using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour
{
    public GameObject encyclopedia;
    public GameObject blackImage;

    public Transform UICanvasTrs;   // Canvas 위치 닫힌 UI
    public Transform OpenUITrs;     // 열린 UI

    public GameObject requestUpBtn; // 파견 Up버튼
    public GameObject requestDownBtn; // 파견 Down버튼


    private GameObject frontUI;

    void Start()
    {
        Fade.Instance.DoFade(Color.black, 1f, 0f, 1f, 0f, true, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }

    public void OnClickEncyclopediaBtn()
    {
        encyclopedia.SetActive(true);
        OpenUI();
    }

    private void OpenUI()
    {
        SetBlackImage(true);
        transform.SetParent(OpenUITrs);
    }

    private void ClosedUI()
    {
        SetBlackImage(false);
        transform.SetParent(UICanvasTrs);
    }

    private void SetBlackImage(bool active)
    {
        if (active)
        {
            blackImage.SetActive(true);
        }
        else
        {
            blackImage.SetActive(false);
        }
    }
    public void UpRequest(GameObject obj) {
        Vector2 targetPos = new Vector2(0f, 0f);
        RectTransform panel = obj.GetComponent<RectTransform>();
        requestUpBtn.SetActive(false);
        requestDownBtn.SetActive(true);
        StartCoroutine(AnimateUI(panel, targetPos, 1f));
        
    }
    public void DownRequest(GameObject obj) {
        Vector2 targetPos = new Vector2(0f, -230f);
        RectTransform panel = obj.GetComponent<RectTransform>();
        requestUpBtn.SetActive(true);
        requestDownBtn.SetActive(false);
        StartCoroutine(AnimateUI(panel, targetPos, 1f));
    }

    IEnumerator AnimateUI(RectTransform panel, Vector2 newPos, float time)
    {
        float elapsedTime = 0f;
        Vector2 startPos = panel.anchoredPosition;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / time;
            t = t * t * (3f - 2f * t); // SmootherStep (부드러운 감속 효과)
            panel.anchoredPosition = Vector2.Lerp(startPos, newPos, t);
            yield return null;
        }
        // 애니메이션이 끝난 후, 정확한 최종값 설정
        panel.anchoredPosition = newPos;
    }
}