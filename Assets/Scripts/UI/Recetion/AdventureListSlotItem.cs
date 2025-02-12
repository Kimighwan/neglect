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

        var rectTransform = GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(160f, 160f);

        adventureData = scrollData as AdventureData;

        if (adventureData == null) return;

        adventureId = adventureData.adventureId;
        var adventureName = adventureData.adventureName;
        var adventurePosition = adventureData.adventurePosition;
        var adventureType = adventureData.adventureType;
        var adventureTier = adventureData.adventureTier;

        m_name.text = adventureName;
        position.text = adventurePosition;
        type.text = adventureType;
        m_class.text = adventureType;
    }

    public void OnClick()
    {

    }
}
