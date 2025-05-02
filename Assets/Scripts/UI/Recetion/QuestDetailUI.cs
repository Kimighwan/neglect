using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDetailUI : BaseUI
{
    public RawImage rankImg;

    public TextMeshProUGUI m_name;
    public TextMeshProUGUI m_level;
    public TextMeshProUGUI m_time;
    public TextMeshProUGUI m_target;
    public TextMeshProUGUI m_reward;

    public TextMeshProUGUI paperName;
    public TextMeshProUGUI paperTime;
    public TextMeshProUGUI paperReward;
    public TextMeshProUGUI rankTxt;

    public Button monsterDetailBtn;
    public Button lockBtn;

    [SerializeField]
    private QuestListItemOverRay rank;

    private int monsterId;

    private bool active;

    public override void Init(Transform anchor, RectTransform canvasRT = null)
    {
        base.Init(anchor);

        var rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector3(0f, -58f, 0f);
        rectTransform.sizeDelta = new Vector2(1176.5f, 967f);

        monsterId = DataTableManager.Instance.monsterDescId;
        SetData();
    }

    private void SetData()
    {
        QuestData data = DataTableManager.Instance.GetQuestData(DataTableManager.Instance.questDetailId);

        m_name.text = "의뢰명 : " + data.questName.Replace("\n", "");
        m_level.text = "의뢰난이도 : " + data.questLevel;
        m_time.text = "의뢰시간 : " + data.questTime.ToString();
        m_target.text = data.questMonster;
        m_reward.text = "의뢰보상 : " + data.questReward.ToString();

        paperName.text = data.questName;
        paperTime.text = data.questTime.ToString() +"일";
        paperReward.text = data.questReward.ToString() +"골드";

        rankTxt.text = data.questLevel;
        rank.rank = data.questLevel;

        if (data.questLevel == "브론즈")
            rankImg.texture = Resources.Load("Arts/QuestRank/bronze_quest") as Texture2D;
        else if (data.questLevel == "실버")
        {
            rankImg.texture = Resources.Load("Arts/QuestRank/silver_quest") as Texture2D;
            if ((data.questId / 100) % 10 == 8)
            {
                rankImg.texture = Resources.Load("Arts/QuestRank/special_quest") as Texture2D;
                rankTxt.text += "(특수)";
                rank.rank += "(특수)";
            }
        }
        else if (data.questLevel == "골드")
        {
            rankImg.texture = Resources.Load("Arts/QuestRank/gold_quest") as Texture2D;
            if ((data.questId / 100) % 10 == 8)
            {
                rankImg.texture = Resources.Load("Arts/QuestRank/special_quest") as Texture2D;
                rankTxt.text += "(특수)";
                rank.rank += "(특수)";
            }
        }
        else if (data.questLevel == "플래티넘")
        {
            rankImg.texture = Resources.Load("Arts/QuestRank/platinum_quest") as Texture2D;
            if ((data.questId / 100) % 10 == 8)
            {
                rankImg.texture = Resources.Load("Arts/QuestRank/special_quest") as Texture2D;
                rankTxt.text += "(특수)";
                rank.rank += "(특수)";
            }
        }
        else if (data.questLevel == "다이아")
            rankImg.texture = Resources.Load("Arts/QuestRank/diamond_quest") as Texture2D;

        if (data.questActive == 1)
            active = true;
        else
            active = false;

        // 도감에 존재하느냐에 따라 활성화/비활성화
        monsterDetailBtn.interactable = active;

        // 몬스터 처치한 적이 있으면 도감 활성화 가능
        if (PlayerPrefs.HasKey($"{data.questMonster}"))
        {
            if (PlayerPrefs.GetInt($"{data.questMonster}") == 1)
            {
                lockBtn.gameObject.SetActive(false);
                monsterDetailBtn.interactable = active;
            }
            else
            {
                monsterDetailBtn.interactable = false;
                lockBtn.gameObject.SetActive(true);
            }
        }
    }


    public void OnClickBackOfQuestDetailList()
    {
        UIManager.Instance.CloseUI(this);

        var todayQuestUI = new BaseUIData();
        UIManager.Instance.OpenUI<QuestListUI>(todayQuestUI);
    }

    public void OnClickBackToTodayQuestListUI()
    {
        UIManager.Instance.CloseUI(this);

        var data = new BaseUIData();
        UIManager.Instance.OpenUI<TodayQuestUI>(data);
    }

    public void OnClickMonsterDescBtn() // 몬스터 도감(설명) 열기
    {
        var monsterDescUI = new BaseUIData();

        UIManager.Instance.OpenUI<MonsterDescUI>(monsterDescUI);
    }

    public void OnClickLockBtn()
    {
        var uiData = new ConfirmUIData();
        uiData.confirmType = ConfirmType.OK;
        uiData.descTxt = "아직 도감이 해금되지 않았습니다.";
        uiData.okBtnTxt = "확인";
        AudioManager.Instance.PlaySFX(SFX.Denied);
        UIManager.Instance.OpenUI<ConfirmUI>(uiData);
        return;
    }

    public void OnClickDeleteBtn()
    {
        var uiData = new ConfirmUIData();
        uiData.confirmType = ConfirmType.OK_CANCEL;
        uiData.descTxt = "정말로 삭제 하시겠습니까?";
        uiData.okBtnTxt = "삭제";
        uiData.cancelBtnTxt = "아니요";
        uiData.onClickOKBtn = () =>
        {
            QuestDelete();
        };
        UIManager.Instance.OpenUI<ConfirmUI>(uiData);
    }

    private void QuestDelete()
    {
        PoolManager.Instance.userQuestList.Remove(DataTableManager.Instance.GetQuestData(DataTableManager.Instance.questDetailId));

        var questId = PlayerPrefs.GetString("QuestId");
        var questIds = questId.Split(',');

        if (questId == "") return;

        string add = "";

        foreach (var item in questIds)
        {
            int questIdOfInt = Convert.ToInt32(item);
            if(DataTableManager.Instance.questDetailId != questIdOfInt)
            {
                if(add == "")
                {
                    add += questIdOfInt.ToString();
                }
                else
                {
                    add += "," + questIdOfInt.ToString();
                }
            }
        }

        PlayerPrefs.SetString("QuestId", add);

        UIManager.Instance.CloseUI(this);

        var todayQuestUI = new BaseUIData();
        UIManager.Instance.OpenUI<QuestListUI>(todayQuestUI);
    }
}
