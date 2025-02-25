using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform cameraTransform;
    public static GameManager gameManager;
    public ScriptMode scriptMode;
    public DialogMode dialogMode;
    private GameInfo info;
    private bool pause = true;
    public bool Pause { get { return pause; } set { } }
    private bool fastMode = false;
    public bool FastMode { get { return fastMode; } set { } }
    void Awake()
    {
        gameManager = this;
    }
    void Start()
    {
        info = GameInfo.gameInfo;
        info.StartGameInfo();
        info.PrepareShowIll(2f, 0f, true);
        Invoke("PauseGame", 1.5f);
    }
    void OnEnable()
    {
        AudioManager.Instance.PlayBGM(BGM.Main6);
    }


    // 코나미 코드에 해당하는 키 시퀀스
    private KeyCode[] konamiCode = new KeyCode[]
    {
        KeyCode.UpArrow,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.B,
        KeyCode.A
    };
    private int index = 0;
    void Update()
    {
        if (!pause && !scriptMode.isScriptMode) {
            info.UpdateGameInfo();
            UITextHandler.textHandler.UpdateTexts();
        }
        // 현재 프레임에 어떤 키가 눌렸는지 확인
        if (Input.anyKeyDown)
        {
            // 입력된 키가 현재 기대하는 코나미 코드와 일치하는지 체크
            if (Input.GetKeyDown(konamiCode[index]))
            {
                index++;

                // 모든 순서를 다 입력했다면
                if (index == konamiCode.Length)
                {
                    GameInfo.gameInfo.ChangeGold(999999);
                    // 여기서 원하는 동작을 실행
                    // 예: 특수 이벤트 실행, 이펙트 재생 등

                    // 인덱스 리셋
                    index = 0;
                }
            }
            else
            {
                // 만약 틀린 키가 눌렸다면, 
                // 만약 그 키가 첫번째 키라면 새롭게 시작 (UpArrow)
                if (Input.GetKeyDown(konamiCode[0]))
                {
                    index = 1;
                }
                else
                {
                    // 올바른 순서가 아니면 인덱스를 초기화
                    index = 0;
                }
            }
        }
    }

    public void PauseGame() {
        pause = !pause;
        if (pause) {
            info.pauseButton.gameObject.GetComponent<UIPressed>().defaultImg = info.pauseAndGo[2];
            info.pauseButton.gameObject.GetComponent<UIPressed>().pressedImg = info.pauseAndGo[3];
            info.pauseButton.sprite = info.pauseAndGo[2];
            info.ChangeAniObjSpeed(0f);
        } else {
            info.pauseButton.gameObject.GetComponent<UIPressed>().defaultImg = info.pauseAndGo[0];
            info.pauseButton.gameObject.GetComponent<UIPressed>().pressedImg = info.pauseAndGo[1];
            info.pauseButton.sprite = info.pauseAndGo[0];
            info.ChangeAniObjSpeed(1f);
        }
        fastMode = false;
        info.fastButton.sprite = info.pauseAndGo[4];
        info.gameSpeed = 1f;
        dialogMode.ChangeDialogSpeed(1f);
    }
    public void QuickGame() {
        fastMode = !fastMode;
        if (fastMode) {
            info.fastButton.sprite = info.pauseAndGo[5];
            info.gameSpeed = 20f;
            info.ChangeAniObjSpeed(10f);
            dialogMode.ChangeDialogSpeed(10f);
        }
        else {
            info.fastButton.sprite = info.pauseAndGo[4];
            info.gameSpeed = 1f;
            dialogMode.ChangeDialogSpeed(1f);
            info.ChangeAniObjSpeed(1f);
        }
    }
    public void OpenTutorial(int id) {
        DataTableManager.Instance.systemDescId = id;
        var systemDescUI = new BaseUIData();
        UIManager.Instance.OpenUI<SystemDescUI>(systemDescUI);
    }
    public void EndTheGame() {
        
    }
}
