using UnityEngine;
using System.Collections;
using TMPro;

public class ScriptManager : MonoBehaviour
{
    public static ScriptManager scriptManager;
    public GameObject background;
    //public Image bgImage;
    public GameObject panel;
    public TextMeshProUGUI dialogueText;  // UI에 연결할 Text 오브젝트
    public TextMeshProUGUI speakerText;
    public float typingSpeed = 0.05f; // 글자 출력 속도 조절 (낮을수록 빠름)
    
    public bool isScriptMode;
    private string[] lines;     // 대사 저장 배열
    private string s;
    private int currentLine = 0;
    private bool isTyping = false; // 현재 타이핑 중인지 확인
    private Coroutine typingCoroutine;

    void Start()
    {
        scriptManager = this;
        TextAsset textAsset = Resources.Load<TextAsset>("TextScripts/IntroScript");
        lines = textAsset.text.Split('\n'); // 줄 단위로 나누기
        background.SetActive(true);
        isScriptMode = true;
        ShowNextDialogue(); // 첫 번째 대사 출력
    }

    void Update()
    {
        // Enter, Space, 마우스 좌클릭 입력 감지
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                // 타이핑 도중 클릭하면 즉시 전체 출력
                StopCoroutine(typingCoroutine);
                dialogueText.text = s; // 현재 대사 전체 표시
                isTyping = false;
                s = "";
            }
            else
            {
                ShowNextDialogue();
            }
        }
    }

    public void ShowNextDialogue()
    {
        if (currentLine < lines.Length)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            s = lines[currentLine].Trim();
            if (s[0] == '$') {
                int start = s.IndexOf('{') + 1;
                int end = s.IndexOf('}');
                string speaker = s.Substring(start, end - start); // 인물 이름 추출
                speakerText.text = '「' + speaker + '」'; // UI에 인물 정보 표시
                s = s.Substring(end + 1); // '}' 다음부터 대사 가져오기
            } else speakerText.text = "";
            typingCoroutine = StartCoroutine(TypeText(s));
            currentLine++; // 다음 대사로 이동
        }
        else
        {
            dialogueText.text = ""; // 모든 대사가 끝나면 빈 문자열 표시
            background.SetActive(false);
            panel.SetActive(false);
            isScriptMode = false;
        }
    }

    IEnumerator TypeText(string line)
    {
        isTyping = true;
        dialogueText.text = ""; // 텍스트 초기화

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed); // 글자 출력 속도 조절
        }

        isTyping = false;
    }

    public bool IsTyping() {
        return isTyping;
    }
}
