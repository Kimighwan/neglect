using System;
using System.Collections;
using System.Collections.Generic;
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
        questData = DataTableManager.Instance.GetQuestData(RandomIndexMake());

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
        int randomIdA = UnityEngine.Random.Range(131001, 131007);
        int randomIdB = UnityEngine.Random.Range(132007, 132013);
        int randomIdC = UnityEngine.Random.Range(133013, 133022);
        int randomIdD = UnityEngine.Random.Range(134022, 134026);
        int randomIdE = UnityEngine.Random.Range(135026, 135029);

        int i = UnityEngine.Random.Range(1, 6);

        switch (i)
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
