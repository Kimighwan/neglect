using TMPro;
using UnityEngine;

public class QuestSelectedUI : MonoBehaviour
{
    public int questId;

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
        var rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
        rectTransform.sizeDelta = new Vector2(311f, 515f);

        GetQuestData();
    }

    private void GetQuestData()
    {
        QuestData data = DataTableManager.Instance.GetQuestData(questId);

        questName = data.questName;
        questLevel = data.questLevel;
        questTime = data.questTime.ToString();
        questReward = data.questReward.ToString();
        questDescId = data.questMonsterDescId;
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
}
