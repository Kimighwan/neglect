using Gpm.Ui;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    private AdventureData adventureData;

    // Export
    public TextMeshProUGUI curStateTxt;
    public TextMeshProUGUI exportGold;

    private int adventureId;
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
        classTxt.text = adventureClass;
        typeTxt.text = adventureType;

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
        else if (adventurePosition == "방어")
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

        if (adventureTier == "브론즈") exportGold.text = "+G 40";
        else if(adventureTier == "실버") exportGold.text = "+G 100";
        else if (adventureTier == "골드") exportGold.text = "+G 200";
        else if (adventureTier == "플래티넘") exportGold.text = "+G 400";
        else if (adventureTier == "다이아") exportGold.text = "+G 1000";
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
            uiData.okBtnTxt = "삭제";
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
            GameInfo.gameInfo.ChangeGold(40);
            GameInfo.gameInfo.CalculateTodayGold(40);
        }
        else if (adventureTier == "실버")
        {
            PoolManager.Instance.silverAd--;
            GameInfo.gameInfo.ChangeGold(100);
            GameInfo.gameInfo.CalculateTodayGold(100);
        }
        else if (adventureTier == "골드")
        {
            PoolManager.Instance.goldAd--;
            GameInfo.gameInfo.ChangeGold(200);
            GameInfo.gameInfo.CalculateTodayGold(200);
        }
        else if (adventureTier == "플래티넘")
        {
            GameInfo.gameInfo.ChangeGold(400);
            GameInfo.gameInfo.CalculateTodayGold(400);
            PoolManager.Instance.platinumAd--;
        }
        else
        {
            GameInfo.gameInfo.ChangeGold(1000);
            GameInfo.gameInfo.CalculateTodayGold(1000);
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
