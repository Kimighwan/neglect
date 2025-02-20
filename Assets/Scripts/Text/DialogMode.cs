using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogMode : MonoBehaviour
{
    private ScriptDialogObjectData data;
    private float minTypingSpeed = 0.2f;
    private float maxTypingSpeed = 0.8f;
    private float holdTime = 1f;
    public float changeMinTypingSpeed = 0.2f;
    public float changeMaxTypingSpeed = 0.8f;
    public float changeHoldTime = 0.5f;

    private Queue<ScriptData> scriptQueue = new Queue<ScriptData>();
    private bool isSpeakerA = true;

    void Start() {
        data = ScriptDialogObjectData.data;
    }
    public void ChangeDialogSpeed(float f) {
        minTypingSpeed = changeMinTypingSpeed / f;
        maxTypingSpeed = changeMaxTypingSpeed / f;
        holdTime = changeHoldTime / f;
    }

    public void PrepareDialogText(int startId, int endId)
    {
        scriptQueue.Clear();
        for (int i = startId; i <= endId; i++)
        {
            ScriptData scriptData = DataTableManager.Instance.GetScriptData(i);
            if (data != null)
            {
                scriptQueue.Enqueue(scriptData);
            }
        }
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        data.ActiveAllDialogObject(false, false);

        while (scriptQueue.Count > 0)
        {
            ScriptData scriptData = scriptQueue.Dequeue();
            yield return StartCoroutine(TypeSentence(scriptData));

            while (GameManager.gameManager.IsPaused()) {
                yield return null;
            }

            yield return new WaitForSeconds(holdTime);
        }
        yield return new WaitForSeconds(0.5f);

        data.ActiveAllDialogObject(false, false);
    }

    private IEnumerator TypeSentence(ScriptData scriptData)
    {
        if (isSpeakerA)
        {
            data.ActiveAllDialogObject(true, false);
            data.malpungseon1Text.text = "";
            data.speaker1Text.text = scriptData.scriptSpeaker;
            yield return StartCoroutine(TypeEffect(data.malpungseon1Text, scriptData.scriptLine));
        }
        else
        {
            data.ActiveAllDialogObject(false, true);
            data.malpungseon2Text.text = "";
            data.speaker2Text.text = scriptData.scriptSpeaker;
            yield return StartCoroutine(TypeEffect(data.malpungseon2Text, scriptData.scriptLine));
        }
        isSpeakerA = !isSpeakerA;
    }

    private IEnumerator TypeEffect(TextMeshProUGUI targetText, string sentence)
    {
        float typingSpeed = Mathf.Lerp(maxTypingSpeed, minTypingSpeed, Mathf.Clamp01(sentence.Length / 100f));
        foreach (char letter in sentence)
        {
            targetText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
