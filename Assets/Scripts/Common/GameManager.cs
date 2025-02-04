using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool pause = false;
    void Start()
    {
        GameInfo.gameInfo.StartGameInfo();
    }

    void Update()
    {
        if (!pause) {
            GameInfo.gameInfo.UpdateGameInfo();
            TextHandler.textHandler.UpdateTexts();
        }
    }

    public void PauseGame() {
        pause = !pause;
        GameInfo.gameInfo.gameSpeed = 1f;
    }
    public void QuickGame() {
        GameInfo.gameInfo.gameSpeed = 4f;
    }
}
