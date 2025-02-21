using System;
using TMPro;
using UnityEngine;

public class RandomQuestSelectUI : MonoBehaviour
{
    public TextMeshProUGUI m_name;
    public TextMeshProUGUI level;
    public TextMeshProUGUI time;
    public TextMeshProUGUI reward;

    private QuestData questData;

    private int questId;
    private int questTime;
    private int questReward;
    private int questMonsterDescId;

    private int j;
    private int resultId;


    private string questName;
    private string questLevel;

    private Transform pos;  // 의뢰 종이 위치


    private void Awake()
    {
        var rectTransform = GetComponent<RectTransform>();

        pos = GameObject.FindGameObjectWithTag("QeustSelectGroup").transform;

        this.transform.SetParent(pos);
        rectTransform.sizeDelta = new Vector2(311.3f, 515.6f);

        GetQuestData();
        InitData();
    }

    public void OnClickSelected()
    {
        string pre = PlayerPrefs.GetString("QuestId");  // 저장된 quest ID 불러오기

        // 선택한 quest ID 저장
        if (pre == "")
        {
            PlayerPrefs.SetString("QuestId", questId.ToString());
        }
        else
        {
            PlayerPrefs.SetString("QuestId", pre + "," + questId.ToString());
        }

        // 골드 차감

    }

    private void GetQuestData()
    {
        questData = DataTableManager.Instance.GetQuestDataUsingIndex(RandomIndexMake());

        questId = questData.questId;
        this.questName = questData.questName;
        this.questLevel = questData.questLevel;
        this.questTime = questData.questTime;
        this.questReward = questData.questReward;
        this.questMonsterDescId = questData.questMonsterDescId;
    }

    private void InitData()
    {
        m_name.text = questName;
        level.text = questLevel;
        time.text = questTime.ToString();
        reward.text = questReward.ToString();
    }

    private int RandomIndexMake()   // 무작위 숫자
    {
        int randomIdA = UnityEngine.Random.Range(1, 10);    // 브론즈
        int randomIdB = UnityEngine.Random.Range(10, 20);   // 실버
        int randomIdC = UnityEngine.Random.Range(20, 30);   // 골드
        int randomIdD = UnityEngine.Random.Range(31, 38);   // 플래니텀
        int randomIdE = UnityEngine.Random.Range(38, 41);   // 다이아

        int resultValue = UnityEngine.Random.Range(1, 101);

        int[] probability = new int[5];

        switch (GameInfo.gameInfo.Level)
        {
            case 1:
                probability[0] = 80; probability[1] = 20; probability[2] = 0; probability[3] = 0; probability[4] = 0;
                break;
            case 2:
                probability[0] = 45; probability[1] = 40; probability[2] = 15; probability[3] = 0; probability[4] = 0;
                break;
            case 3:
                probability[0] = 20; probability[1] = 45; probability[2] = 30; probability[3] = 5; probability[4] = 0;
                break;
            case 4:
                probability[0] = 0; probability[1] = 35; probability[2] = 50; probability[3] = 10; probability[4] = 5;
                break;
            case 5:
                probability[0] = 0; probability[1] = 10; probability[2] = 60; probability[3] = 25; probability[4] = 5;
                break;
        }

        int cumulativeProbability = 0;

        for(j = 0; j < 5; j++)
        {
            cumulativeProbability += probability[j];

            if (resultValue < cumulativeProbability)
            {
                break;
            }
        }

        switch (j)
        {
            case 1:
                resultId = randomIdA;
                break;
            case 2:
                resultId = randomIdB;
                break;
            case 3:
                resultId = randomIdC;
                break;
            case 4:
                resultId = randomIdD;
                break;
            case 5:
                resultId = randomIdE;
                break;
            default:
                break;
        }

        return resultId;
    }


    //private void SetMonsterDescID()
    //{
    //    DataTableManager.Instance.monsterDescId = questMonsterDescId;
    //}

    private void SetQuestDetailID()
    {
        DataTableManager.Instance.questDetailId = questId;
    }

    public void OnClickDetailBtn()  // 의뢰 세부사항 UI 열기
    {
        DataTableManager.Instance.monsterDescId = questMonsterDescId;
        SetQuestDetailID();

        var questDetailUI = new BaseUIData();
        UIManager.Instance.CloseUI(UIManager.Instance.GetActiveUI<TodayQuestUI>());
        UIManager.Instance.OpenUI<QuestDetailUI>(questDetailUI);
    }
}
