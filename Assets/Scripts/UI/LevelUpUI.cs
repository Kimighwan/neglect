using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : BaseUI
{
    public TextMeshProUGUI levelInfo;
    public TextMeshProUGUI otherInfo;
    public Image bar;
    private float fill = 0f;

    private void Start() {
        int l = GameInfo.gameInfo.Level;
        int ro = GameInfo.gameInfo.Rooms;
        int re = GameInfo.gameInfo.Requests;
        levelInfo.text = $"Level {l - 1} >> {l}";
        otherInfo.text = $"활성화된 객실 수 {ro - 1} >> {ro}\n활성화된 파견 수 {re - 1} >> {re}\n모험가 새로고침 확률 변경";
        bar.fillAmount = fill;
    }

    private void Update() {
        if (fill >= 1f) OnClickCloseButton();
        fill += Time.deltaTime * 0.2f;
        bar.fillAmount = fill;
    }
}
