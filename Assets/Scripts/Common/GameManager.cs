using UnityEngine;
using UnityEngine.SceneManagement;

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
        Invoke("PauseGame", 1f);
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
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(konamiCode[index]))
            {
                index++;

                // 모든 순서를 다 입력했다면
                if (index == konamiCode.Length)
                {
                    GameInfo.gameInfo.ChangeGold(999999);
                    index = 0;
                }
            }
            else
            {
                if (Input.GetKeyDown(konamiCode[0]))
                {
                    index = 1;
                }
                else
                {
                    index = 0;
                }
            }
        }
        if (Input.GetMouseButtonDown(0)) AudioManager.Instance.PlaySFX(SFX.Click2);
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
            info.gameSpeed = 15f;
            info.ChangeAniObjSpeed(10f);
            dialogMode.ChangeDialogSpeed(6f);
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
        Invoke("BackToTitle", 2f);
    }
    private void BackToTitle() {
        Fade.Instance.DoFade(Color.black, 0f, 1f, 1f, 0f, false, () =>
        {
            SceneManager.LoadScene(0);
            UIManager.Instance.CloseAllOpenUI();

            Fade.Instance.DoFade(Color.black, 1f, 0f, 1f, 1f, false, () =>
            {
            });
        });
    }
}
