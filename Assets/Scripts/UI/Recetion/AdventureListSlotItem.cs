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

    private AdventureData adventureData;

    private int adventureId;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        adventureData = scrollData as AdventureData;

        if (adventureData == null) return;

        adventureId = adventureData.adventureId;
        var adventureName = adventureData.adventureName;
        var adventurePosition = adventureData.adventurePosition;
        var adventureType = adventureData.adventureType;
        var adventureTier = adventureData.adventureTier;
        var adventureClass = adventureData.adventureClass;

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
}
