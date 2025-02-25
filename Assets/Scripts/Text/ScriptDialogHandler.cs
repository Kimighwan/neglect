using System.Collections.Generic;
using UnityEngine;

public class ScriptDialogHandler : MonoBehaviour
{
    public static ScriptDialogHandler handler;
    public ScriptMode script;
    public DialogMode dialog;

    private GameInfo info;
    private int scriptIndex = 0;
    private List<int> startScript = new List<int> { 100001, 100016, 100101, 100201, 100301 };
    private List<int> endScript = new List<int> { 100015, 100032, 100120, 100219, 100315 };
    private List<int> scriptStartDay = new List<int> { 1, 1, 5, 10, 15}; // 8시에
    private List<bool> illExist = new List<bool> { true, false, false, false, false };

    private int dialogIndex = 0;
    private List<int> startDialog = new List<int> { 101911, 101921, 101931 };
    private List<int> endDialog = new List<int> { 101915, 101927, 101938 };
    private List<int> dialogStartDay = new List<int> { 4, 9, 14 }; // 9시에

    void Awake()
    {
        handler = this;
    }
    private void Start() {
        info = GameInfo.gameInfo;
    }
    private void Update() {
        if (info.Day >= scriptStartDay[scriptIndex] && info.Timer >= 80f && !script.isScriptMode) {
            PlayScript(startScript[scriptIndex], endScript[scriptIndex], illExist[scriptIndex]);
            // 스크립트 재생
            scriptIndex++;
        }
        if (info.Day >= dialogStartDay[dialogIndex] && info.Timer >= 90f) {
            PlayDialog(startDialog[dialogIndex], endDialog[dialogIndex]);
            // 대화 재생
            dialogIndex++;
        }
    }
    public void ConditionalScriptPlay(int q_id) {
        switch (q_id) {
        case 132901: // 1챕터
            PlayScript(100151, 100171, false);
            break;
        case 133902: // 2챕터
            PlayScript(100251, 100260, false);
            break;
        case 139999: // 3챕터
            PlayScript(100351, 100377, false);
            GameManager.gameManager.EndTheGame();
            break;
        }
    }
    public void ConditionalDialogPlay(int q_id) {
        switch (q_id) {
        case 132801: // 얼음 마녀 조사
            PlayDialog(101811, 101816);
            break;
        case 133801: // 호문클루스 연구시설 조사
            PlayDialog(101821, 101827);
            break;
        case 134801: // 새끼용 포획
            PlayDialog(101831, 101835);
            break;
        }
    }
    private void PlayScript(int s, int e, bool i) {
        script.PrepareScriptText(s, e, i);
        script.ShowNextScript();
        script.isScriptMode = true;
    }
    private void PlayDialog(int s, int e) {
        dialog.PrepareDialogText(s, e);
    }
}
