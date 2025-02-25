using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDetailUI : BaseUI
{
    public TextMeshProUGUI m_name;
    public TextMeshProUGUI m_level;
    public TextMeshProUGUI m_time;
    public TextMeshProUGUI m_target;
    public TextMeshProUGUI m_reward;

    public TextMeshProUGUI paperName;
    public TextMeshProUGUI paperLevel;
    public TextMeshProUGUI paperTime;
    public TextMeshProUGUI paperReward;

    public Button monsterDetailBtn;

    private int monsterId;

    private bool active;

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

        paperName.text = data.questName;
        paperLevel.text = data.questLevel;
        paperTime.text = data.questTime.ToString();
        paperReward.text = data.questReward.ToString();

        if (data.questActive == 1)
            active = true;
        else
            active = false;

        // 도감에 존재하느냐에 따라 활성화/비활성화
        monsterDetailBtn.interactable = active;

        // 몬스터 처치한 적이 있으면 도감 활성화 가능
        if (PlayerPrefs.HasKey($"{data.questMonster}"))
        {
            if (PlayerPrefs.GetInt($"{data.questMonster}") == 1)
                monsterDetailBtn.interactable = active;
            else
                monsterDetailBtn.interactable = false;
        }
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

        UIManager.Instance.OpenUI<MonsterDescUI>(monsterDescUI);
    }
}
