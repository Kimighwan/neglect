using TMPro;
using UnityEngine.UI;

public class RoomUI : BaseUI
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI level;
    public TextMeshProUGUI other;
    public TextMeshProUGUI neededGold;
    public Button button;

    private int index; 
    private bool isActive;
    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        var indexData = uiData as RoomIndex;
        index = indexData.index;
        isActive = GameInfo.gameInfo.isRoomActivated(index);
    }

    void Start()
    {
        if (!isActive) {
            title.text = "객실 개방";
            level.text = "";
            other.text = "모험가 수 +2\n하루 수익 +300";
            neededGold.text = "필요 골드 1000";
        }
        else {
            title.text = "객실 레벨업";
            int l = GameInfo.gameInfo.GetRoomLevel(index);
            if (l == 1) {
                level.text = "1 >> 2";
                other.text = "모험가 수 2 >> 4\n하루 수익 300 >> 1000";
                neededGold.text = "필요 골드 3000";
            }
            else if (l == 2) {
                level.text = "2 >> 3";
                other.text = "모험가 수 4 >> 6\n하루 수익 1000 >> 4000";
                neededGold.text = "필요 골드 10000";
            }
            else if (l == 3) {
                title.text = "";
                level.text = "최고 등급 객실";
                neededGold.text = "추가 레벨업X";
                button.interactable = false;
            }
            else OnClickCloseButton();
        }
    }

    public void OnClickActivateRoom() {
        GameInfo.gameInfo.RoomActive(index);
    }

    public void OnClickLevelUpButton() {
        if (!isActive) OnClickActivateRoom();
        else GameInfo.gameInfo.RoomLevelUp(index);
    }

    public override void OnClickCloseButton()
    {
        base.OnClickCloseButton();
        Destroy(this);
    }
}
