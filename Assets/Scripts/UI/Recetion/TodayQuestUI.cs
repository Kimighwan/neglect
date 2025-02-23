using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TodayQuestUI : BaseUI
{
    public TextMeshProUGUI txt;

    private List<GameObject> uiListPool = new List<GameObject>();   // 의뢰 종이 3장 Pool


    private void Awake()
    {
        SetQuestList();
    }

    public override void Init(Transform anchor)
    {
        base.Init(anchor);

        var rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector3(0f, -58f, 0f);
        rectTransform.sizeDelta = new Vector2(1176.5f, 967f);
    }

    private void Update()
    {
        if(GameInfo.gameInfo.Level == 1)
        {
            txt.text = "확률 : 80:20:0:0:0";
        }
        else if (GameInfo.gameInfo.Level == 2)
        {
            txt.text = "확률 : 45:40:15:0:0";
        }
        else if (GameInfo.gameInfo.Level == 3)
        {
            txt.text = "확률 : 20:45:30:5:0";
        }
        else if (GameInfo.gameInfo.Level == 4)
        {
            txt.text = "확률 : 0:35:50:10:5";
        }
        else
        {
            txt.text = "확률 : 0:10:60:25:5";
        }
    }

    private void SetQuestList() // 의뢰 리스트 생성
    {
        PoolManager.Instance.userQuestIndex.Clear();

        for (int index = 0; index < 3; index++)
        {
            var ui = Instantiate(Resources.Load("UI/RandomQuestSelectUI") as GameObject);
            uiListPool.Add(ui);
        }
    }

    public void OnClickAwakeBtn()
    {
        // 골드 지불하며 의뢰 리스트 초기화
        RemoveList();
        SetQuestList();
    }

    public void RemoveList()
    {
        for (int index = 0; index < 3; index++)
        {
            Destroy(uiListPool[index].gameObject);
        }

        uiListPool.Clear();
    }

    public void OnClickBackBtnOfTodatQuest()
    {
        UIManager.Instance.CloseUI(this);

        var receptionUI = new BaseUIData();
        UIManager.Instance.OpenUI<ReceptionUI>(receptionUI);
    }
}
