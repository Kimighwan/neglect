using Gpm.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestListItemSlot : InfiniteScrollItem
{
    public RawImage rankImage;

    public TextMeshProUGUI m_name;
    public TextMeshProUGUI level;
    public TextMeshProUGUI time;
    public TextMeshProUGUI reward;

    private QuestData questData;

    private int questMonsterDescId;
    private int questId;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        var rectTransform = GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(360f, 515f);

        questData = scrollData as QuestData;

        if (questData == null) return;

        questId = questData.questId;
        var questName = questData.questName;
        var questLevel = questData.questLevel;
        var questTime = questData.questTime;
        var questReward = questData.questReward;

        questMonsterDescId = questData.questMonsterDescId;

        m_name.text = questName;
        level.text = questLevel;
        time.text = questTime.ToString();
        reward.text = questReward.ToString();

        if (questLevel == "브론즈")
            rankImage.texture = Resources.Load("Arts/QuestRank/bronze_quest") as Texture2D;
        else if (questLevel == "실버")
            rankImage.texture = Resources.Load("Arts/QuestRank/silver_quest") as Texture2D;
        else if (questLevel == "골드")
            rankImage.texture = Resources.Load("Arts/QuestRank/gold_quest") as Texture2D;
        else if (questLevel == "플래티넘")
            rankImage.texture = Resources.Load("Arts/QuestRank/platinum_quest") as Texture2D;
        else if (questLevel == "다이아")
            rankImage.texture = Resources.Load("Arts/QuestRank/diamond_quest") as Texture2D;
    }

    public void OnClickDetailBtn()  // 의뢰 세부사항 UI 열기
    {
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
