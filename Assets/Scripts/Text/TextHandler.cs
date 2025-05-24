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
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI neededGoldText;

    private void Awake() {
        textHandler = this;
    }

    public void UpdateTexts() {
        UpdateGoldText();
        UpdateDayText();
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
        int y = ScriptHandler.scriptHandler.GetToStoryDay() - x;
        if (y == 0) toStoryText.text = "D-Day";
        else toStoryText.text = $"Next\nD-{y}";
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
