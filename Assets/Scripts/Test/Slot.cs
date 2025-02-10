using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Slot : InfiniteScrollItem
{
    public TextMeshProUGUI m_name;
    public TextMeshProUGUI level;
    public TextMeshProUGUI time;
    public TextMeshProUGUI reward;

    private QuestData questData;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        questData = scrollData as QuestData;

        if (questData == null) return; 

        var questId = questData.questId;
        var questName = questData.questName;
        var questLevel = questData.questLevel;
        var questTime= questData.questTime;
        var questReward = questData.questReward;

        m_name.text = questName;
        level.text = questLevel;
        time.text = questTime.ToString();
        reward.text = questReward.ToString();
    }
}
