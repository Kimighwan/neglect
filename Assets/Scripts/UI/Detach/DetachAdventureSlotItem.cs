using Gpm.Ui;
using TMPro;
using UnityEngine;

public class DetachAdventureSlotItem : InfiniteScrollItem
{
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtPosition;
    public TextMeshProUGUI txtClass;
    public TextMeshProUGUI txtType;

    public GameObject checkImage;

    private AdventureData adventureData;

    private int adventureid;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        var rectTransform = GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(160f, 160f);

        adventureData = scrollData as AdventureData;

        if (adventureData == null) return;

        adventureid = adventureData.adventureId;
        var adventureName = adventureData.adventureName;
        var adventurePosition = adventureData.adventurePosition;
        var adventureClass = adventureData.adventureClass;
        var adventureType = adventureData.adventureType;
        var adventureTier = adventureData.adventureTier;

        txtName.text = adventureName;
        txtPosition.text = adventurePosition;
        txtClass.text = adventureClass;
        txtType.text = adventureType;
    }

    public void OnClickSelect()
    {
        checkImage.SetActive(true);
    }
}
