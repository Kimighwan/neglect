using TMPro;
using UnityEngine.UI;

public class LevelUpUI : BaseUI
{
    public TextMeshProUGUI levelInfo;
    public TextMeshProUGUI otherInfo;
    public TextMeshProUGUI levelUpInfo;
    public Button levelUpButton;

    private void OnEnable() {
        int l = GameInfo.gameInfo.Level;
        if (l == 5) {
            levelInfo.text = "Max Level";
            otherInfo.text = "";
            levelUpInfo.text = "-";
            levelUpButton.interactable = false;
        }
        else {
            levelInfo.text = $"Level {l} >> {l + 1}";
            if (l == 4) {
                otherInfo.text = $"객실 수 {l} >> {l}\n파견 수 {l + 1} >> {l + 1}\n의뢰 수 {(l + 2) * 2} >> {(l + 3) * 2}\n모험가 새로고침 확률 변경";
                levelUpInfo.text = "필요 골드 50000G";
            }
            else {
                otherInfo.text = $"객실 수 {l} >> {l + 1}\n파견 수 {l + 1} >> {l + 2}\n의뢰 수 {(l + 2) * 2} >> {(l + 3) * 2}\n모험가 새로고침 확률 변경";
                if (l == 1) levelUpInfo.text = "필요 골드 500G";
                else if (l == 2) levelUpInfo.text = "필요 골드 3000G";
                else if (l == 3) levelUpInfo.text = "필요 골드 10000G";
            }
        }
    }

    public void OnClickLevelUpButton() {
        if (GameInfo.gameInfo.OnClickLevelUpYes()) {
            OnClickCloseButton();
            AudioManager.Instance.PlaySFX(SFX.LevelUp);
            UIManager.Instance.OpenSimpleInfoUI("길드 레벨 업!");
        }
    }
}
