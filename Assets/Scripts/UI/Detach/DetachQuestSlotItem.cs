using Gpm.Ui;
using TMPro;
using UnityEngine;

public class DetachQuestSlotItem : InfiniteScrollItem
{
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtLevel;
    public TextMeshProUGUI txtTime;
    public TextMeshProUGUI txtReward;

    public GameObject checkImage;

    private QuestData questData;

    private int questid;
    private int questMonsterDescId;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        var rectTransform = GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(160f, 160f);

        questData = scrollData as QuestData;

        if (questData == null) return;

        questid = questData.questId;
        var questName = questData.questName;
        var questLevel = questData.questLevel;
        var questTime = questData.questTime;
        var questReward = questData.questReward;
        var questMonster = questData.questMonster;
        questMonsterDescId = questData.questMonsterDescId;

        txtName.text = questName;
        txtLevel.text = questLevel;
        txtTime.text = questTime.ToString();
        txtReward.text = questReward.ToString();
    }

    public void OnClickSelect()
    {
        checkImage.SetActive(true);
    }
}
