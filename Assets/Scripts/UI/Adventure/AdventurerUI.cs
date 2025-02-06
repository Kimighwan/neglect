using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerUI : BaseUI
{
    public override void Init(Transform anchor)
    {
        base.Init(anchor);

        var rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector3(0f, -58f, 0f);
        rectTransform.sizeDelta = new Vector2(1176.5f, 967f);
    }

    private void SetAdventureList()
    {
        // 모험가 리스트 초기화
        for(int index = 0; index < 3;  index++)
        {
            var adventureUI = new BaseUIData();
            UIManager.Instance.OpenUI<RandomAdventureSelectUI>(adventureUI);
        }
    }

    public void OnClickAwakeBtn()
    {
        // 골드 지불하며 모험가 리스트 초기화
        SetAdventureList();
    }
}
