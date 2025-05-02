using Gpm.Ui;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetachAdventureSlotItem : InfiniteScrollItem
{
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI posTxt;
    public TextMeshProUGUI classTxt;
    public TextMeshProUGUI typeTxt;

    public RawImage rankImg;
    public RawImage positionImg;
    public RawImage classImg;
    public RawImage typeImg;

    public GameObject checkImage;

    private AdventureData adventureData;

    [SerializeField]
    private TextMeshProUGUI rankTxt;

    [SerializeField]
    private AdventureListSlotItemOverRay pos;
    [SerializeField]
    private AdventureListSlotItemOverRay m_class;
    [SerializeField]
    private AdventureListSlotItemOverRay type;
    [SerializeField]
    private AdventureListSlotItemOverRay rank;

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
        posTxt.text = adventurePosition;
        pos.pos = adventurePosition;

        classTxt.text = adventureClass;
        m_class.m_class = adventureClass;

        typeTxt.text = adventureType;
        type.type = adventureType;

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
        {
            rankImg.texture = Resources.Load("Arts/Rank/RankBronze") as Texture2D;
            rankTxt.text = "브론즈";
            rank.rank = "브론즈";
        }
        else if (adventureTier == "실버")
        {
            rankImg.texture = Resources.Load("Arts/Rank/RankSilver") as Texture2D;
            rankTxt.text = "실버";
            rank.rank = "실버";
        }
        else if (adventureTier == "골드")
        {
            rankImg.texture = Resources.Load("Arts/Rank/RankGold") as Texture2D;
            rankTxt.text = "골드";
            rank.rank = "골드";
        }
        else if (adventureTier == "플래티넘")
        {
            rankImg.texture = Resources.Load("Arts/Rank/RankPlatinum") as Texture2D;
            rankTxt.text = "플래티넘";
            rank.rank = "플래티넘";
        }
        else
        {
            rankImg.texture = Resources.Load("Arts/Rank/RankDiamond") as Texture2D;
            rankTxt.text = "다이아";
            rank.rank = "다이아";
        }
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
        var ui = UIManager.Instance.GetActiveUI<EmergencyQuestUI>() as EmergencyQuestUI;

        if (AdventureData.adventureSelectId.Contains(adventureid))  // 선택 취소
        {
            AdventureData.adventureSelectId.Remove(adventureid);

            if (tmp.adventureIndex < 10)
            {
                /// 파견 index에 맞는 QuestManager의 선택된 모험가 삭제
                PoolManager.Instance.questManagers[tmp.adventureIndex - 1].adventureDatas.Remove(adventureData);  
                return;
            }
            else
            {
                /// 파견 index에 맞는 QuestManager의 선택된 모험가 삭제
                ui.questManager.adventureDatas.Remove(adventureData);
                return;
            }

        }

        if (AdventureData.adventureSelectId.Count == 4) // 제일 마지막에 선택한 모험가 삭제 필요
        {
            AdventureData.adventureSelectId.RemoveAt(0);

            if (tmp.adventureIndex < 10)
            {
                PoolManager.Instance.questManagers[tmp.adventureIndex - 1].adventureDatas.RemoveAt(0);
            }
            else
            {
                ui.questManager.adventureDatas.RemoveAt(0);
            }
        }

        AdventureData.adventureSelectId.Add(adventureid);

        if(tmp.adventureIndex < 10)
            PoolManager.Instance.questManagers[tmp.adventureIndex - 1].adventureDatas.Add(adventureData);   // 선택된 모험가 파견 index에 맞는 QuestManager에 넣기
        else
        {
            
            ui.questManager.adventureDatas.Add(adventureData);
        }
    }
}
