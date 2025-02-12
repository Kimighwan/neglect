using Gpm.Ui;
using System.Linq;
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

    private void Update()
    {
        if (AdventureData.adventureSelectId.Contains(adventureid))
            checkImage.SetActive(true);
        else
            checkImage.SetActive(false);
    }


    public void OnClickAdventureBtn()
    {
        if (AdventureData.adventureSelectId.Contains(adventureid))
        {
            AdventureData.adventureSelectId.Remove(adventureid);
            return;
        }

        if (AdventureData.adventureSelectId.Count == 4)
        {
            AdventureData.adventureSelectId.RemoveAt(0);
        }

        AdventureData.adventureSelectId.Add(adventureid);
    }

    public void OnClickSelectBtn()
    {
        if (QuestData.questSelectedId == 0)
        {
            return;
        }
    }
}
