using Gpm.Ui;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdventureListSlotItem : InfiniteScrollItem
{
    public TextMeshProUGUI m_name;

    public RawImage positionImg;
    public RawImage classImg;
    public RawImage typeImg;
    public RawImage rankImg;
    public RawImage stateImg;

    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI positionTxt;
    public TextMeshProUGUI classTxt;
    public TextMeshProUGUI typeTxt;
    public TextMeshProUGUI stateTxt;

    public GameObject exportGameObject;

    [SerializeField]
    private AdventureListSlotItemPosOverRay pos;
    [SerializeField]
    private AdventureListSlotItemPosOverRay m_class;
    [SerializeField]
    private AdventureListSlotItemPosOverRay type;

    private AdventureData adventureData;
    [SerializeField]
    private TextMeshProUGUI rankTxt;

    // Export
    public TextMeshProUGUI curStateTxt;
    public TextMeshProUGUI exportGoldTxt;

    private int adventureId;

    private int exportGold;

    private string adventurePosition;
    private string adventureType;
    private string adventureClass;
    private string adventureTier;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        adventureData = scrollData as AdventureData;

        if (adventureData == null) return;

        adventureId = adventureData.adventureId;
        var adventureName = adventureData.adventureName;
        adventurePosition = adventureData.adventurePosition;
        adventureType = adventureData.adventureType;
        adventureTier = adventureData.adventureTier;
        adventureClass = adventureData.adventureClass;

        nameTxt.text = adventureName;
        positionTxt.text = adventurePosition;
        pos.pos = adventurePosition;        // 오버레이 Position Text 초기화
        classTxt.text = adventureClass;
        m_class.m_class = adventureClass;   // 오버레이 Class Text 초기화
        typeTxt.text = adventureType;
        type.type = adventureType;          // 오버레이 Type Text 초기화

        if (PoolManager.Instance.usingAdventureList.Contains(adventureId))
        {
            stateTxt.text = "파견 중";
            stateImg.texture = Resources.Load("Arts/Icon/arrived") as Texture2D;
        }
        else
        {
            stateTxt.text = "대기 중";
            stateImg.texture = Resources.Load("Arts/Icon/waiting") as Texture2D;
        }
            

        // Postion Image
        if (adventurePosition == "전위")
            positionImg.texture = Resources.Load("Arts/Icon/Pos/FrontIcon") as Texture2D;
        else if(adventurePosition == "중위")
            positionImg.texture = Resources.Load("Arts/Icon/Pos/MiddleIcon") as Texture2D;
        else
            positionImg.texture = Resources.Load("Arts/Icon/Pos/BackIcon") as Texture2D;

        // Class Image
        if (adventureClass == "공격")
            classImg.texture = Resources.Load("Arts/Icon/Class/AttackIcon") as Texture2D;
        else if (adventureClass == "방어")
            classImg.texture = Resources.Load("Arts/Icon/Class/DefenceIcon") as Texture2D;
        else
            classImg.texture = Resources.Load("Arts/Icon/Class/SupportIcon") as Texture2D;

        // Type Image
        if (adventureType == "물리")
            typeImg.texture = Resources.Load("Arts/Icon/Type/PhysicIcon") as Texture2D;
        else if (adventureType == "마법")
            typeImg.texture = Resources.Load("Arts/Icon/Type/MagicIcon") as Texture2D;
        else
            typeImg.texture = Resources.Load("Arts/Icon/Type/HolyIcon") as Texture2D;

        // Rank Image
        if (adventureTier == "브론즈")
            rankImg.texture = Resources.Load("Arts/Rank/RankBronze") as Texture2D;
        else if (adventureTier == "실버")
            rankImg.texture = Resources.Load("Arts/Rank/RankSilver") as Texture2D;
        else if (adventureTier == "골드")
            rankImg.texture = Resources.Load("Arts/Rank/RankGold") as Texture2D;
        else if (adventureTier == "플래티넘")
            rankImg.texture = Resources.Load("Arts/Rank/RankPlatinum") as Texture2D;
        else
            rankImg.texture = Resources.Load("Arts/Rank/RankDiamond") as Texture2D;

        m_name.text = adventureName;

        SetStateText();

        if (adventureTier == "브론즈") {exportGoldTxt.text = "+G 50"; exportGold = 50; rankTxt.text = "브론즈"; }
        else if(adventureTier == "실버") {exportGoldTxt.text = "+G 250"; exportGold = 250; rankTxt.text = "실버"; }
        else if (adventureTier == "골드") {exportGoldTxt.text = "+G 500"; exportGold = 500; rankTxt.text = "골드"; }
        else if (adventureTier == "플래티넘") {exportGoldTxt.text = "+G 1000"; exportGold = 1000; rankTxt.text = "플래티넘"; }
        else if (adventureTier == "다이아") {exportGoldTxt.text = "+G 2500"; exportGold = 2500; rankTxt.text = "다이아"; }
    }

    private void SetStateText()
    {
        if (PoolManager.Instance.usingAdventureList.Contains(adventureId))
        {
            curStateTxt.text = "파견 중";
        }
        else
        {
            curStateTxt.text = "대기 중";
        }
    }


    public void OnClickExportBtn()
    {
        if (stateTxt.text == "파견 중")
        {
            // 파견 중이라면 못하게 막기
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK;
            uiData.descTxt = "파견 중입니다.";
            uiData.okBtnTxt = "확인";
            UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            return;
        }
        else
        {
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK_CANCEL;
            uiData.descTxt = "정말로 방출 하시겠습니까?";
            uiData.okBtnTxt = "+G" + exportGold.ToString();
            uiData.cancelBtnTxt = "아니요";
            uiData.onClickOKBtn = () =>
            {
                Export();
            };
            UIManager.Instance.OpenUI<ConfirmUI>(uiData);
        }
        
    }

    private void Export()
    {
        if (adventureTier == "브론즈")
        {
            PoolManager.Instance.bronzAd--;
            GameInfo.gameInfo.ChangeGold(50);
            GameInfo.gameInfo.CalculateTodayGold(50);
            GameInfo.gameInfo.addGold += 50;
        }
        else if (adventureTier == "실버")
        {
            PoolManager.Instance.silverAd--;
            GameInfo.gameInfo.ChangeGold(250);
            GameInfo.gameInfo.CalculateTodayGold(250);
            GameInfo.gameInfo.addGold += 250;
        }
        else if (adventureTier == "골드")
        {
            PoolManager.Instance.goldAd--;
            GameInfo.gameInfo.ChangeGold(500);
            GameInfo.gameInfo.CalculateTodayGold(500);
            GameInfo.gameInfo.addGold += 500;
        }
        else if (adventureTier == "플래티넘")
        {
            GameInfo.gameInfo.ChangeGold(1000);
            GameInfo.gameInfo.CalculateTodayGold(1000);
            GameInfo.gameInfo.addGold += 1000;
            PoolManager.Instance.platinumAd--;
        }
        else
        {
            GameInfo.gameInfo.ChangeGold(2500);
            GameInfo.gameInfo.CalculateTodayGold(2500);
            GameInfo.gameInfo.addGold += 2500;
            PoolManager.Instance.diaAd--;
        }

        // PlayerPrefs에 해당 모험가 제거
        DeleteAdventure();

        // 인피니티 스크롤 업데이트
        AdventurerListUI.Instance.UpdateScrollItem();
    }


    public void OnClickExportGameObjectUI()
    {
        exportGameObject.SetActive(true);

        UIManager.Instance.exportAdventureId = adventureId;
    }

    private void DeleteAdventure()
    {
        var adventureId = PlayerPrefs.GetString("AdventureId");
        var adventureIds = adventureId.Split(',');

        if (adventureId == "") return;

        string addId = "";

        foreach (var item in adventureIds)
        {
            int adventureIdOfInt = Convert.ToInt32(item);

            if (Convert.ToInt32(this.adventureId) != adventureIdOfInt)
            {
                if (addId == "")
                    addId += adventureIdOfInt.ToString();
                else
                    addId += "," + adventureIdOfInt.ToString();
            }
        }

        PlayerPrefs.SetString("AdventureId", addId);
    }
}
