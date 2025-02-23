using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public ScriptMode scriptMode;
    public DialogMode dialogMode;
    private GameInfo info;
    private bool pause = true;
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

    void Update()
    {
        if (!pause && !scriptMode.isScriptMode) {
            info.UpdateGameInfo();
            UITextHandler.textHandler.UpdateTexts();
        }
    }

    public void PauseGame() {
        pause = !pause;
        info.gameSpeed = 1f;
        dialogMode.ChangeDialogSpeed(1f);
    }
    public void QuickGame() {
        info.gameSpeed = 50f;
        dialogMode.ChangeDialogSpeed(10f);
    }
    public bool IsPaused() {
        return pause;
    }
    public void EndTheGame() {
        
    }
}
