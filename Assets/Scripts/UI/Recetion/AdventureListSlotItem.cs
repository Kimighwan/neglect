using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdventureListSlotItem : InfiniteScrollItem
{
    public TextMeshProUGUI m_name;
    public TextMeshProUGUI position;
    public TextMeshProUGUI type;
    public TextMeshProUGUI m_class;

    public Transform positionPos;
    public Transform classPos;
    public Transform typePos;


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

        m_name.text = adventureName;
        position.text = adventurePosition;
        type.text = adventureType;
        m_class.text = adventureClass;
    }

    public void OnClick()
    {
        Debug.Log($"{adventureId}");
    }

    public void OpenExportBtnUI()
    {
        var adventureExportUI = new BaseUIData();
        UIManager.Instance.OpenUI<AdventureExportUI>(adventureExportUI);

        var ui = UIManager.Instance.GetActiveUI<AdventureExportUI>();

        ui.gameObject.transform.SetParent(this.transform);

        var rectTransform = ui.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
        rectTransform.sizeDelta = new Vector2(110f, 110f);
    }

    public void PositionMouseOnUI()
    {
        UIManager.Instance.advemtureDetailDescText = adventurePosition;
        var advemtureDetailDescUI = new BaseUIData();
        UIManager.Instance.OpenUI<AdvemtureDetailDescUI>(advemtureDetailDescUI);

        var ui = UIManager.Instance.GetActiveUI<AdvemtureDetailDescUI>();
        ui.transform.SetParent(positionPos);
        ui.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(50f, 0f, 0f);
    }

    public void PositionMouseOffUI()
    {
        UIManager.Instance.CloseUI(UIManager.Instance.GetActiveUI<AdvemtureDetailDescUI>());
    }

    public void ClassMouseOnUI()
    {
        UIManager.Instance.advemtureDetailDescText = adventureClass;
        var advemtureDetailDescUI = new BaseUIData();
        UIManager.Instance.OpenUI<AdvemtureDetailDescUI>(advemtureDetailDescUI);

        var ui = UIManager.Instance.GetActiveUI<AdvemtureDetailDescUI>();
        ui.transform.SetParent(classPos);
        ui.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(50f, 0f, 0f);
    }

    public void ClassMouseOffUI()
    {
        UIManager.Instance.CloseUI(UIManager.Instance.GetActiveUI<AdvemtureDetailDescUI>());
    }

    public void TypeMouseOnUI()
    {
        UIManager.Instance.advemtureDetailDescText = adventureType;
        var advemtureDetailDescUI = new BaseUIData();
        UIManager.Instance.OpenUI<AdvemtureDetailDescUI>(advemtureDetailDescUI);

        var ui = UIManager.Instance.GetActiveUI<AdvemtureDetailDescUI>();
        ui.transform.SetParent(typePos);
        ui.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(50f, 0f, 0f);
    }

    public void TypeMouseOffUI()
    {
        UIManager.Instance.CloseUI(UIManager.Instance.GetActiveUI<AdvemtureDetailDescUI>());
    }
}
