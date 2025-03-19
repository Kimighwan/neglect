using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptDialogHandler : MonoBehaviour
{
    public static ScriptDialogHandler handler;
    public ScriptMode script;
    public DialogMode dialog;

    private ScriptDialogObjectData data;
    private GameInfo info;
    private int scriptIndex = 0;
    private int dialogIndex = 0;

    void Awake() {
        handler = this;
    }
    private void Start() {
        info = GameInfo.gameInfo;
        data = ScriptDialogObjectData.data;
    }
    private void Update() {
        if (info.Day >= data.GetScrStartDay(scriptIndex) && info.Timer >= data.GetScrStartTime(scriptIndex) && !script.isScriptMode) 
        {
            PlayScript(data.GetScrStartId(scriptIndex), data.GetScrEndId(scriptIndex));
            scriptIndex++;
        }
        if (info.Day >= data.GetDiaStartDay(scriptIndex) && info.Timer >= data.GetDiaStartTime(scriptIndex)) {
            PlayDialog(data.GetDiaStartId(scriptIndex), data.GetDiaEndId(scriptIndex));
            dialogIndex++;
        }
    }

    #region ScriptPart

    private void PlayScript(int s, int e) {
        GameManager.gameManager.PauseGame();
        script.isScriptMode = true;
        script.PlayScriptText(s, e);
    }
    public void ConditionalScriptPlay(int q_id) {
        switch (q_id) {
        case 132901: // 1챕터
            PlayScript(100151, 100171);
            break;
        case 133902: // 2챕터
            PlayScript(100251, 100260);
            break;
        case 139999: // 3챕터
            PlayScript(100351, 100377);
            PlayScript(109101, 109124);
            break;
        }
    }

    #endregion

    #region DialogPart

    private void PlayDialog(int s, int e) {
        dialog.PlayDialogText(s, e);
    }
    public void ConditionalDialogPlay(int q_id) {
        if (dialog.IsSpeaking())
        {
            dialog.KillDialog();
            StartCoroutine(WaitForDialogAndPlay(q_id));
        }
        else
        {
            PlayDialogWithId(q_id);
        }
    }
    private IEnumerator WaitForDialogAndPlay(int q_id) {
        // dialog가 완전히 종료될 때까지 기다림.
        while (dialog.IsSpeaking())
        {
            yield return null; // 매 프레임 상태 체크
        }
        
        PlayDialogWithId(q_id);
    }
    private void PlayDialogWithId(int q_id) {
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

    #endregion
}
