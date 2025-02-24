using Gpm.Ui;
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

    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI positionTxt;
    public TextMeshProUGUI classTxt;
    public TextMeshProUGUI typeTxt;

    public GameObject exportGameObject;

    private AdventureData adventureData;

    private int adventureId;
    private string adventurePosition;
    private string adventureType;
    private string adventureClass;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        adventureData = scrollData as AdventureData;

        if (adventureData == null) return;

        adventureId = adventureData.adventureId;
        var adventureName = adventureData.adventureName;
        adventurePosition = adventureData.adventurePosition;
        adventureType = adventureData.adventureType;
        var adventureTier = adventureData.adventureTier;
        adventureClass = adventureData.adventureClass;

        nameTxt.text = adventureName;
        positionTxt.text = adventurePosition;
        classTxt.text = adventureClass;
        typeTxt.text = adventureType;

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
        if (adventureClass == "물리")
            typeImg.texture = Resources.Load("Arts/Icon/Type/PhysicIcon") as Texture2D;
        else if (adventurePosition == "마법")
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
    }


    public void OnClick()
    {
        Debug.Log($"{adventureId}");
    }


    public void OnClickExportGameObjectUI()
    {
        exportGameObject.SetActive(true);

        UIManager.Instance.exportAdventureId = adventureId;

        //var adventureExportUI = new BaseUIData();
        //UIManager.Instance.OpenUI<AdventureExportUI>(adventureExportUI);

        //var ui = UIManager.Instance.GetActiveUI<AdventureExportUI>();

        //ui.gameObject.transform.SetParent(this.transform);

        //var rectTransform = ui.GetComponent<RectTransform>();
        //rectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
        //rectTransform.sizeDelta = new Vector2(110f, 110f);
    }


    //public void PositionMouseOnUI()
    //{
    //    UIManager.Instance.advemtureDetailDescText = adventurePosition;
    //    var advemtureDetailDescUI = new BaseUIData();
    //    UIManager.Instance.OpenUI<AdvemtureDetailDescUI>(advemtureDetailDescUI);

    //    var ui = UIManager.Instance.GetActiveUI<AdvemtureDetailDescUI>();
    //    ui.transform.SetParent(positionPos);
    //    ui.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(50f, 0f, 0f);
    //}

    public void PositionMouseOffUI()
    {
        UIManager.Instance.CloseUI(UIManager.Instance.GetActiveUI<AdvemtureDetailDescUI>());
    }


    //public void ClassMouseOnUI()
    //{
    //    UIManager.Instance.advemtureDetailDescText = adventureClass;
    //    var advemtureDetailDescUI = new BaseUIData();
    //    UIManager.Instance.OpenUI<AdvemtureDetailDescUI>(advemtureDetailDescUI);

    //    var ui = UIManager.Instance.GetActiveUI<AdvemtureDetailDescUI>();
    //    ui.transform.SetParent(classPos);
    //    ui.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(50f, 0f, 0f);
    //}


    public void ClassMouseOffUI()
    {
        UIManager.Instance.CloseUI(UIManager.Instance.GetActiveUI<AdvemtureDetailDescUI>());
    }


    //public void TypeMouseOnUI()
    //{
    //    UIManager.Instance.advemtureDetailDescText = adventureType;
    //    var advemtureDetailDescUI = new BaseUIData();
    //    UIManager.Instance.OpenUI<AdvemtureDetailDescUI>(advemtureDetailDescUI);

    //    var ui = UIManager.Instance.GetActiveUI<AdvemtureDetailDescUI>();
    //    ui.transform.SetParent(typePos);
    //    ui.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(50f, 0f, 0f);
    //}


    public void TypeMouseOffUI()
    {
        UIManager.Instance.CloseUI(UIManager.Instance.GetActiveUI<AdvemtureDetailDescUI>());
    }
}
