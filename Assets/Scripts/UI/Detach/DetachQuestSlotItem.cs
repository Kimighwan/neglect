using Gpm.Ui;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

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

    private void Update()
    {
        if (QuestData.questSelectedId == questid)
            checkImage.SetActive(true);
        else
            checkImage.SetActive(false);
    }


    public void OnClickQuestBtn()   // 퀘스트 클릭
    {
        DetachQuestListUI tmp = UIManager.Instance.GetActiveUI<DetachQuestListUI>() as DetachQuestListUI;

        if (QuestData.questSelectedId == questid)
        {
            QuestData.questSelectedId = 0;
            PoolManager.Instance.questData.Remove(tmp.qusetIndex);
            checkImage.SetActive(false);
        }
        else
        {
            QuestData.questSelectedId = questid;
            PoolManager.Instance.questData.Remove(tmp.qusetIndex);
            PoolManager.Instance.questData.Add(tmp.qusetIndex, questData);
        }
    }
}
