using TMPro;
using UnityEngine;

public class QuestDetailUI : BaseUI
{
    public TextMeshProUGUI m_name;
    public TextMeshProUGUI m_level;
    public TextMeshProUGUI m_time;
    public TextMeshProUGUI m_target;
    public TextMeshProUGUI m_reward;


    private int monsterId;

    public override void Init(Transform anchor)
    {
        base.Init(anchor);

        var rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector3(0f, -58f, 0f);
        rectTransform.sizeDelta = new Vector2(1176.5f, 967f);

        monsterId = DataTableManager.Instance.monsterDescId;
        SetData();
    }

    private void SetData()
    {
        QuestData data = DataTableManager.Instance.GetQuestData(DataTableManager.Instance.questDetailId);

        m_name.text = "의뢰명 : " + data.questName;
        m_level.text = "의뢰난이도 : " + data.questLevel;
        m_time.text = "의뢰시간 : " + data.questTime.ToString();
        m_target.text = data.questMonster;
        m_reward.text = "의로보상 : " + data.questReward.ToString();
    }

    public void OnClickBackOfQuestDetailList()
    {
        UIManager.Instance.CloseUI(this);

        var todayQuestUI = new BaseUIData();
        UIManager.Instance.OpenUI<TodayQuestUI>(todayQuestUI);
    }

    public void OnClickMonsterDescBtn() // 몬스터 도감(설명) 열기
    {
        var monsterDescUI = new BaseUIData();

        UIManager.Instance.CloseUI(this);
        UIManager.Instance.OpenUI<MonsterDescUI>(monsterDescUI);
    }
}
