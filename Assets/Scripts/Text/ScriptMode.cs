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
    private int outSpeaker = -1;

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

    public void PrepareScriptText(int startId, int endId, bool b) {
        for (int i = startId; i <= endId; i++)
        {
            ScriptData scriptData = DataTableManager.Instance.GetScriptData(i);
            if (data != null)
            {
                scriptList.Add(scriptData);
            }
        }
        illExist = b;
        if (!b) AudioManager.Instance.PlayBGM(BGM.ScriptDefault); 
        ActiveObjects(true);
    }

    public void ShowNextScript()
    {
        if (currentLine < scriptList.Count)
        {
            // 일러스트 설정
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            ScriptData scriptData = scriptList[currentLine];
            if (scriptData.scriptExp != "") {
                ShowCharWithExp(scriptData.scriptSpeaker, scriptData.scriptExp, scriptData.scriptPos);
            }
            else if (scriptData.scriptIll != "") ShowIllImage(scriptData.scriptIll);

            if (outSpeaker != -1) {
                this.GetComponent<ScriptImageHandler>().OutSpeaker(outSpeaker);
                outSpeaker = -1;
            }
            if (scriptData.scriptInOut == "out") {
                if (scriptData.scriptPos == "left") outSpeaker = 0;
                else if (scriptData.scriptPos == "middle") outSpeaker = 1;
                else if (scriptData.scriptPos == "right") outSpeaker = 2;
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
        currentLine = scriptList.Count;
        ShowNextScript();
    }

    private void ShowCharWithExp(string name, string exp, string pos) {
        string charName = "";
        if (name == "데이지") charName = "Daisy";
        else if (name == "멜링" || name == "???") charName = "Melling";
        this.GetComponent<ScriptImageHandler>().SetCharacter(charName, exp, pos);
    }
    private void ShowIllImage(string fileName) {
        this.GetComponent<ScriptImageHandler>().SetIllImage(fileName);
    }

    private void ActiveObjects(bool b) {
        if (b) {
            if (illExist) data.background.SetActive(b);
            else data.backPanel.SetActive(b);
        }
        else {
            data.background.SetActive(b);
            data.backPanel.SetActive(b);
        }
        PoolManager.Instance.isNotTouch = b;
        data.panel.SetActive(b);
        data.skipBtn.SetActive(b);
    }

    private void EndScripts() {
        GameManager.gameManager.PauseGame();
        int id = scriptList[currentLine - 1].scriptId;

        if (id == 109124)
        {
            ScriptDialogHandler.handler.EndingIll();
        }
        if (id == 109125)
        {
            GameManager.gameManager.EndTheGame();
        }
        if (id == 100036)
        {
            PoolManager.Instance.isNotTutorialTouch = true;
            GameManager.gameManager.PauseGame();
            tutorialImg.SetActive(true);
        }
        data.scr.text = "";
        ActiveObjects(false);
        scriptList.Clear();
        this.GetComponent<ScriptImageHandler>().EndTheScripts();
        currentLine = 0;
        isScriptMode = false;
        AudioManager.Instance.PlayBGM(BGM.Main6);
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
        }
    }

    public void OnClickTutorialBtn()
    {
        tutorialImg.SetActive(true);
        PoolManager.Instance.isNotTutorialTouch = true;

        if (!GameManager.gameManager.Pause) GameManager.gameManager.PauseGame();
    }

    public void EndingScript(int i) {
        GameInfo.gameInfo.PrepareShowIll(1f, 0f, true);
        ScriptData scriptData = DataTableManager.Instance.GetScriptData(i);
        if (data != null)
        {
            scriptList.Add(scriptData);
        }
        illExist = true;
        ActiveObjects(true);
        data.panel.SetActive(false);
        data.skipBtn.SetActive(false);
        ShowIllImage(scriptData.scriptIll);
    }
}
