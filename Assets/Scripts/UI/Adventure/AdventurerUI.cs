using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdventurerUI : BaseUI
{
    public TextMeshProUGUI txt;
    private List<GameObject> uiListPool = new List<GameObject>();   // 모험가 카드 3장 Pool
    private Desk desk;

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

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
        var deskData = uiData as AdventurerUIWithDesk;
        desk = deskData.desk;
    }

    private void Update()
    {
        if(GameInfo.gameInfo.Level == 1)
        {
            txt.text = "확률 : 79:20:1:0:0";
        }
        else if (GameInfo.gameInfo.Level == 2)
        {
            txt.text = "확률 : 44:40:15:1:0";
        }
        else if (GameInfo.gameInfo.Level == 3)
        {
            txt.text = "확률 : 19:40:30:10:1";
        }
        else if (GameInfo.gameInfo.Level == 4)
        {
            txt.text = "확률 : 0:20:50:25:5";
        }
        else
        {
            txt.text = "확률 : 0:0:50:40:10";
        }

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
        if(GameInfo.gameInfo.Gold < 100)
        {
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK;
            uiData.descTxt = "골드가 부족합니다.";
            uiData.okBtnTxt = "확인";
            UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            return;
        }

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

    public override void OnClickCloseButton()
    {
        desk.OnClickBut();
        base.OnClickCloseButton();
    }
}
