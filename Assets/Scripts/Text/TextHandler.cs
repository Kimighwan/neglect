using System;
using TMPro;
using UnityEngine;

/*
모든 UI관련된 텍스트를 관리하기 위해 만든 스크립트
자동 업데이트 되도록 만들 예정
*/

public class UITextHandler : MonoBehaviour
{
    public static UITextHandler textHandler;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI toStoryText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI neededGoldText;

    private void Awake() {
        textHandler = this;
    }

    public void UpdateTexts() {
        UpdateGoldText();
        UpdateDayText();
        UpdateTimeText();
        UpdateLevelText();
    }

    private void UpdateGoldText() {
        int g = GameInfo.gameInfo.Gold;
        string s = g.ToString("N0");
        goldText.text = s;
    }

    private void UpdateDayText() {
        int x = GameInfo.gameInfo.Day;
        dayText.text = $"Day {x}";
        int y = ScriptDialogHandler.handler.GetToStoryDay() - x;
        if (y == 0) toStoryText.text = "D-Day";
        else toStoryText.text = $"Next D-{y}";
    }
    
    // 시간 정보 자동 변환
    private void UpdateTimeText() {
        float t = GameInfo.gameInfo.Timer;
        string s = "";
        int i = (int)Math.Truncate(t);
        if (i < 120) {
            int j = (i >= 0 && i < 10) ? 12 : (i/10 % 12);
            s = $"AM {j}";
        }
        else {
            int j = (i >= 120 && i < 130) ? 12 : (i/10 % 12);
            s = $"PM {j}";
        }
        timeText.text = s;
    }

    private void UpdateLevelText() {
        levelText.text = $"Lv {GameInfo.gameInfo.Level.ToString()}";
        int g = GameInfo.gameInfo.GetNeededGold();
        if (g != 0) {
            neededGoldText.text = $"필요한 골드 {g}";
        }
        else neededGoldText.text = "Max Lv";
    }
}
