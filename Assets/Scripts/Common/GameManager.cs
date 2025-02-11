using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ScriptMode scriptMode;
    public DialogMode dialogMode;
    private GameInfo info;
    private bool pause = false;
    private bool once = false;
    void Start()
    {
        info = GameInfo.gameInfo;
        info.StartGameInfo();
    }

    void Update()
    {
        if (!pause && !scriptMode.isScriptMode) {
            info.UpdateGameInfo();
            TextHandler.textHandler.UpdateTexts();
        }
        if (info.Day == 2 && info.Timer > 8f && !once) {
            dialogMode.PrepareDialogText(101811, 101816, true);
            once = true;
        }
    }

    public void PauseGame() {
        pause = !pause;
        info.gameSpeed = 1f;
    }
    public void QuickGame() {
        info.gameSpeed = 5f;
    }
}
