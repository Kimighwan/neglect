using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdventureExportUI : BaseUI
{
    public TextMeshProUGUI stateText;

    private void OnEnable()
    {
        SetStateText();
    }

    public override void Init(Transform anchor)
    {
        base.Init(anchor);
    }

    public void OnClickExportBtn()
    {
        UIManager.Instance.CloseUI(this);

        // 제거하는 모험가에 따라 골드 얻기
        // AddGold()

        // PlayerPrefs에 해당 모험가 제거
        DeleteAdventure();

        // 인피니티 스크롤 업데이트
        AdventurerListUI.Instance.UpdateScrollItem();
    }

    private void DeleteAdventure()
    {
        var adventureId = PlayerPrefs.GetString("AdventureId");
        var adventureIds = adventureId.Split(',');

        if (adventureId == "") return;

        string addId = "";

        foreach (var item in adventureIds)
        {
            int adventureIdOfInt = Convert.ToInt32(item);

            if (UIManager.Instance.exportAdventureId != adventureIdOfInt)
            {
                if (addId == "")
                    addId += adventureIdOfInt.ToString();
                else
                    addId += "," + adventureIdOfInt.ToString();
            }
        }

        PlayerPrefs.SetString("AdventureId", addId);
    }

    private void SetStateText()
    {
        Debug.Log("SetStateText");
        if (PoolManager.Instance.usingAdventureList.Contains(UIManager.Instance.exportAdventureId))
        {
            stateText.text = "파견 중";
        }
        else
        {
            stateText.text = "대기 중";
        }
    }
}
