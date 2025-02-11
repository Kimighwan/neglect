using Gpm.Ui;
using TMPro;
using UnityEngine;

public class QuestListItemSlot : InfiniteScrollItem
{
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

        rectTransform.sizeDelta = new Vector2(311f, 515f);

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
