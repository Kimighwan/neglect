using Gpm.Ui;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DetachAdventureListUI : BaseUI
{
    public InfiniteScroll infiniteScrollList;

    public TextMeshProUGUI sortBtnText;
    public TextMeshProUGUI orderBtnText;
    public TextMeshProUGUI countTxt;
    public TextMeshProUGUI iconRate;                // 성공 확률

    public GameObject iconOB;
    public RawImage iconImage;                      // 아이콘 이미지

    [SerializeField] public int adventureIndex;     // 파견에서 몇번째 파견창인지

    private AdventureSortType adventureSortType = AdventureSortType.GRADE;
    private AdventureOrderType adventureOrderType = AdventureOrderType.DOWN;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        var adventureIndexClass = uiData as AdventureIndexClass;
        adventureIndex = adventureIndexClass.index;

        SortAdventure();
        infiniteScrollList.layout.space = new Vector2(10f, 10f);
    }

    private void OnEnable()
    {
        PoolManager.Instance.SetDetachAdventureListData();
        SetScroll();
    }

    private void OnDisable()
    {
        AdventureData.adventureSelectId.Clear();
    }

    private void Update()
    {
        if(adventureIndex < 10)
        {
            countTxt.text
                        = PoolManager.Instance.questManagers[adventureIndex - 1].adventureDatas.Count.ToString()
                        + "/4";

            if(PoolManager.Instance.questManagers[adventureIndex - 1].adventureDatas.Count == 0 || !PoolManager.Instance.questData.ContainsKey(adventureIndex))
            {
                iconOB.SetActive(false);
                iconRate.text = "";
            }
            else
            {
                if (PoolManager.Instance.questManagers[adventureIndex - 1].iconColor == 1)
                {
                    iconImage.texture = Resources.Load("Arts/Icon/IconFaceHard") as Texture2D;
                    iconRate.text = "성공확률 90% 미만";
                }
                else if (PoolManager.Instance.questManagers[adventureIndex - 1].iconColor == 2)
                {
                    iconImage.texture = Resources.Load("Arts/Icon/IconAdd") as Texture2D;
                    iconRate.text = "성공확률 90% ~ 100%";
                }
                else if (PoolManager.Instance.questManagers[adventureIndex - 1].iconColor == 3)
                {
                    iconImage.texture = Resources.Load("Arts/Icon/IconFaceNormal") as Texture2D;
                    iconRate.text = "대성공확률 0% ~ 10%";
                }
                else if (PoolManager.Instance.questManagers[adventureIndex - 1].iconColor == 4)
                {
                    iconImage.texture = Resources.Load("Arts/Icon/IconFaceEasy") as Texture2D;
                    iconRate.text = "대성공확률 10% 초과";
                }

                iconOB.SetActive(true);
            }
        }
        else
        {
            var tmp = UIManager.Instance.GetActiveUI<EmergencyQuestUI>() as EmergencyQuestUI;
            var questManagetOfCurEmergency = tmp.GetComponent<QuestManager>();
            countTxt.text = questManagetOfCurEmergency.adventureDatas.Count.ToString() + "/4";

            if (questManagetOfCurEmergency.adventureDatas.Count == 0)
            {
                iconOB.SetActive(false);
                iconRate.text = "";
            }
            else 
            {
                if (questManagetOfCurEmergency.iconColor == 1)
                {
                    iconImage.texture = Resources.Load("Arts/Icon/IconFaceHard") as Texture2D;
                    iconRate.text = "성공확률 90% 미만";
                }
                else if (questManagetOfCurEmergency.iconColor == 2)
                {
                    iconImage.texture = Resources.Load("Arts/Icon/IconAdd") as Texture2D;
                    iconRate.text = "성공확률 90% ~ 100%";
                }
                else if (questManagetOfCurEmergency.iconColor == 3)
                {
                    iconImage.texture = Resources.Load("Arts/Icon/IconFaceNormal") as Texture2D;
                    iconRate.text = "대성공확률 0% ~ 10%";
                }
                else if (questManagetOfCurEmergency.iconColor == 4)
                {
                    iconImage.texture = Resources.Load("Arts/Icon/IconFaceEasy") as Texture2D;
                    iconRate.text = "대성공확률 10% 초과";
                }

                iconOB.SetActive(true);
            }
        }
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
                    if (adventureOrderType == AdventureOrderType.UP)
                    {
                        compareResult = ((itemB.adventureId / 1000) % 10).CompareTo((itemA.adventureId / 1000) % 10);
                    }

                    // 오름차순
                    if (adventureOrderType == AdventureOrderType.DOWN)
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
                    if (adventureOrderType == AdventureOrderType.UP)
                    {
                        compareResult = ((itemB.adventureId / 100) % 10).CompareTo((itemA.adventureId / 100) % 10);
                    }

                    // 오름차순
                    if (adventureOrderType == AdventureOrderType.DOWN)
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
                    if (adventureOrderType == AdventureOrderType.UP)
                    {
                        compareResult = ((itemB.adventureId / 10) % 10).CompareTo((itemA.adventureId / 10) % 10);
                    }

                    // 오름차순
                    if (adventureOrderType == AdventureOrderType.DOWN)
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
                    if (adventureOrderType == AdventureOrderType.UP)
                    {
                        compareResult = (itemB.adventureId % 10).CompareTo(itemA.adventureId % 10);
                    }

                    // 오름차순
                    if (adventureOrderType == AdventureOrderType.DOWN)
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


    public void OnClickSelectBtn()  // 선택 완료 버튼
    {
        // 의뢰 처리 하는 곳에서 adventureData를 넘겨준다
        // 의뢰 처리 하는 곳에서 의뢰도 받게 될 것인데
        // 위 두 개의 정보를 가지고 의뢰 시스템이 작동한다

        if (AdventureData.adventureSelectId.Count == 0)
        {
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK;
            uiData.descTxt = "모험가 한 명 이상\n선택하십시오.";
            uiData.okBtnTxt = "확인";
            UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            return;
        }

        if (adventureIndex < 10)
        {
            
            PoolManager.Instance.adventureBtn[adventureIndex - 1].interactable = false;
            PoolManager.Instance.adventureBtn[adventureIndex - 1].GetComponent<UIButtonAnimator>().SetUnActiveAnim(true);

            PoolManager.Instance.adventureTxt[adventureIndex - 1].text = "선택 완료";

            Common();
        }

        if (adventureIndex > 10)
        {
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK_CANCEL;
            uiData.descTxt = "경고! 모험가 선택을 완료하면 자동으로 의뢰가 시작됩니다. 완료하시겠습니까?";
            uiData.okBtnTxt = "확인";
            uiData.cancelBtnTxt = "재선택";
            uiData.onClickOKBtn = () =>
            {
                Common();

                var ui = UIManager.Instance.GetActiveUI<EmergencyQuestUI>() as EmergencyQuestUI;
                ui.OnClickStartBtn();
            };
            UIManager.Instance.OpenUI<ConfirmUI>(uiData);
        }
    }

    private void Common()
    {
        PoolManager.Instance.UsingAdventureData();
        UIManager.Instance.CloseUI(UIManager.Instance.GetActiveUI<DetachAdventureListUI>());
    }

    public override void OnClickCloseButton()
    {
        base.OnClickCloseButton();

        if (adventureIndex < 10)
            PoolManager.Instance.questManagers[adventureIndex - 1].adventureDatas.Clear();
        else
        {
            var tmp = UIManager.Instance.GetActiveUI<EmergencyQuestUI>() as EmergencyQuestUI;
            tmp.GetComponent<QuestManager>().adventureDatas.Clear();
        }
    }

}
