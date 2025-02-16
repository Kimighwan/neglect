using Gpm.Ui;
using System.Collections.Generic;
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
    private static List<AdventureData> adList = new List<AdventureData>();

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


    public void OnClickAdventureBtn()   // 모험가 클릭
    {
        DetachAdventureListUI tmp = UIManager.Instance.GetActiveUI<DetachAdventureListUI>() as DetachAdventureListUI;

        if (AdventureData.adventureSelectId.Contains(adventureid))
        {
            AdventureData.adventureSelectId.Remove(adventureid);
            adList.Remove(adventureData);                           // 선택한 모험가 삭제

            QuestManager.Instance.adventureDatas.Remove(tmp.adventureIndex);    // 기존 파견 index에 맞는 리스트 삭제
            QuestManager.Instance.adventureDatas.Add(tmp.adventureIndex, adList);   // 삭제된 모험가 리스트 다시 넣기

            return;
        }

        if (AdventureData.adventureSelectId.Count == 4) // 제일 마지막에 선택한 모험가 삭제
        {
            AdventureData.adventureSelectId.RemoveAt(0);
            adList.RemoveAt(0);
        }

        AdventureData.adventureSelectId.Add(adventureid);

        adList.Add(adventureData);

        QuestManager.Instance.adventureDatas.Remove(tmp.adventureIndex);    // 기존 파견 index에 맞는 리스트 삭제
        QuestManager.Instance.adventureDatas.Add(tmp.adventureIndex, adList);   // 삭제된 모험가 리스트 다시 넣기
    }

}
