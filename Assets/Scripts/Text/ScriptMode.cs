using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptMode : MonoBehaviour
{
    public GameObject tutorialImg;

    public float typingSpeed = 0.05f;
    public bool isScriptMode;

    private ScriptDialogObjectData data;
    private List<ScriptData> scriptList;
    private int currentLine = 0;
    private bool isTyping = false;
    private bool illExist = true;
    private Coroutine typingCoroutine;

    private void Start()
    {
        data = ScriptDialogObjectData.data;
        scriptList = new List<ScriptData>();
    }

    private void Update()
    {
        if (isScriptMode && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
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
        this.GetComponent<ScriptImageHandler>().speakerNum = 0;
        for (int i = startId; i <= endId; i++)
        {
            ScriptData scriptData = DataTableManager.Instance.GetScriptData(i);
            if (data != null)
            {
                scriptList.Add(scriptData);
            }
        }
        ActiveObjects(true);
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
            if (scriptData.scriptIll != "") ShowIllImage(scriptData.scriptIll); // 일러스트 있는 스크립트 라인
            else {
                AudioManager.Instance.PlayBGM(BGM.ScriptDefault);
                if (scriptData.scriptInOut == "in") this.GetComponent<ScriptImageHandler>().speakerNum++;
                if (scriptData.scriptExp != "") ShowCharWithExp(scriptData.scriptSpeaker, scriptData.scriptExp, scriptData.scriptInOut, scriptData.scriptPos); // 캐릭터 있는 스크립트 라인
            }

            data.scrSpeaker.text = string.IsNullOrEmpty(scriptData.scriptSpeaker) ? "" : "「" + scriptData.scriptSpeaker + "」";
            typingCoroutine = StartCoroutine(TypeText(scriptData.scriptLine));
            currentLine++;
        }
        else
        {
            EndScripts();
        }
    }

    public IEnumerator TypeText(string line)
    {
        isTyping = true;
        data.scr.text = ""; // 초기화
        
        bool insideTag = false;
        
        foreach (char letter in line)
        {
            if (letter == '<') insideTag = true;

            data.scr.text += letter;

            if (letter == '>') insideTag = false;

            if (!insideTag)
            {
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        isTyping = false;
    }
    
    public void OnClickSkip()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        currentLine = scriptList.Count;
        ShowNextScript();
    }

    private void ShowCharWithExp(string name, string exp, string inout, string pos) {
        string charName = "";
        if (name == "데이지") charName = "Daisy";
        else if (name == "멜링" || name == "???") charName = "Melling";
        this.GetComponent<ScriptImageHandler>().SetCharacter(charName, exp, inout, pos);
    }
    private void ShowIllImage(string fileName) {
        this.GetComponent<ScriptImageHandler>().SetIllImage(fileName);
    }

    private void ActiveObjects(bool b) {
        data.background.SetActive(b);
        PoolManager.Instance.isNotTouch = b;
        data.panel.SetActive(b);
        data.skipBtn.SetActive(b);
    }

    private void EndScripts() {
        int id = scriptList[currentLine - 1].scriptId;
        data.scr.text = "";
        currentLine = 0;
        ActiveObjects(false);
        scriptList.Clear();
        this.GetComponent<ScriptImageHandler>().EndTheScripts();

        GameManager.gameManager.PauseGame();
        isScriptMode = false;
        ScriptEndAct(id);
        AudioManager.Instance.PlayBGM(BGM.Main6);
    }

    private void ScriptEndAct(int id) {
        switch (id) {
            case 100120: // 슬라임 홍수 시작
                var slimeUiData = new EmergencyQuestUIData(11);
                UIManager.Instance.OpenUI<EmergencyQuestUI>(slimeUiData);
                break;
            case 100219: // 몬스터 웨이브 시작
                var monsterWaveUiData = new EmergencyQuestUIData(12);
                UIManager.Instance.OpenUI<EmergencyQuestUI>(monsterWaveUiData);
                break;
            case 100315: // 모험가 구함 시작
                var uiData = new EmergencyQuestUIData(13);
                UIManager.Instance.OpenUI<EmergencyQuestUI>(uiData);
                break;
            case 100036:
                PoolManager.Instance.isNotTutorialTouch = true;
                GameManager.gameManager.PauseGame();
                tutorialImg.SetActive(true);
                break;
            case 109124:
                GameManager.gameManager.EndTheGame();
                break;
        }
    }

    public void OnClickTutorialBtn()
    {
        tutorialImg.SetActive(true);
        PoolManager.Instance.isNotTutorialTouch = true;

        if (!GameManager.gameManager.Pause) GameManager.gameManager.PauseGame();
    }
}
