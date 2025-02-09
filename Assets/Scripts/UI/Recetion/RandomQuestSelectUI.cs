using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomQuestSelectUI : MonoBehaviour
{
    public TextMeshProUGUI name;
    public TextMeshProUGUI level;
    public TextMeshProUGUI time;
    public TextMeshProUGUI reward;


    private QuestData questData;

    private int questId;
    private int questTime;
    private int questReward;

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
        string pre = PlayerPrefs.GetString("QuestId");  // 저장된 모험가 ID 불러오기

        // 영입된 모험가 저장하기
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
    }

    private void InitData()
    {
        name.text = questName;
        level.text = questLevel;
        time.text = questTime.ToString();
        reward.text = questReward.ToString();
    }

    private int RandomIndexMake()   // 무작위 숫자
    {
        int randomId;

        randomId = UnityEngine.Random.Range(130001, 130029);

        return randomId;
    }
}
