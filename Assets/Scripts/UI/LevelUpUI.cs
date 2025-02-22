using TMPro;
using UnityEngine.UI;

public class LevelUpUI : BaseUI
{
    public TextMeshProUGUI levelInfo;
    public TextMeshProUGUI otherInfo;
    public Button levelUpButton;

    private void OnEnable() {
        int l = GameInfo.gameInfo.Level;
        if (l == 5) {
            levelInfo.text = "Max Level";
            otherInfo.text = "";
            levelUpButton.interactable = false;
        }
        else {
            levelInfo.text = $"Level {l} >> {l + 1}";
            if (l == 4) otherInfo.text = $"객실 수 {l} >> {l}\n파견 수 {l + 1} >> {l + 1}\n의뢰 수 {(l + 2) * 2} >> {(l + 3) * 2}\n모험가 새로고침 확률 변경";
            else otherInfo.text = $"객실 수 {l} >> {l + 1}\n파견 수 {l + 1} >> {l + 2}\n의뢰 수 {(l + 2) * 2} >> {(l + 3) * 2}\n모험가 새로고침 확률 변경";
        }
    }

    public void OnClickYes() {
        if (GameInfo.gameInfo.OnClickLevelUpYes()) OnClickCloseButton();
    }
    public void OnClickNo() {
        OnClickCloseButton();
    }
}
