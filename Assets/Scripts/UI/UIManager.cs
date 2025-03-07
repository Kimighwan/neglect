using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonBehaviour<UIManager>
{
    public string advemtureDetailDescText;

    public int exportAdventureId;

    public Transform UICanvasTrs;       // 컨버스 위치
    public Transform ClosedUITrs;       // 비활성 UI 저장소 위치

    public Transform fadeCanvasTrs;     // 검정 화면 위에 표시하기 위한 위치


    private BaseUI frontUI; // 최상단 UI
    private Dictionary<System.Type, GameObject> openUIPool = new Dictionary<System.Type, GameObject>(); // 활성화된 UI 저장소
    private Dictionary<System.Type, GameObject> closeUIPool = new Dictionary<System.Type, GameObject>(); // 비활성화된 UI 저장소

    void Start()
    {
        Fade.Instance.DoFade(Color.black, 1f, 0f, 1f, 0f, true);
        PlayerPrefs.GetString("AdventureId", "");
    }

    private void Update()
    {
        HandleInput();

        if (frontUI == null)
        {
            PoolManager.instance.isNotTouchUI = false;

            if (PoolManager.Instance.checkHavesSpecialQuest != 0)
            {
                // 특수 의뢰를 받아서 해당 의뢰와 관련된 스크립트를 UI가 안 띄워져 있을 때 재생
                ScriptDialogHandler.handler.ConditionalDialogPlay(PoolManager.Instance.checkHavesSpecialQuest);

                // 혹시 모를 재생되면서 바로 UI 창을 킨다면 닫아버리기
                CloseAllOpenUI();

                // 체크용 변수 다시 초기화
                PoolManager.Instance.checkHavesSpecialQuest = 0;
            }
        }
        else
        {
            PoolManager.instance.isNotTouchUI = true;
        }
    }

    protected override void Init()
    {
        base.Init();
        fadeCanvasTrs = GameObject.FindGameObjectWithTag("Fade").transform;
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
        GameInfo.gameInfo.LockRoomClick();
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
        var lastChild = UICanvasTrs.GetChild(UICanvasTrs.childCount - 2);
        if (lastChild)
        {
            frontUI = lastChild.gameObject.GetComponent<BaseUI>();
        }

        if (!ExistOpenUI()) Invoke("UnLockRoomClick", 0.01f);
    }
    private void UnLockRoomClick() { // 객실 클릭 활성화
        GameInfo.gameInfo.UnLockRoomClick();
    }

    public BaseUI GetActiveUI<T>() // 원하는 ui가 열렸으면 가져오고 아니면 null 반환
    {
        var uiType = typeof(T);
        return openUIPool.ContainsKey(uiType) ? openUIPool[uiType].GetComponent<BaseUI>() : null;
    }

    public bool ExistOpenUI() // 활성화된 ui 있는지 확인
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
    public void OpenSimpleInfoUI(string str) {
        var simpleInfoUI = new StringInfo(str);
        UIManager.Instance.OpenUI<SimpleInfoUI>(simpleInfoUI);
    }

    #region OnClickEvent

    public void OnClickEncyclopediaBtn()
    {
        var encyclopediaUI = new BaseUIData();
        UIManager.Instance.OpenUI<EncyclopediaUI>(encyclopediaUI);
    }
    public void OnClickAdventureTable(Desk desk)
    {
        var adventureUI = new AdventurerUIWithDesk(desk);
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
    public void OnClickEndToday() {
        var EndToday = new BaseUIData();
        UIManager.instance.OpenUI<ReportUI>(EndToday);
    }
    public void OnClickGameSetting() {
        var settingUI = new BaseUIData();
        UIManager.Instance.OpenUI<GameSettingUI>(settingUI);
    }

    #endregion

    #region Adventure



    #endregion


    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.X))
        {
            // AudioManager.Instance.PlaySFX(SFX.ui_button_click);

            if(GetActiveUI<AdventurerUI>())
                GameManager.gameManager.cameraTransform.position = new Vector3(0f, 0f, -10f);

            if (frontUI != null) // UI가 띄워져 있다면
            {
                if (GetActiveUI<ReportUI>())
                {
                    return;
                }

                if (GetActiveUI<DetachAdventureListUI>())
                {
                    var ui = GetActiveUI<DetachAdventureListUI>() as DetachAdventureListUI;
                    if(ui.adventureIndex < 10) // 일반 의뢰 진행 중
                        PoolManager.Instance.questManagers[ui.adventureIndex - 1].adventureDatas.Clear();
                    else
                    {
                        var tmp = UIManager.Instance.GetActiveUI<EmergencyQuestUI>() as EmergencyQuestUI;
                        tmp.GetComponent<QuestManager>().adventureDatas.Clear();
                    }
                }
                frontUI.CloseUI(); // 띄워져있는 UI 닫기
            }
            else // 아무 UI도 없다면 게임 종료 팝업UI 띄우기
            {
                var uiData = new ConfirmUIData();
                uiData.confirmType = ConfirmType.OK_CANCEL;
                uiData.descTxt = "종료하시겠습니까?";
                uiData.okBtnTxt = "종료";
                uiData.cancelBtnTxt = "취소";
                uiData.onClickOKBtn = () =>
                {
                    Application.Quit();
                };
                UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            }
        }
    }

}
