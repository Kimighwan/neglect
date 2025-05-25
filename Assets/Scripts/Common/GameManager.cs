using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform cameraTransform;
    public static GameManager gameManager;
    private GameInfo info;

    void Awake()
    {
        gameManager = this;
    }
    void Start()
    {
        AudioManager.Instance.BgmVol = AudioManager.Instance.BgmVol * 10.0f;
        AudioManager.Instance.UpdateVolume();
        info = GameInfo.gameInfo;
        info.StartGameInfo();
        ScriptHandler.scriptHandler.PlayIntroScript();
    }

    // 코나미 코드에 해당하는 키 시퀀스
    private KeyCode[] konamiCode = new KeyCode[]
    {
        KeyCode.UpArrow,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.B,
        KeyCode.A
    };
    private int index = 0;
    void Update()
    {
        UITextHandler.textHandler.UpdateTexts();
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(konamiCode[index]))
            {
                index++;

                // 모든 순서를 다 입력했다면
                if (index == konamiCode.Length)
                {
                    GameInfo.gameInfo.ChangeGold(999999);
                    index = 0;
                }
            }
            else
            {
                if (Input.GetKeyDown(konamiCode[0]))
                {
                    index = 1;
                }
                else
                {
                    index = 0;
                }
            }
        }
    }
    public void EndTheGame()
    {
        Fade.Instance.DoFade(Color.black, 0f, 1f, 2f, 0f, true, () =>
        {
            // 게임 엔딩 로직
            SceneManager.LoadScene(0);
        });
    }
}
