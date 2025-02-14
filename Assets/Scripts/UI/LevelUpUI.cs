using TMPro;
using UnityEngine;

public class LevelUpUI : BaseUI
{
    public TextMeshProUGUI levelInfo;
    public TextMeshProUGUI otherInfo;
    private float fill = 0f;

    private void Start() {
        int l = GameInfo.gameInfo.Level;
        int ro = GameInfo.gameInfo.Rooms;
        int re = GameInfo.gameInfo.Requests;
        levelInfo.text = $"Level {l} >> {l + 1}";
        otherInfo.text = $"활성화된 객실 수 {ro} >> {ro + 1}\n활성화된 파견 수 {re} >> {re + 1}\n모험가 새로고침 확률 변경";
    }

    public void OnClickYes() {
        Debug.Log("클릭");
        if (GameInfo.gameInfo.OnClickLevelUpYes()) OnClickCloseButton();
    }
    public void OnClickNo() {
        OnClickCloseButton();
    }
}
