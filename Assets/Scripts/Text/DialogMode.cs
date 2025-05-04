using Febucci.UI;
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
        data.ActiveAllDialogObject(false, false);
        PlayDialogLine();
    }

    public void PlayDialogLine()
    {
        if (scriptQueue.Count > 0) {
            ScriptData scriptData = scriptQueue.Dequeue();
            TypeSentence(scriptData);
            // 출력하고 HoldTime 추가
            // 퍼즈하면 재생되다가 멈추도록
            // killDialog뜨면 그 상태로 끝내야함
        }
        // while (scriptQueue.Count > 0)
        // {
        //     yield return new WaitForSeconds(holdTime);
        //     while (GameManager.gameManager.Pause) {
        //         yield return null;
        //     }
        // }
        else {
            onSpeaking = false;
            data.ActiveAllDialogObject(false, false);
        }
    }

    private void TypeSentence(ScriptData scriptData)
    {
        if (scriptData.scriptSpeaker == "NPC1") isSpeakerA = true;
        else isSpeakerA = false;

        if (isSpeakerA)
        {
            data.ActiveAllDialogObject(true, false);
            data.malpungseon1Text.GetComponent<TypewriterByCharacter>().ShowText(scriptData.scriptLine);
            data.speaker1Text.text = scriptData.scriptSpeaker;
        }
        else
        {
            data.ActiveAllDialogObject(false, true);
            data.malpungseon2Text.GetComponent<TypewriterByCharacter>().ShowText(scriptData.scriptLine);
            data.speaker2Text.text = scriptData.scriptSpeaker;
        }
    }

    public void SkipTypingAnimation() {
        if (isSpeakerA)
        {
            data.malpungseon1Text.GetComponent<TypewriterByCharacter>().StartDisappearingText();
            data.malpungseon1Text.GetComponent<TypewriterByCharacter>().SkipTypewriter();
        }
        else
        {
            data.malpungseon2Text.GetComponent<TypewriterByCharacter>().StartDisappearingText();
            data.malpungseon2Text.GetComponent<TypewriterByCharacter>().SkipTypewriter();
        }
        PlayDialogLine();
    }

    public void KillDialog() {
        scriptQueue.Clear();
    }
    public bool IsSpeaking() {
        return onSpeaking;
    }
}
