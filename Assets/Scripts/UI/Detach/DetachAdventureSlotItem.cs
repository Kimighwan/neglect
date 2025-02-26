using Gpm.Ui;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetachAdventureSlotItem : InfiniteScrollItem
{
    public TextMeshProUGUI txtName;

    public RawImage rankImg;
    public RawImage positionImg;
    public RawImage classImg;
    public RawImage typeImg;

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

        // Postion Image
        if (adventurePosition == "전위")
            positionImg.texture = Resources.Load("Arts/Icon/Pos/FrontIcon") as Texture2D;
        else if (adventurePosition == "중위")
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
