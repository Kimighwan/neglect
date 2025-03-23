using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdventurerUI : BaseUI
{
    public TextMeshProUGUI bronzeTxt;
    public TextMeshProUGUI silverTxt;
    public TextMeshProUGUI goldTxt;
    public TextMeshProUGUI platinumTxt;
    public TextMeshProUGUI diaTxt;
    public TextMeshProUGUI coundTxt;
    public TextMeshProUGUI rerollTxt;

    private List<GameObject> uiListPool = new List<GameObject>();   // 모험가 카드 3장 Pool
    private Desk desk;

    private void Awake()
    {
        SetAdventureList();
        string.Format($"{bronzeTxt : 0, 2}");
        string.Format($"{silverTxt: 0, 2}");
        string.Format($"{goldTxt: 0, 2}");
        string.Format($"{platinumTxt: 0, 2}");
        string.Format($"{diaTxt: 0, 2}");
    }

    public override void Init(Transform anchor)
    {
        base.Init(anchor);

        var rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
        //rectTransform.sizeDelta = new Vector2(1176.5f, 967f);
    }

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
        var deskData = uiData as AdventurerUIWithDesk;
        desk = deskData.desk;
    }

    private void Update()
    {
        if(GameInfo.gameInfo.nextDayAdventure)
        {
            PoolManager.Instance.userAdventureIndex.Clear();
            RemoveList();
            SetAdventureList();
            GameInfo.gameInfo.nextDayAdventure = false;
        }

        CountUpdate();

        rerollTxt.text = GameInfo.gameInfo.AdventureReroll + "/5";

        if (GameInfo.gameInfo.Level == 1)
        {
            bronzeTxt.text = "79%";
            silverTxt.text = "20%";
            goldTxt.text = "1%";
            platinumTxt.text = "0%";
            diaTxt.text = "0%";
        }
        else if (GameInfo.gameInfo.Level == 2)
        {
            bronzeTxt.text = "44%";
            silverTxt.text = "40%";
            goldTxt.text = "15%";
            platinumTxt.text = "1%";
            diaTxt.text = "0%";
        }
        else if (GameInfo.gameInfo.Level == 3)
        {
            bronzeTxt.text = "19%";
            silverTxt.text = "40%";
            goldTxt.text = "30%";
            platinumTxt.text = "10%";
            diaTxt.text = "1%";
        }
        else if (GameInfo.gameInfo.Level == 4)
        {
            bronzeTxt.text = "0%";
            silverTxt.text = "20%";
            goldTxt.text = "50%";
            platinumTxt.text = "25%";
            diaTxt.text = "5%";
        }
        else
        {
            bronzeTxt.text = "0%";
            silverTxt.text = "0%";
            goldTxt.text = "50%";
            platinumTxt.text = "40%";
            diaTxt.text = "10%";
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

    public void OnClickAwakeBtn()   // 새로고침
    {
        if(!GameInfo.gameInfo.ChangeGold(-100))
        {
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK;
            uiData.descTxt = "골드가 부족합니다.";
            uiData.okBtnTxt = "확인";
            AudioManager.Instance.PlaySFX(SFX.Denied);
            UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            return;
        }

        if(GameInfo.gameInfo.AdventureReroll >= 5)
        {
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK;
            uiData.descTxt = "새로고침 횟수 소진";
            uiData.okBtnTxt = "확인";
            AudioManager.Instance.PlaySFX(SFX.Denied);
            UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            return;
        }

        GameInfo.gameInfo.AddAdventureReroll();

        PoolManager.Instance.userAdventureIndex.Clear();
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
        GameManager.gameManager.cameraTransform.position = new Vector3(0f, 0f, -10f);
        base.OnClickCloseButton();
    }

    private void CountUpdate()
    {
        coundTxt.text = CheckCurrentAdventureCount().ToString() + "/" + GameInfo.gameInfo.GetMaxAdventurerCounts().ToString();
    }

    private int CheckCurrentAdventureCount()
    {
        var a = PlayerPrefs.GetString("AdventureId");
        var b = a.Split(',');

        int tmp = 0;

        foreach (var item in b)
        {
            if (item != "") tmp++;

        }

        return tmp;
    }
}
