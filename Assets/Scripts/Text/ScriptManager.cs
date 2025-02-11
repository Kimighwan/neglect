using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ScriptManager : MonoBehaviour
{
    public static ScriptManager scriptManager;
    public GameObject background;
    public GameObject panel;
    public GameObject skipBtn;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerText;
    public float typingSpeed = 0.05f;

    public bool isScriptMode;
    private List<ScriptData> scriptList;
    private int currentLine = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        scriptManager = this;
        scriptList = new List<ScriptData>();
        
        for (int i = 100001; i <= 100015; i++)
        {
            ScriptData data = GetScriptData(i);
            if (data != null)
            {
                scriptList.Add(data);
            }
        }
        
        background.SetActive(true);
        panel.SetActive(true);
        skipBtn.SetActive(true);
        isScriptMode = true;
        ShowNextDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = scriptList[currentLine - 1].scriptLine;
                isTyping = false;
            }
            else
            {
                ShowNextDialogue();
            }
        }
    }

    public void ShowNextDialogue()
    {
        if (currentLine < scriptList.Count)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            
            ScriptData scriptData = scriptList[currentLine];
            speakerText.text = string.IsNullOrEmpty(scriptData.scriptSpeaker) ? "" : "「" + scriptData.scriptSpeaker + "」";
            typingCoroutine = StartCoroutine(TypeText(scriptData.scriptLine));
            currentLine++;
        }
        else
        {
            dialogueText.text = "";
            background.SetActive(false);
            panel.SetActive(false);
            skipBtn.SetActive(false);
            isScriptMode = false;
        }
    }

    IEnumerator TypeText(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    public void OnClickSkip()
    {
        currentLine = scriptList.Count;
        ShowNextDialogue();
    }

    private ScriptData GetScriptData(int scriptId)
    {
        return DataTableManager.Instance.GetScriptData(scriptId); // 기존 데이터 테이블 활용
    }
}
