using Gpm.Ui;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;



public class DetachQuestListUI : BaseUI
{
    public InfiniteScroll infiniteScrollList;

    public TextMeshProUGUI sortBtnText;
    public TextMeshProUGUI orderBtnText;


    public int qusetIndex;     // 파견에서 몇번째 파견창인지


    private QuestSortType questSortType = QuestSortType.Level;
    private QuestOrderType questOrderType = QuestOrderType.DOWN;


    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        var questIndexClass = uiData as QuestIndex;
        qusetIndex = questIndexClass.index;

        SortQuest();
        infiniteScrollList.layout.space = new Vector2(10f, 10f);
    }

    private void OnEnable()
    {
        PoolManager.Instance.SetDetachQuestListData();
        SetScroll();
    }

    private void OnDisable()
    {
        QuestData.questSelectedId = 0;
    }

    private void SetScroll()
    {
        infiniteScrollList.Clear();

        foreach (var questData in PoolManager.Instance.userQuestList)
        {
            var slotData = new QuestData();

            slotData.questId = questData.questId;
            slotData.questName = questData.questName;
            slotData.questLevel = questData.questLevel;
            slotData.questTime = questData.questTime;
            slotData.questReward = questData.questReward;
            slotData.questMonsterDescId = questData.questMonsterDescId;
            slotData.questMonster = questData.questMonster;

            infiniteScrollList.InsertData(slotData);
        }
    }

    private void SortQuest()
    {
        switch (questSortType)
        {
            case QuestSortType.Level:
                sortBtnText.text = "LEVEL";

                infiniteScrollList.SortDataList((a, b) =>
                {
                    int compareResult = 0;

                    var itemA = a.data as QuestData;
                    var itemB = b.data as QuestData;

                    // 내림차순
                    if (questOrderType == QuestOrderType.DOWN)
                    {
                        orderBtnText.text = "DOWN";
                        compareResult = ((itemB.questId / 1000) % 10).CompareTo((itemA.questId / 1000) % 10);
                    }

                    // 오름차순
                    if (questOrderType == QuestOrderType.UP)
                    {
                        orderBtnText.text = "UP";
                        compareResult = ((itemA.questId / 1000) % 10).CompareTo((itemB.questId / 1000) % 10);
                    }

                    if (compareResult == 0)
                    {
                        var itemAId = itemA.questId;

                        var itemBId = itemB.questId;

                        compareResult = itemAId.CompareTo((itemBId));
                    }

                    return compareResult;
                });
                break;
            default:
                break;
        }
    }

    // 정렬 버튼
    public void OnClickSortBtn()
    {
        switch (questSortType)
        {
            case QuestSortType.Level:
                questSortType = QuestSortType.Level;
                break;
            default:
                break;
        }

        SortQuest();
    }

    // 순서 선택 버튼
    public void OnClickOrderBtn()
    {
        switch (questOrderType)
        {
            case QuestOrderType.DOWN:
                questOrderType = QuestOrderType.UP;
                break;
            case QuestOrderType.UP:
                questOrderType = QuestOrderType.DOWN;
                break;
            default:
                break;
        }

        SortQuest();
    }


    public void OnClickSelectBtn()
    {
        // 의뢰 처리 하는 곳에서 questData를 넘겨준다
        // 의뢰 처리 하는 곳에서 모험가도 받게 될 것인데
        // 위 두 개의 정보를 가지고 의뢰 시스템이 작동한다
        if(QuestData.questSelectedId == 0)
        {
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK;
            uiData.descTxt = "의뢰를 다시 선택하십시오.";
            uiData.okBtnTxt = "확인";
            AudioManager.Instance.PlaySFX(SFX.Denied);
            UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            return;
        }

        PoolManager.Instance.questBtn[qusetIndex - 1].interactable = false;

        // 버튼 Text 업데이트
        PoolManager.Instance.questTxt[qusetIndex - 1].text = PoolManager.Instance.questData[qusetIndex].questName;
        if (PoolManager.Instance.questTxt[qusetIndex - 1].text.Length > 6)
        {
            string tmp = PoolManager.Instance.questTxt[qusetIndex - 1].text.Substring(0, 5) + "...";
            PoolManager.Instance.questTxt[qusetIndex - 1].text = tmp;
        }

        PoolManager.Instance.questManagers[qusetIndex - 1].nameTxt.text = PoolManager.Instance.questData[qusetIndex].questName;
        PoolManager.Instance.questManagers[qusetIndex - 1].rankTxt.text = PoolManager.Instance.questData[qusetIndex].questLevel;
        PoolManager.Instance.questManagers[qusetIndex - 1].timeTxt.text = PoolManager.Instance.questData[qusetIndex].questTime.ToString();
        PoolManager.Instance.questManagers[qusetIndex - 1].rewardTxt.text = PoolManager.Instance.questData[qusetIndex].questReward.ToString();

        if (PoolManager.Instance.questData[qusetIndex].questLevel == "브론즈")
            PoolManager.Instance.questManagers[qusetIndex - 1].rankImg.texture = Resources.Load("Arts/QuestRank/bronze_quest") as Texture2D;
        else if (PoolManager.Instance.questData[qusetIndex].questLevel == "실버")
            PoolManager.Instance.questManagers[qusetIndex - 1].rankImg.texture = Resources.Load("Arts/QuestRank/silver_quest") as Texture2D;
        else if (PoolManager.Instance.questData[qusetIndex].questLevel == "골드")
            PoolManager.Instance.questManagers[qusetIndex - 1].rankImg.texture = Resources.Load("Arts/QuestRank/gold_quest") as Texture2D;
        else if (PoolManager.Instance.questData[qusetIndex].questLevel == "플래티넘")
            PoolManager.Instance.questManagers[qusetIndex - 1].rankImg.texture = Resources.Load("Arts/QuestRank/platinum_quest") as Texture2D;
        else if (PoolManager.Instance.questData[qusetIndex].questLevel == "다이아")
            PoolManager.Instance.questManagers[qusetIndex - 1].rankImg.texture = Resources.Load("Arts/QuestRank/diamond_quest") as Texture2D;

        PoolManager.Instance.UsingQuestData();
        UIManager.Instance.CloseUI(UIManager.Instance.GetActiveUI<DetachQuestListUI>());
    }
}
