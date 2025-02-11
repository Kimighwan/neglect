using Gpm.Ui;
using UnityEngine;

public class DetachAdventureListUI : BaseUI
{
    public InfiniteScroll infiniteScrollList;

    public override void Init(Transform anchor)
    {
        base.Init(anchor);
    }

    private void OnEnable()
    {
        PoolManager.Instance.SetAdventureListData();
        SetScroll();
    }

    private void SetScroll()
    {
        infiniteScrollList.Clear();

        foreach(var adventureData in PoolManager.Instance.userAdventureList)
        {
            var slotData = new AdventureData();

            slotData.adventureId = adventureData.adventureId;
            slotData.adventureName = adventureData.adventureName;
            slotData.adventurePosition = adventureData.adventurePosition;
            slotData.adventureClass = adventureData.adventureClass;
            slotData.adventureType = adventureData.adventureType;
            slotData.adventureTier = adventureData.adventureTier;

            infiniteScrollList.InsertData(slotData);
        }
    }
}
