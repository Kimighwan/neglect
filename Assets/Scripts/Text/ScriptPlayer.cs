using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Febucci.UI;
using TMPro;

public class ScriptPlayer : MonoBehaviour
{
    public Image illustration; // 일러스트 배경
    public Image backPanel; // 스크립트 나올 때 뒤에 불투명하게 처리
    public Image scriptPanel; // 위에 스크립트가 출력되는 패널
    public Image leftPos; // 왼쪽에 올 캐릭
    public Image middlePos; // 가운데에 올 캐릭
    public Image rightPos; // 오른쪽에 올 캐릭
    public Button skipButton; // 스크립트 전체 스킵 버튼 
    public TextMeshProUGUI speaker; // 캐릭터 이름 출력
    public TextMeshProUGUI script; // 스크립트 내용

    public bool isScriptMode;

    private List<ScriptData> scriptList;
    private int currentLine = 0;
    private bool isTyping = false;
    private Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();

    private void Start()
    {
        scriptList = new List<ScriptData>();
        leftPos.rectTransform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
    }

    private void Update()
    {
        if (isScriptMode && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            if (isTyping)
            {
                SkipTypingAnimator();
            }
            else
            {
                ShowNextScript();
            }
        }
    }

    public void PrepareScriptText(int startId, int endId)
    {
        for (int i = startId; i <= endId; i++)
        {
            ScriptData scriptData = DataTableManager.Instance.GetScriptData(i);
            scriptList.Add(scriptData);
        }
        AudioPlayer(endId);
        ActiveObjects(true);
    }
    private void AudioPlayer(int Id)
    {
        switch (Id)
        {
            case 100017:
                AudioManager.Instance.StopBGM();
                break;
            case 100036:
                AudioManager.Instance.StopBGM();
                break;
            case 109124:
                AudioManager.Instance.PlayBGM(BGM.ED1);
                break;
            default:
                AudioManager.Instance.PlayBGM(BGM.ScriptDefault);
                break;
        }
    }

    public void ShowNextScript()
    {
        if (currentLine < scriptList.Count)
        {
            ScriptData scriptData = scriptList[currentLine];
            if (scriptData.scriptIll != "") // 일러스트가 존재하는 경우
            {
                backPanel.gameObject.SetActive(false);
                illustration.gameObject.SetActive(true);
                SetIll(scriptData.scriptIll);
            }
            else // 일러스트가 존재하지 않는 경우
            {
                illustration.gameObject.SetActive(false);
                backPanel.gameObject.SetActive(true);
                SetCharacter(scriptData.scriptLeftPos, scriptData.scriptMiddlePos, scriptData.scriptRightPos);
            }

            scriptPanel.gameObject.SetActive(true);
            speaker.text = string.IsNullOrEmpty(scriptData.scriptSpeaker) ? "" : "「" + scriptData.scriptSpeaker + "」";
            WhoTalking(speaker.text, scriptData.scriptLeftPos.Split('_')[0], scriptData.scriptMiddlePos.Split('_')[0], scriptData.scriptRightPos.Split('_')[0]);
            TypeText(scriptData.scriptLine);
            currentLine++;
        }
        else
        {
            EndScripts();
        }
    }
    private void SetIll(string fileName)
    {
        if (fileName == "0") {
            AudioManager.Instance.StopBGM();
            illustration.sprite = null;
            illustration.color = new Color(0f, 0f, 0f);
            return;
        }
        else illustration.color = new Color(1f, 1f, 1f);
        illustration.sprite = LoadSprite(fileName, true);
        switch (fileName) {
            case "0_1":
                AudioManager.Instance.PlayBGM(BGM.ScriptIntro);
                break;
            case "0_2":
                AudioManager.Instance.PlayBGM(BGM.Script0_2);
                break;
        }
    }
    private void SetCharacter(string left, string middle, string right)
    {
        if (left != "")
        {
            leftPos.sprite = LoadSprite(left, false);
            leftPos.gameObject.SetActive(true);
        }
        else leftPos.gameObject.SetActive(false);
        if (middle != "")
        {
            middlePos.sprite = LoadSprite(middle, false);
            middlePos.gameObject.SetActive(true);
        }
        else middlePos.gameObject.SetActive(false);
        if (right != "")
        {
            rightPos.sprite = LoadSprite(right, false);
            rightPos.gameObject.SetActive(true);
        }
        else rightPos.gameObject.SetActive(false);
    }
    private Sprite LoadSprite(string fileName, bool ill)
    {
        if (!spriteCache.TryGetValue(fileName, out Sprite sprite))
        { // 캐시에 없으면 Resources 폴더에서 로드
            string path = ill ? $"Arts/Illustration/{fileName}" : $"Arts/Characters/{fileName}";
            sprite = Resources.Load<Sprite>(path);
            if (sprite != null) spriteCache[fileName] = sprite;
            else
            {
                Debug.LogWarning($"[SetCharacter] {path} 경로에서 스프라이트를 찾을 수 없음!");
                return null;
            }
        }
        return sprite;
    }
    private void WhoTalking(string name, string left, string middle, string right)
    {
        switch (name)
        {
            case "데이지":
                name = "Daisy";
                break;
            case "멜링":
                name = "Melling";
                break;
            default:
                return;
        }

        if (left == name)
        {
            leftPos.rectTransform.localScale = new Vector3(-1.2f, 1.2f, 1.2f);
            middlePos.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            rightPos.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else if (middle == name)
        {
            leftPos.rectTransform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            middlePos.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            rightPos.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else if (right == name)
        {
            leftPos.rectTransform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            middlePos.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            rightPos.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
    }
    private void TypeText(string line)
    {
        isTyping = true;
        script.GetComponent<TypewriterByCharacter>().ShowText(line);
    }
    public void SkipTypingAnimator() {
        isTyping = false;
        script.GetComponent<TypewriterByCharacter>().SkipTypewriter();
    }
    public void OnClickSkip()
    {
        currentLine = scriptList.Count;
        ShowNextScript();
    }

    private void ActiveObjects(bool b)
    {
        PoolManager.Instance.isNotTouch = b;
        scriptPanel.gameObject.SetActive(b);
        skipButton.gameObject.SetActive(b);
        speaker.gameObject.SetActive(b);
        script.gameObject.SetActive(b);
    }

    private void EndScripts() {
        int id = scriptList[currentLine - 1].scriptId;
        data.scr.text = "";
        ActiveObjects(false);
        scriptList.Clear();
        this.GetComponent<ScriptImageHandler>().EndTheScripts();
        currentLine = 0;
        isScriptMode = false;
        if (id != 109124)AudioManager.Instance.PlayBGM(BGM.Main6);

        switch (id) {
            case 100036:
                PoolManager.Instance.isNotTutorialTouch = true;
                tutorialImg.SetActive(true);
                break;
            case 100121: // 슬라임 홍수 시작
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
            case 100377:
                ScriptHandler.handler.EndingScriptPlay(109101, 109124, false);
                break;
            case 109124:
                Fade.Instance.DoFade(Color.black, 0f, 1f, 1f, 0f, false);
                Invoke("EndTheGame", 1f);
                break;
        }
    }

    private void EndTheGame() {
        GameManager.gameManager.EndTheGame();
    }

    public void OnClickTutorialBtn()
    {
        tutorialImg.SetActive(true);
        PoolManager.Instance.isNotTutorialTouch = true;
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

