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
        }
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
    private IEnumerator HoldText() {
        yield return new WaitForSeconds(holdTime);
        SkipTypeWriter();
        PlayDialogLine();
    }

    public void SkipTypeWriter() {
        if (isSpeakerA) data.malpungseon1Text.GetComponent<TypewriterByCharacter>().SkipTypewriter();
        else data.malpungseon2Text.GetComponent<TypewriterByCharacter>().SkipTypewriter();
    }
    public void SkipTypingAnimation() {
        StartCoroutine(HoldText());
    }
    public void KillDialog() {
        scriptQueue.Clear();
        SkipTypeWriter();
    }
    public void StopTypingAnimation(bool b) {
        if (b) {
            if (isSpeakerA) data.malpungseon1Text.GetComponent<TypewriterByCharacter>().StopShowingText();
            else data.malpungseon2Text.GetComponent<TypewriterByCharacter>().StopShowingText();
        }
        else {
            if (isSpeakerA) data.malpungseon1Text.GetComponent<TypewriterByCharacter>().StartShowingText(false);
            else data.malpungseon2Text.GetComponent<TypewriterByCharacter>().StartShowingText(false);
        }
    }
    public bool IsSpeaking() {
        return onSpeaking;
    }
}
