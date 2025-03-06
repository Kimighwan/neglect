using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogMode : MonoBehaviour
{
    private bool onSpeaking = false;
    private ScriptDialogObjectData data;
    private float holdTime = 2f;
    private float changeHoldTime = 2f;
    private bool killDialog = false;

    private Queue<ScriptData> scriptQueue = new Queue<ScriptData>();
    private bool isSpeakerA = true;

    void Start() {
        data = ScriptDialogObjectData.data;
    }
    
    public void ChangeDialogSpeed(float f) {
        holdTime = changeHoldTime / f;
    }

    public void PrepareDialogText(int startId, int endId)
    {
        onSpeaking = false;
        killDialog = false;
        scriptQueue.Clear();
        for (int i = startId; i <= endId; i++)
        {
            ScriptData scriptData = DataTableManager.Instance.GetScriptData(i);
            if (data != null)
            {
                scriptQueue.Enqueue(scriptData);
            }
        }
        onSpeaking = true;
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        data.ActiveAllDialogObject(false, false);

        while (scriptQueue.Count > 0)
        {
            ScriptData scriptData = scriptQueue.Dequeue();

            StartCoroutine(TypeSentence(scriptData));

            yield return new WaitForSeconds(holdTime);

            while (GameManager.gameManager.Pause) {
                yield return null;
            }
            if (killDialog) {
                killDialog = false;
                scriptQueue.Clear();
                break;
            }
        }

        data.ActiveAllDialogObject(false, false);
        onSpeaking = false;
    }

    private IEnumerator TypeSentence(ScriptData scriptData)
    {
        if (isSpeakerA)
        {
            data.ActiveAllDialogObject(true, false);
            data.malpungseon1Text.text = "";
            data.speaker1Text.text = scriptData.scriptSpeaker;
            yield return StartCoroutine(TypeEffect(data.malpungseon1Text, scriptData.scriptLine, data.malpungseonUI1, data.malpungseon1));
        }
        else
        {
            data.ActiveAllDialogObject(false, true);
            data.malpungseon2Text.text = "";
            data.speaker2Text.text = scriptData.scriptSpeaker;
            yield return StartCoroutine(TypeEffect(data.malpungseon2Text, scriptData.scriptLine, data.malpungseonUI2, data.malpungseon2));
        }
        isSpeakerA = !isSpeakerA;
    }

    // 수정된 부분: 전체 문장을 즉시 출력
    private IEnumerator TypeEffect(TextMeshProUGUI targetText, string sentence, GameObject targetUI, GameObject targetMalpungseon) {
        targetText.text = sentence;

        // Vector3 originalUIPos = targetUI.transform.localPosition;
        // Vector3 originalPosMal = targetMalpungseon.transform.position;

        // int shakeCount = 3;
        // float period = 0.5f;
        // float uiOffsetY = 5f;
        // float offsetY = 0.0741f;

        // for (int i = 0; i < shakeCount; i++)
        // {
        //     float elapsed = 0f;
            
        //     while (elapsed < period)
        //     {
        //         float t = elapsed / period;

        //         // UI는 local position에 오프셋을 더해 업데이트
        //         targetUI.transform.localPosition = originalUIPos + new Vector3(0, uiOffsetY, 0);

        //         // UI offset을 world space로 변환하여 말풍선에도 적용
        //         Vector3 worldOffset = targetUI.transform.TransformVector(new Vector3(0, offsetY, 0));
        //         targetMalpungseon.transform.position = originalPosMal + worldOffset;

        //         elapsed += Time.deltaTime;
        //         yield return null;
        //     }
        //     uiOffsetY *= -1;
        //     offsetY *= -1;
        // }

        // // 움직임 종료 후 원래 위치로 복원
        // targetUI.transform.localPosition = originalUIPos;
        // targetMalpungseon.transform.position = originalPosMal;

        yield return null;
    }

    public void KillDialog() {
        killDialog = true;
    }
    public bool IsSpeaking() {
        return onSpeaking;
    }
}
