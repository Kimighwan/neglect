using TMPro;
using UnityEngine;

public class QuestSelectedUI : MonoBehaviour
{
    public TextMeshProUGUI m_name;
    public TextMeshProUGUI level;
    public TextMeshProUGUI time;
    public TextMeshProUGUI reward;


    private string questName;
    private string questLevel;
    private string questTime;
    private string questReward;

    private int questDescId;

    private void Awake()
    {
        GetQuestData();
        InitData();
    }

    private void GetQuestData()
    {
        QuestData data = DataTableManager.Instance.GetQuestData(130001);

        questName = data.questName;
        questLevel = data.questLevel;
        questTime = data.questTime.ToString();
        questReward = data.questReward.ToString();
        questDescId = data.questMonsterDescId;
    }

    private void InitData()
    {
        m_name.text = questName;
        level.text = questLevel;
        time.text = questTime;
        reward.text = questReward;
    }

    private void SetMonsterDescID()
    {
        DataTableManager.Instance.monsterDescId = questDescId;
    }

    public void OnClickDetailBtn()  // 의뢰 세부사항 UI 열기
    {
        SetMonsterDescID();

        var questDetailUI = new BaseUIData();
        UIManager.Instance.OpenUI<QuestDetailUI>(questDetailUI);
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
