using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachAdventureListUI : BaseUI
{
    public InfiniteScroll infiniteScrollList;

    public Transform pos;   // 리스트에 해당하는 각각의 아이템 부모위치

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
