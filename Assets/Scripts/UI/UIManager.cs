using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonBehaviour<UIManager>
{
    public string advemtureDetailDescText;

    public int exportAdventureId;

    public Transform UICanvasTrs; // 컨버스 위치
    public Transform ClosedUITrs; // 비활성 UI 저장소 위치

    private BaseUI frontUI; // 최상단 UI
    private Dictionary<System.Type, GameObject> openUIPool = new Dictionary<System.Type, GameObject>(); // 활성화된 UI 저장소
    private Dictionary<System.Type, GameObject> closeUIPool = new Dictionary<System.Type, GameObject>(); // 비활성화된 UI 저장소

    void Start()
    {
        Fade.Instance.DoFade(Color.black, 1f, 0f, 1f, 0f, true, false);
        PlayerPrefs.GetString("AdventureId", "");
    }

    protected override void Init()
    {
        base.Init();
    }

    private BaseUI GetUI<T>(out bool isAlreadyOpen) // UI 인스턴스를 관리하며 원하는 UI 반환
    {
        System.Type uiType = typeof(T);

        BaseUI ui = null;
        isAlreadyOpen = false;

        if (openUIPool.ContainsKey(uiType)) // 이미 활성화 되었다면
        {
            ui = openUIPool[uiType].GetComponent<BaseUI>();
            isAlreadyOpen = true;
        }
        else if (closeUIPool.ContainsKey(uiType)) // 비활성화 되었다면
        {
            ui = closeUIPool[uiType].GetComponent<BaseUI>();
            closeUIPool.Remove(uiType);
        }
        else // 생성된 적이 없다면 생성해주기
        {
            var uiObj = Instantiate(Resources.Load($"UI/{uiType}", typeof(GameObject))) as GameObject;
            ui = uiObj.GetComponent<BaseUI>();
        }

        return ui;
    }

    public void OpenUI<T>(BaseUIData uiData)
    {
        System.Type uiType = typeof(T);

        bool isAlredyPone = false;
        var ui = GetUI<T>(out isAlredyPone);

        if (!ui)
        {
            return;
        }

        if (isAlredyPone)
        {
            return;
        }

        var siblingIndex = UICanvasTrs.childCount - 1;
        ui.Init(UICanvasTrs);
        ui.transform.SetSiblingIndex(siblingIndex);
        ui.gameObject.SetActive(true);
        ui.SetInfo(uiData);
        ui.ShowUI();

        frontUI = ui;
        openUIPool[uiType] = ui.gameObject;
    }

    public void CloseUI(BaseUI ui)
    {
        System.Type uiType = ui.GetType();

        ui.gameObject.SetActive(false);

        openUIPool.Remove(uiType);
        closeUIPool[uiType] = ui.gameObject;

        ui.transform.SetParent(ClosedUITrs);

        frontUI = null;
        var lastChild = UICanvasTrs.GetChild(UICanvasTrs.childCount - 3);
        if (lastChild)
        {
            frontUI = lastChild.gameObject.GetComponent<BaseUI>();
        }
    }

    public BaseUI GetActiveUI<T>() // 원하는 ui가 열렸으면 가져오고 아니면 null 반환
    {
        var uiType = typeof(T);
        return openUIPool.ContainsKey(uiType) ? openUIPool[uiType].GetComponent<BaseUI>() : null;
    }

    public bool ExistOpenUI() // 활서아화된 ui 있는지 확인
    {
        return frontUI != null;
    }

    public BaseUI GetCurrentFrontUI() // 최상단 UI 리턴
    {
        return frontUI;
    }

    public void CloseCurrentFrontUI() // 최상단 UI 닫기
    {
        frontUI.CloseUI();
    }

    public void CloseAllOpenUI() // 열려있는 모든 UI 닫기
    {
        while (frontUI)
        {
            frontUI.CloseUI(true);
        }
    }

    #region OnClickEvent

    public void OnClickEncyclopediaBtn()
    {
        var encyclopediaUI = new BaseUIData();
        UIManager.Instance.OpenUI<EncyclopediaUI>(encyclopediaUI);
    }

    public void OnClickAdventureTable()
    {
        var adventureUI = new BaseUIData();
        UIManager.Instance.OpenUI<AdventurerUI>(adventureUI);
    }

    public void OnClickCounter()
    {
        var receptionUI = new BaseUIData();
        UIManager.Instance.OpenUI<ReceptionUI>(receptionUI);
    }

    public void OnClickLevelUp() {
        var levelUpUI = new BaseUIData();
        UIManager.instance.OpenUI<LevelUpUI>(levelUpUI);
    }

    #endregion

    #region 파견창

    // 파견 창 관련해서 부드럽게 올라오게 하는 부분 
    public GameObject requestUpBtn; // 파견 Up버튼
    public GameObject requestDownBtn; // 파견 Down버튼
    public void UpRequest(GameObject obj) {
        Vector2 targetPos = new Vector2(0f, 0f);
        RectTransform panel = obj.GetComponent<RectTransform>();
        requestUpBtn.SetActive(false);
        requestDownBtn.SetActive(true);
        StartCoroutine(AnimateUI(panel, targetPos, 0.5f));
        
    }
    public void DownRequest(GameObject obj) {
        Vector2 targetPos = new Vector2(0f, -235f);
        RectTransform panel = obj.GetComponent<RectTransform>();
        requestUpBtn.SetActive(true);
        requestDownBtn.SetActive(false);
        StartCoroutine(AnimateUI(panel, targetPos, 0.5f));
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
    // 파견 창 관련해서 부드럽게 올라오게 하는 부분 

    #endregion

    #region Adventure



    #endregion

}
