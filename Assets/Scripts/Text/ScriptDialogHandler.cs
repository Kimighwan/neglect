using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptDialogHandler : MonoBehaviour
{
    public static ScriptDialogHandler handler;
    public ScriptMode script;
    public DialogMode dialog;

    private GameInfo info;
    private int scriptIndex = 0;
    private List<int> startScript = new List<int> { 100001, 100018, 100101, 100201, 100301 };
    private List<int> endScript = new List<int> { 100017, 100036, 100121, 100219, 100315 };
    private List<int> scriptStartDay = new List<int> { 1, 1, 5, 10, 15};
    private List<bool> illExist = new List<bool> { true, false, false, false, false };

    private int dialogIndex = 0;
    private List<int> startDialog = new List<int> { 101911, 101921, 101931 };
    private List<int> endDialog = new List<int> { 101915, 101927, 101938 };
    private List<int> dialogStartDay = new List<int> { 4, 9, 14 };

    void Awake()
    {
        handler = this;
    }
    private void Start() {
        info = GameInfo.gameInfo;
    }
    private void Update() {
        
        if (scriptIndex <= 4 && info.Day >= scriptStartDay[scriptIndex] && info.Timer >= 80f && !script.isScriptMode) 
        {
            // 12시에 스토리 스크립트 재생하기 위한 조건 문
            if (startScript[scriptIndex] == 100101 || startScript[scriptIndex] == 100201 || startScript[scriptIndex] == 100301)
                if (info.Timer < 120f) return;

            PlayScript(startScript[scriptIndex], endScript[scriptIndex], illExist[scriptIndex]);
            // 스크립트 재생
            scriptIndex++;
        }
        if (dialogIndex <= 2 && info.Day >= dialogStartDay[dialogIndex] && info.Timer >= 80f) {
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
            break;
        }
    }
    public void EndingScriptPlay(int s, int e, bool ill) {
        PlayScript(s, e, ill);
    }

    public void ConditionalDialogPlay(int q_id) {
        if (dialog.IsSpeaking())
        {
            dialog.KillDialog();
            StartCoroutine(WaitForDialogAndPlay(q_id));
        }
        else
        {
            PlayDialogById(q_id);
        }
    }

    private IEnumerator WaitForDialogAndPlay(int q_id)
    {
        // dialog가 완전히 종료될 때까지 기다림.
        while (dialog.IsSpeaking())
        {
            yield return null; // 매 프레임 상태 체크
        }
        
        PlayDialogById(q_id);
    }

    private void PlayDialogById(int q_id)
    {
        switch (q_id)
        {
            case 132801: // 얼음 마녀 조사
                PlayDialog(101811, 101816);
                break;
            case 133801: // 호문클루스 연구시설 조사
                PlayDialog(101821, 101827);
                break;
            case 134801: // 새끼용 포획
                PlayDialog(101831, 101835);
                break;
            default:
                Debug.LogWarning("알 수 없는 대화 ID: " + q_id);
                break;
        }
    }

    private void PlayScript(int s, int e, bool i) {
        GameManager.gameManager.PauseGame();
        script.PrepareScriptText(s, e, i);
        script.ShowNextScript();
        script.isScriptMode = true;
    }
    private void PlayDialog(int s, int e) {
        dialog.PrepareDialogText(s, e);
    }

    public int GetToStoryDay() {
        return scriptStartDay[scriptIndex];
    }
}
