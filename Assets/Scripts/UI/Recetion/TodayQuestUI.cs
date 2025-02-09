using System.Collections.Generic;
using UnityEngine;

public class TodayQuestUI : BaseUI
{
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

    private void SetQuestList() // 의뢰 리스트 생성
    {
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
