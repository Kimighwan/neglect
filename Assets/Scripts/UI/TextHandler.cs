using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
모든 UI관련된 텍스트를 관리하기 위해 만든 스크립트
자동 업데이트 되도록 만들 예정
*/

public class TextHandler : MonoBehaviour
{
    public static TextHandler textHandler;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI plusGoldText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI neededGoldText;

    private void Awake() {
        textHandler = this;
    }

    public void UpdateTexts() {
        UpdateGoldText();
        dayText.text = $"Day {GameInfo.gameInfo.Day.ToString()}";
        UpdateTimeText();
        UpdateLevelText();
    }

    private void UpdateGoldText() {
        int g = GameInfo.gameInfo.Gold;
        string s = g.ToString("N0");
        goldText.text = s;
        plusGoldText.text = $"+{GameInfo.gameInfo.plusGold.ToString()}";
    }
    
    // 시간 정보 자동 변환
    private void UpdateTimeText() {
        float t = GameInfo.gameInfo.Timer;
        string s = "";
        int i = (int)Math.Truncate(t);
        if (i < 12) {
            i = (i == 0) ? 12 : (i % 12);
            s = $"AM {i}";
        }
        else {
            i = (i == 12) ? 12 : (i % 12);
            s = $"PM {i}";
        }
        timeText.text = s;
    }

    private void UpdateLevelText() {
        levelText.text = $"Level {GameInfo.gameInfo.Level.ToString()}";
        int g = GameInfo.gameInfo.GetNeededGold();
        if (g != 0) {
            neededGoldText.text = $"필요한 골드 {g}";
        }
        else neededGoldText.text = "Max Level";
    }
}
