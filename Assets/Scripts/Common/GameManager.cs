using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    private bool endGame = false;
    void Awake()
    {
        gameManager = this;
    }
    void Start()
    {
        AudioManager.Instance.BgmVol = AudioManager.Instance.BgmVol * 10.0f;
        AudioManager.Instance.UpdateVolume();
        info = GameInfo.gameInfo;
        info.StartGameInfo();
        info.PrepareShowIll(2f, 0f, true);
        Invoke("PauseGame", 1f);
    }
    void OnEnable()
    {
        //PlayerPrefs.DeleteAll();
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
        }

        UITextHandler.textHandler.UpdateTexts();
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

        if (Input.GetMouseButtonDown(0)) {
            AudioManager.Instance.PlaySFX(SFX.Click2);
            if (endGame) Invoke("GoToTitle", 2f);
        }
    }

    public void PauseGame() {
        pause = !pause;
        if (pause) {
            SpriteState spriteState = info.pauseButton.gameObject.GetComponent<Button>().spriteState;
            spriteState.pressedSprite = info.pauseAndGo[3];
            info.pauseButton.gameObject.GetComponent<Button>().spriteState = spriteState;
            info.pauseButton.sprite = info.pauseAndGo[2];
            info.ChangeAniObjSpeed(0f);
        } else {
            SpriteState spriteState = info.pauseButton.gameObject.GetComponent<Button>().spriteState;
            spriteState.pressedSprite = info.pauseAndGo[1];
            info.pauseButton.gameObject.GetComponent<Button>().spriteState = spriteState;
            info.pauseButton.sprite = info.pauseAndGo[0];
            info.ChangeAniObjSpeed(1f);
        }
        fastMode = false;
        info.fastButton.sprite = info.pauseAndGo[4];
        info.gameSpeed = 1f;
        dialogMode.ChangeDialogSpeed(1f);
    }
    public void QuickGame() {
        if (pause)
        {
            PauseGame();
        }

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
        ScriptDialogObjectData data = ScriptDialogObjectData.data;
        data.background.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Arts/Illustration/ED1");
        Fade.Instance.DoFade(Color.black, 1f, 0f, 2f, 0f, false);
        data.background.SetActive(true);
        Invoke("ReadyForEndGame", 2f);
    }
    private void ReadyForEndGame() {
        endGame = true;
    }
    public void GoToTitle() {
        UIManager.Instance.CloseAllOpenUI();
        AudioManager.Instance.StopBGM();
        Fade.Instance.DoFade(Color.black, 0f, 1f, 1f, 0f, false, () =>
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

            // 혹시 몰라 팝업창이 안 보이면 걍 종료되도록...
            Application.Quit();

            // 타이틀로 가기 버그 수정하기에 시간이 부족하여 위 코드로 대체
            //SceneManager.LoadScene(0);

            //Fade.Instance.DoFade(Color.black, 1f, 0f, 1f, 1f, false, () =>
            //{
            //});
        });
    }
}
