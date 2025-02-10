using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestListUI : BaseUI
{
    private List<int> questId = new List<int>();

    public Transform pos;

    public override void Init(Transform anchor)
    {
        base.Init(anchor);

        var rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector3(0f, -58f, 0f);
        rectTransform.sizeDelta = new Vector2(1176.5f, 967f);
    }

    private void OnEnable()
    {
        CheckMyQuest();
    }

    public void OnClickBackOfQuestList()
    {
        UIManager.Instance.CloseUI(this);

        var receptionUI = new BaseUIData();
        UIManager.Instance.OpenUI<ReceptionUI>(receptionUI);
    }

    private void CheckMyQuest() // 가지고 있는 의뢰 체크
    {
        questId.Clear();

        string myQuestOfString = PlayerPrefs.GetString("QuestId");
        string[] myQuestOfstrings = myQuestOfString.Split(',');

        if (myQuestOfString == "") return;

        foreach (string str in myQuestOfstrings)
        {
            questId.Add(Convert.ToInt32(str));
            InstantiateQuestList(Convert.ToInt32(str));
        }
    }

    private void InstantiateQuestList(int id)   // 의뢰 UI 인스턴스화
    {
        var item = Instantiate(Resources.Load("UI/QuestSelectedUI") as GameObject);
        item.transform.SetParent(pos);

        //item.GetComponent<QuestSelectedUI>().questId = id;
    }
}
