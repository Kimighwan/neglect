using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TodayQuestUI : BaseUI
{
    public TextMeshProUGUI bronzeTxt;
    public TextMeshProUGUI silverTxt;
    public TextMeshProUGUI goldTxt;
    public TextMeshProUGUI platinumTxt;
    public TextMeshProUGUI diaTxt;
    public TextMeshProUGUI countText;

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
        SetCountText();

        if (GameInfo.gameInfo.Level == 1)
        {
            bronzeTxt.text = "80%";
            silverTxt.text = "20%";
            goldTxt.text = "0%";
            platinumTxt.text = "0%";
            diaTxt.text = "0%";
        }
        else if (GameInfo.gameInfo.Level == 2)
        {
            bronzeTxt.text = "45%";
            silverTxt.text = "40%";
            goldTxt.text = "15%";
            platinumTxt.text = "0%";
            diaTxt.text = "0%";
        }
        else if (GameInfo.gameInfo.Level == 3)
        {
            bronzeTxt.text = "20%";
            silverTxt.text = "45%";
            goldTxt.text = "30%";
            platinumTxt.text = "5%";
            diaTxt.text = "0%";
        }
        else if (GameInfo.gameInfo.Level == 4)
        {
            bronzeTxt.text = "0%";
            silverTxt.text = "35%";
            goldTxt.text = "50%";
            platinumTxt.text = "10%";
            diaTxt.text = "5%";
        }
        else
        {
            bronzeTxt.text = "0%";
            silverTxt.text = "10%";
            goldTxt.text = "60%";
            platinumTxt.text = "25%";
            diaTxt.text = "5%";
        }

        if (GameInfo.gameInfo.nextDay)
        {
            RemoveList();
            SetQuestList();
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
        if (!GameInfo.gameInfo.ChangeGold(-100))
        {
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK;
            uiData.descTxt = "골드가 부족합니다.";
            uiData.okBtnTxt = "확인";
            AudioManager.Instance.PlaySFX(SFX.Denied);
            UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            return;
        }

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

    private int CheckCurrentQuestCount()
    {
        int tmp = 0;

        var a = PlayerPrefs.GetString("QuestId");
        foreach (var item in a.Split(','))
        {
            if (item != "")
                tmp++;
        }

        return tmp;
    }

    private int CheckMaxQuestCount()
    {
        int tmp = 0;

        if (GameInfo.gameInfo.Level == 1)
            tmp = 6;
        else if (GameInfo.gameInfo.Level == 2)
            tmp = 8;
        else if (GameInfo.gameInfo.Level == 3)
            tmp = 10;
        else if (GameInfo.gameInfo.Level == 4)
            tmp = 12;
        else if (GameInfo.gameInfo.Level == 5)
            tmp = 14;

        return tmp;
    }

    private void SetCountText()
    {
        countText.text = CheckCurrentQuestCount().ToString() + "/" + CheckMaxQuestCount().ToString();
    }
}
