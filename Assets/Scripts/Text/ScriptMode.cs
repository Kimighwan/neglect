using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptMode : MonoBehaviour
{
    public float typingSpeed = 0.05f;
    public bool isScriptMode;

    private ScriptObjectData data;
    private List<ScriptData> scriptList;
    private int currentLine = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private void Start()
    {
        data = ScriptObjectData.data;
        scriptList = new List<ScriptData>();
        PrepareScriptText(100001, 100099);
        ShowNextScript();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                data.scr.text = scriptList[currentLine - 1].scriptLine;
                isTyping = false;
            }
            else
            {
                ShowNextScript();
            }
        }
    }

    public void PrepareScriptText(int startId, int endId) {
        for (int i = startId; i <= endId; i++)
        {
            ScriptData data = DataTableManager.Instance.GetScriptData(i);
            if (data != null)
            {
                scriptList.Add(data);
            }
        }
        
        data.background.SetActive(true);
        data.panel.SetActive(true);
        data.skipBtn.SetActive(true);
        isScriptMode = true;
    }

    public void ShowNextScript()
    {
        if (currentLine < scriptList.Count)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            
            ScriptData scriptData = scriptList[currentLine];
            if (scriptData.scriptExp != "") ShowCharWithExp(scriptData.scriptSpeaker, scriptData.scriptExp);
            data.scrSpeaker.text = string.IsNullOrEmpty(scriptData.scriptSpeaker) ? "" : "「" + scriptData.scriptSpeaker + "」";
            typingCoroutine = StartCoroutine(TypeText(scriptData.scriptLine));
            currentLine++;
        }
        else
        {
            data.scr.text = "";
            data.background.SetActive(false);
            data.panel.SetActive(false);
            data.skipBtn.SetActive(false);
            scriptList.Clear();
            isScriptMode = false;
        }
    }

    IEnumerator TypeText(string line)
    {
        isTyping = true;
        data.scr.text = "";

        foreach (char letter in line)
        {
            data.scr.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
    
    public void OnClickSkip()
    {
        currentLine = scriptList.Count;
        ShowNextScript();
    }

    private void ShowCharWithExp(string c, string e) {
        if (c == "안나") {
            
        }
        else if (c == "본부직원") {
            
        }
    }
}
