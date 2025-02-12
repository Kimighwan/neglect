using Gpm.Ui;
using TMPro;
using UnityEngine;

public class DetachAdventureListUI : BaseUI
{
    public InfiniteScroll infiniteScrollList;

    public TextMeshProUGUI sortBtnText;
    public TextMeshProUGUI orderBtnText;


    private AdventureSortType adventureSortType = AdventureSortType.GRADE;
    private AdventureOrderType adventureOrderType = AdventureOrderType.DOWN;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        SortAdventure();
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

    private void SortAdventure()
    {
        switch (adventureSortType)
        {
            case AdventureSortType.GRADE:
                sortBtnText.text = "GRADE";

                infiniteScrollList.SortDataList((a, b) =>
                {
                    int compareResult = 0;

                    var itemA = a.data as AdventureData;
                    var itemB = b.data as AdventureData;

                    // 내림차순
                    if (adventureOrderType == AdventureOrderType.DOWN)
                    {
                        compareResult = ((itemB.adventureId / 1000) % 10).CompareTo((itemA.adventureId / 1000) % 10);
                    }

                    // 오름차순
                    if (adventureOrderType == AdventureOrderType.UP)
                    {
                        compareResult = ((itemA.adventureId / 1000) % 10).CompareTo((itemB.adventureId / 1000) % 10);
                    }

                    if (compareResult == 0)
                    {
                        var itemAName = itemA.adventureName;

                        var itemBName = itemB.adventureName;

                        compareResult = itemAName.CompareTo((itemBName));
                    }

                    return compareResult;
                });
                break;
            case AdventureSortType.POSITION:
                sortBtnText.text = "POSITION";

                infiniteScrollList.SortDataList((a, b) =>
                {
                    int compareResult = 0;

                    var itemA = a.data as AdventureData;
                    var itemB = b.data as AdventureData;

                    // 내림차순
                    if (adventureOrderType == AdventureOrderType.DOWN)
                    {
                        compareResult = ((itemB.adventureId / 100) % 10).CompareTo((itemA.adventureId / 100) % 10);
                    }

                    // 오름차순
                    if (adventureOrderType == AdventureOrderType.UP)
                    {
                        compareResult = ((itemA.adventureId / 100) % 10).CompareTo((itemB.adventureId / 100) % 10);
                    }

                    if (compareResult == 0)
                    {
                        var itemAName = itemA.adventureName;

                        var itemBName = itemB.adventureName;

                        compareResult = itemAName.CompareTo((itemBName));
                    }

                    return compareResult;
                });
                break;
            case AdventureSortType.CLASS:
                sortBtnText.text = "CLASS";

                infiniteScrollList.SortDataList((a, b) =>
                {
                    int compareResult = 0;

                    var itemA = a.data as AdventureData;
                    var itemB = b.data as AdventureData;

                    // 내림차순
                    if (adventureOrderType == AdventureOrderType.DOWN)
                    {
                        compareResult = ((itemB.adventureId / 10) % 10).CompareTo((itemA.adventureId / 10) % 10);
                    }

                    // 오름차순
                    if (adventureOrderType == AdventureOrderType.UP)
                    {
                        compareResult = ((itemA.adventureId / 10) % 10).CompareTo((itemB.adventureId / 10) % 10);
                    }

                    if (compareResult == 0)
                    {
                        var itemAName = itemA.adventureName;

                        var itemBName = itemB.adventureName;

                        compareResult = itemAName.CompareTo((itemBName));
                    }

                    return compareResult;
                });
                break;
            case AdventureSortType.TYPE:
                sortBtnText.text = "TYPE";

                infiniteScrollList.SortDataList((a, b) =>
                {
                    int compareResult = 0;

                    var itemA = a.data as AdventureData;
                    var itemB = b.data as AdventureData;

                    // 내림차순
                    if (adventureOrderType == AdventureOrderType.DOWN)
                    {
                        compareResult = (itemB.adventureId % 10).CompareTo(itemA.adventureId % 10);
                    }

                    // 오름차순
                    if (adventureOrderType == AdventureOrderType.UP)
                    {
                        compareResult = (itemA.adventureId % 10).CompareTo(itemB.adventureId % 10);
                    }

                    if (compareResult == 0)
                    {
                        var itemAName = itemA.adventureName;

                        var itemBName = itemB.adventureName;

                        compareResult = itemAName.CompareTo((itemBName));
                    }

                    return compareResult;
                });
                break;
            default:
                break;
        }

        OrderTextUpdate();
    }

    // 정렬 버튼
    public void OnClickSortBtn()
    {
        switch (adventureSortType)
        {
            case AdventureSortType.GRADE:
                adventureSortType = AdventureSortType.POSITION;
                break;
            case AdventureSortType.POSITION:
                adventureSortType = AdventureSortType.CLASS;
                break;
            case AdventureSortType.CLASS:
                adventureSortType = AdventureSortType.TYPE;
                break;
            case AdventureSortType.TYPE:
                adventureSortType = AdventureSortType.GRADE;
                break;
            default:
                break;
        }

        SortAdventure();
    }

    // 순서 선택 버튼
    public void OnClickOrderBtn()
    {
        switch (adventureOrderType)
        {
            case AdventureOrderType.DOWN:
                adventureOrderType = AdventureOrderType.UP;
                orderBtnText.text = "UP";
                break;
            case AdventureOrderType.UP:
                adventureOrderType = AdventureOrderType.DOWN;
                orderBtnText.text = "DOWN";
                break;
            default:
                break;
        }

        SortAdventure();
    }

    private void OrderTextUpdate()
    {
        switch (adventureOrderType)
        {
            case AdventureOrderType.DOWN:
                orderBtnText.text = "DOWN";
                break;
            case AdventureOrderType.UP:
                orderBtnText.text = "UP";
                break;
            default:
                break;
        }
    }
}
