using Gpm.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestListItemSlot : InfiniteScrollItem
{
    public RawImage rankImage;

    public TextMeshProUGUI m_name;
    public TextMeshProUGUI time;
    public TextMeshProUGUI reward;
    public TextMeshProUGUI rankTxt;

    private QuestData questData;

    public GameObject ingQuest;

    [SerializeField]
    private QuestListItemOverRay rank;

    private bool onIngQuest = false;

    private int questMonsterDescId;
    private int questId;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        // var rectTransform = GetComponent<RectTransform>();

        // rectTransform.sizeDelta = new Vector2(300f, 500f);
        // rectTransform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        // rectTransform.anchoredPosition = new Vector3(960f, 0f, 0f);
        // rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, 290f);
        // rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 290f);
        // rectTransform.pivot = new Vector2(0.5f, 0.5f);

        questData = scrollData as QuestData;

        if (questData == null) return;

        questId = questData.questId;
        var questName = questData.questName;
        var questLevel = questData.questLevel;
        var questTime = questData.questTime;
        var questReward = questData.questReward;

        questMonsterDescId = questData.questMonsterDescId;

        m_name.text = questName;
        time.text = questTime.ToString() + "일";
        reward.text = questReward.ToString() + "골드";

        rankTxt.text = questLevel;
        rank.rank = questLevel;

        if (questLevel == "브론즈")
            rankImage.texture = Resources.Load("Arts/QuestRank/bronze_quest") as Texture2D;
        else if (questLevel == "실버")
        {
            rankImage.texture = Resources.Load("Arts/QuestRank/silver_quest") as Texture2D;
            if ((questId / 100) % 10 == 8)
            {
                rankImage.texture = Resources.Load("Arts/QuestRank/special_quest") as Texture2D;
                rankTxt.text += "\n(특수)";
                rank.rank += "(특수)";
            }
        }
        else if (questLevel == "골드")
        {
            rankImage.texture = Resources.Load("Arts/QuestRank/gold_quest") as Texture2D;
            if ((questId / 100) % 10 == 8)
            {
                rankImage.texture = Resources.Load("Arts/QuestRank/special_quest") as Texture2D;
                rankTxt.text += "\n(특수)";
                rank.rank += "(특수)";
            }
        }
        else if (questLevel == "플래티넘")
        {
            rankImage.texture = Resources.Load("Arts/QuestRank/platinum_quest") as Texture2D;
            if ((questId / 100) % 10 == 8)
            {
                rankImage.texture = Resources.Load("Arts/QuestRank/special_quest") as Texture2D;
                rankTxt.text += "\n(특수)";
                rank.rank += "(특수)";
            }
        }
        else if (questLevel == "다이아")
            rankImage.texture = Resources.Load("Arts/QuestRank/diamond_quest") as Texture2D;

        if (PoolManager.Instance.usingQuestList.Contains(questId))
            ingQuest.SetActive(true);
        else
            ingQuest.SetActive(false);
    }

    public void OnClickDetailBtn()  // 의뢰 세부사항 UI 열기
    {
        // 의뢰 수행 중임으로 삭제 금지 그로인해 디테일 창 이동 금지
        if (PoolManager.Instance.usingQuestList.Contains(questId) && !onIngQuest)
        {
            onIngQuest = true;
            ingQuest.SetActive(true);
            return;
        }
        else if (PoolManager.Instance.usingQuestList.Contains(questId) && onIngQuest)
        {
            onIngQuest = false;
            ingQuest.SetActive(false);
            return;
        }

        SetMonsterDescID();

        UIManager.Instance.CloseUI(UIManager.Instance.GetActiveUI<QuestListUI>());
        var questDetailUI = new BaseUIData();
        UIManager.Instance.OpenUI<QuestDetailUI>(questDetailUI);
    }

    private void SetMonsterDescID()
    {
        DataTableManager.Instance.questDetailId = questId;
        DataTableManager.Instance.monsterDescId = questMonsterDescId;
    }
}
