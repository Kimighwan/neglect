using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerListUI : BaseUI
{
    public override void Init(Transform anchor)
    {
        base.Init(anchor);

        var rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector3(0f, -58f, 0f);
        rectTransform.sizeDelta = new Vector2(1176.5f, 967f);
    }

    public void OnClickBackBtnOfAdventureListUI()
    {
        UIManager.Instance.CloseUI(this);

        UIManager.Instance.OnClickCounter();
    }
}
