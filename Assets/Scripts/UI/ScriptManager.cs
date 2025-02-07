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
    public float typingSpeed = 0.05f; // 글자 출력 속도 조절 (낮을수록 빠름)
    
    private string[] lines;     // 대사 저장 배열
    private int currentLine = 0;
    private bool isTyping = false; // 현재 타이핑 중인지 확인
    private Coroutine typingCoroutine;

    void Start()
    {
        scriptManager = this;
        TextAsset textAsset = Resources.Load<TextAsset>("TextScripts/TestScript");
        lines = textAsset.text.Split('\n'); // 줄 단위로 나누기
        IntroScriptMode();
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
                dialogueText.text = lines[currentLine - 1].Trim(); // 현재 대사 전체 표시
                isTyping = false;
            }
            else
            {
                ShowNextDialogue();
            }
        }
    }

    public void IntroScriptMode() {
        background.SetActive(true);
        panel.SetActive(true);
        ShowNextDialogue(); // 첫 번째 대사 출력
    }
    public bool IsTyping() {
        return isTyping;
    }

    private void ShowNextDialogue()
    {
        if (currentLine < lines.Length)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            typingCoroutine = StartCoroutine(TypeText(lines[currentLine].Trim()));
            currentLine++; // 다음 대사로 이동
        }
        else
        {
            dialogueText.text = ""; // 모든 대사가 끝나면 빈 문자열 표시
            background.SetActive(false);
            panel.SetActive(false);
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
}
