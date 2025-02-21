using System.Collections.Generic;
using UnityEngine;

public class ScriptDialogHandler : MonoBehaviour
{
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
    private List<int> dialogStartDay = new List<int> { 5, 10, 15 }; // 9시에

    private void Start() {
        info = GameInfo.gameInfo;
    }
    private void Update() {
        if (info.Day >= scriptStartDay[scriptIndex] && info.Timer >= 80f && !script.isScriptMode) {
            script.PrepareScriptText(startScript[scriptIndex], endScript[scriptIndex], illExist[scriptIndex]);
            script.ShowNextScript();
            script.isScriptMode = true;
            // 스크립트 재생
            scriptIndex++;
        }
        if (info.Day >= dialogStartDay[dialogIndex] && info.Timer >= 90f) {
            dialog.PrepareDialogText(startDialog[dialogIndex], endDialog[dialogIndex]);
            // 대화 재생
            dialogIndex++;
        }
    }
}
