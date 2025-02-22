public class RoomUI : BaseUI
{
    private int index; 
    private bool isActive;
    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        var indexData = uiData as RoomIndex;
        index = indexData.index;
        isActive = indexData.isActive;
    }

    void Start()
    {
        if (!isActive) {
            // 객실 개방 창으로 띄우기
        }
    }

    public void OnClickActivateRoom() {
        GameInfo.gameInfo.RoomActive(index);
    }

    public void OnClickLevelUpButton() {
        GameInfo.gameInfo.RoomLevelUp(index);
    }
}
