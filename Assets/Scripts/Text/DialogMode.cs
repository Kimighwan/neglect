using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogMode : MonoBehaviour
{
    private float minTypingSpeed = 0.2f;
    private float maxTypingSpeed = 0.8f;
    private float holdTime = 1f;
    public float changeMinTypingSpeed = 0.2f;
    public float changeMaxTypingSpeed = 0.8f;
    public float changeHoldTime = 0.5f;

    private TextMeshProUGUI textA;
    private TextMeshProUGUI charTextA;
    private GameObject uiMalpungseonA;
    private GameObject malpungseonA;
    
    private TextMeshProUGUI textB;
    private TextMeshProUGUI charTextB;
    private GameObject uiMalpungseonB;
    private GameObject malpungseonB;

    private Queue<ScriptData> scriptQueue = new Queue<ScriptData>();
    private bool isSpeakerA = true;
    private int dialogIndex = 0;

    public void UpdateDialog() {
        if (!GameInfo.gameInfo.malpungseonOnce && GameInfo.gameInfo.Day == ScriptDialogObjectData.data.GetDialogStartDay(dialogIndex)) {
            if ((GameInfo.gameInfo.Timer/10) > ScriptDialogObjectData.data.GetDialogStartTime(dialogIndex)) {
                bool b = (dialogIndex % 2 == 0) ? true : false; 
                PrepareDialogText(ScriptDialogObjectData.data.GetDialogStartId(dialogIndex), ScriptDialogObjectData.data.GetDialogEndId(dialogIndex), b);
                GameInfo.gameInfo.malpungseonOnce = true;
                dialogIndex++;
            }
        }
    }

    public void ChangeDialogSpeed(float f) {
        minTypingSpeed = changeMinTypingSpeed / f;
        maxTypingSpeed = changeMaxTypingSpeed / f;
        holdTime = changeHoldTime / f;
    }

    public void PrepareDialogText(int startId, int endId, bool isDeskL)
    {
        AllocateObject();
        scriptQueue.Clear();
        for (int i = startId; i <= endId; i++)
        {
            ScriptData data = DataTableManager.Instance.GetScriptData(i);
            if (data != null)
            {
                scriptQueue.Enqueue(data);
            }
        }
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        ActiveObjectAB(false, false);

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

        ActiveObjectAB(false, false);
    }

    private IEnumerator TypeSentence(ScriptData scriptData)
    {
        if (isSpeakerA)
        {
            ActiveObjectAB(true, false);
            textA.text = "";
            charTextA.text = scriptData.scriptSpeaker;
            yield return StartCoroutine(TypeEffect(textA, scriptData.scriptLine));
        }
        else
        {
            ActiveObjectAB(false, true);
            textB.text = "";
            charTextB.text = scriptData.scriptSpeaker;
            yield return StartCoroutine(TypeEffect(textB, scriptData.scriptLine));
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

    private void ActiveObjectAB(bool a, bool b) {
        uiMalpungseonA.SetActive(a);
        malpungseonA.SetActive(a);
        uiMalpungseonB.SetActive(b);
        malpungseonB.SetActive(b);
    }

    private void AllocateObject() {
        ScriptDialogObjectData data = ScriptDialogObjectData.data;
        textA = data.malpungseon1Text;
        charTextA = data.speaker1Text;
        uiMalpungseonA = data.malpungseon11;
        malpungseonA = data.malpungseon1;

        textB = data.malpungseon2Text;
        charTextB = data.speaker2Text;
        uiMalpungseonB = data.malpungseon22;
        malpungseonB = data.malpungseon2;
    }
}
