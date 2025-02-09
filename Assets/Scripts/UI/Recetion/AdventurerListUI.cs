using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 여기서 가지고 있는 모험가들 ID를 모두 가져온다
// 각 ID를 사용해 AdventureInformationUI 프리펩을 초기화한다.
// 초기화된 UI들을 띄운다.

public class AdventurerListUI : BaseUI
{
    public Transform adventureInformationPos;
    public TMP_FontAsset fontAsset;

    private int[] idListOfint;

    private string adventureName;
    private string adventurePosition;
    private string imageName2;
    private string imageName3;

    private List<int> instancedAdventure = new List<int>();

    public override void Init(Transform anchor)
    {
        base.Init(anchor);

        var rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector3(0f, -58f, 0f);
        rectTransform.sizeDelta = new Vector2(1176.5f, 967f);

        idListOfint = null;

        CheckMyAdventure();
    }

    public void OnClickBackBtnOfAdventureListUI()
    {
        UIManager.Instance.CloseUI(this);

        UIManager.Instance.OnClickCounter();
    }


    private void CheckMyAdventure()
    {
        string idList = PlayerPrefs.GetString("AdventureId");    // 저장된 모험가 ID 불러오기
        string[] idListOfstring = idList.Split(',');

        if (idList == "") return;

        for(int index = 0; index < idListOfstring.Length; index++)
        {
            GetAdventure(Convert.ToInt32(idListOfstring[index]));
        }
    }

    private void GetAdventure(int id)
    {
        if (instancedAdventure.Contains(id))
            return;

        AdventureData adventureData = DataTableManager.Instance.GetAdventureData(id);

        instancedAdventure.Add(id);

        adventureName = adventureData.adventureName;
        adventurePosition = adventureData.adventurePosition;
        imageName2 = adventureData.adventureClass;
        imageName3 = adventureData.adventureType;

        InstantiateAdventureInformationUI(adventureName);
    }

    private void InstantiateAdventureInformationUI(string name)
    {
        var item = Instantiate(Resources.Load("UI/AdventureInformationUI") as GameObject);

        item.transform.SetParent(adventureInformationPos);

        GameObject obj = new GameObject("Name");
        obj.transform.SetParent(item.transform);

        obj.AddComponent<TextMeshProUGUI>();
        obj.GetComponent<TextMeshProUGUI>().text = name;
        obj.GetComponent<TextMeshProUGUI>().font = fontAsset;
    }
}
