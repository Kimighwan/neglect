using System.Collections.Generic;
using UnityEngine;

public class AdventurerUI : BaseUI
{
    private List<GameObject> uiListPool = new List<GameObject>();   // 모험가 카드 3장 Pool

    private void Awake()
    {
        SetAdventureList();
    }

    public override void Init(Transform anchor)
    {
        base.Init(anchor);

        var rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector3(0f, -58f, 0f);
        rectTransform.sizeDelta = new Vector2(1176.5f, 967f);
    }

    private void SetAdventureList() // 모험가 리스트 생성
    {
        for (int index = 0; index < 3; index++)
        {
            var ui = Instantiate(Resources.Load("UI/RandomAdventureSelectUI") as GameObject);
            uiListPool.Add(ui);
        }
    }

    public void OnClickAwakeBtn()
    {
        // 골드 지불하며 모험가 리스트 초기화
        RemoveList();
        SetAdventureList();
    }

    public void RemoveList()
    {
        for (int index = 0; index < 3; index++)
        {
            Destroy(uiListPool[index].gameObject);
        }

        uiListPool.Clear();
    }
}
