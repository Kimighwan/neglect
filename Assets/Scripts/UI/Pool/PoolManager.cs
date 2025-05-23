using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PoolManager : SingletonBehaviour<PoolManager>
{
    public List<QuestData> userQuestList { get; private set; } = new List<QuestData>(); 

    public List<int> userQuestIndex = new List<int>();      // 랜덤 의뢰 선택에서 중복 의뢰 보여주지 않기 위한 List

    public List<AdventureData> userAdventureList { get; private set; } = new List<AdventureData> { };

    public List<int> userAdventureIndex = new List<int>();  // 랜덤 모험가 선택에서 중복 모험가를 보여주지 않기 위한 List

    public List<int> usingQuestList = new List<int>();      // 현재 파견 중인 의뢰

    public List<int> usingAdventureList = new List<int>();  // 현재 파견 중인 모험가


    public Button[] questBtn;       // 파견창의 의뢰 선택 버튼
    public Button[] adventureBtn;   // 파견창의 모험가 선택 버튼


    // QuestManager 이전


    // 파견창 Index에 따른 QuestData
    public Dictionary<int, QuestData> questData = new Dictionary<int, QuestData>();

    public Dictionary<int, int> resultList = new Dictionary<int, int>();  // 파견 Index에 따른 전멸, 성공, 대성공 확인

    public Button[] resultBtn;      // 파견창의 결과 확인 버튼

    public TextMeshProUGUI[] questTxt;
    public TextMeshProUGUI[] adventureTxt;

    public GameObject[] gaugeObject;    // 각 파견창의 게이지 오브젝트
    public GameObject[] awakeBtn;       // 파견창의 초기화 버튼 오브젝트

    public QuestManager[] questManagers;
    public Test[] testScripts;

    [SerializeField] public bool specialAdventureAdd = false;    // 특수 모험가 합류 하는가?

    // 새로운 모험가
    public int bronzAd = 0;     // 브론즈 모험가 수
    public int silverAd = 0;    // 실버 모험가 수
    public int goldAd = 0;      // 골드 모험가 수
    public int platinumAd = 0;  // 플래티넘 모험가 수
    public int diaAd = 0;       // 다이아 모험가 수

    // 새로운 의뢰
    public int bronzQ = 0;     // 브론즈 의뢰 수
    public int silverQ = 0;    // 실버 의뢰 수
    public int goldQ = 0;      // 골드 의뢰 수
    public int platinumQ = 0;  // 플래티넘 의뢰 수
    public int diaQ = 0;       // 다이아 의뢰 수

    // 긴급 의뢰
    public bool ready = true;  // 모험가 선택이 완료되었는가?

    // 특수 의뢰를 받았는가? // 0 = 안 받음
    public int checkHavesSpecialQuest = 0;

    // true 이면 객실 같은 오브젝트 클릭 금지
    public bool isNotTouch = false;
    public bool isNotTouchUI = false;
    public bool isNotTutorialTouch = false;
    protected override void Init()
    {
        isDestroyOnLoad = true;

        base.Init();

        // 모험가 수
        // 브론즈
        if (PlayerPrefs.HasKey("bronzAd"))
            bronzAd = PlayerPrefs.GetInt("bronzAd");
        else
            PlayerPrefs.SetInt("bronzAd", 0);

        // 실버
        if (PlayerPrefs.HasKey("silverAd"))
            silverAd = PlayerPrefs.GetInt("silverAd");
        else
            PlayerPrefs.SetInt("silverAd", 0);

        // 골드
        if (PlayerPrefs.HasKey("goldAd"))
            goldAd = PlayerPrefs.GetInt("goldAd");
        else
            PlayerPrefs.SetInt("goldAd", 0);

        // 플래티넘
        if (PlayerPrefs.HasKey("platinumAd"))
            platinumAd = PlayerPrefs.GetInt("platinumAd");
        else
            PlayerPrefs.SetInt("platinumAd", 0);

        // 다이아
        if (PlayerPrefs.HasKey("diaAd"))
            diaAd = PlayerPrefs.GetInt("diaAd");
        else
            PlayerPrefs.SetInt("diaAd", 0);


        // 퀘스트 수
        // 브론즈
        if (PlayerPrefs.HasKey("bronzQ"))
            bronzQ = PlayerPrefs.GetInt("bronzQ");
        else
            PlayerPrefs.SetInt("bronzQ", 0);

        // 실버
        if (PlayerPrefs.HasKey("silverQ"))
            silverQ = PlayerPrefs.GetInt("silverQ");
        else
            PlayerPrefs.SetInt("silverQ", 0);

        // 골드
        if (PlayerPrefs.HasKey("goldQ"))
            goldQ = PlayerPrefs.GetInt("goldQ");
        else
            PlayerPrefs.SetInt("goldQ", 0);

        // 플래티넘
        if (PlayerPrefs.HasKey("platinumQ"))
            platinumQ = PlayerPrefs.GetInt("platinumQ");
        else
            PlayerPrefs.SetInt("platinumQ", 0);

        // 다이아
        if (PlayerPrefs.HasKey("diaQ"))
            diaQ = PlayerPrefs.GetInt("diaQ");
        else
            PlayerPrefs.SetInt("diaQ", 0);
    }

    public void SetDetachQuestListData()
    {
        userQuestList.Clear();

        var questId = PlayerPrefs.GetString("QuestId");
        var questIds = questId.Split(',');

        if (questId == "") return;

        foreach (var item in questIds)
        {
            int questIdOfInt = Convert.ToInt32(item);
            if (usingQuestList.Contains(questIdOfInt)) continue;

            var data = DataTableManager.Instance.GetQuestData(questIdOfInt);

            userQuestList.Add(data);
        }
    }

    public void SetQuestListData()
    {
        userQuestList.Clear();

        var questId = PlayerPrefs.GetString("QuestId");
        var questIds = questId.Split(',');

        if (questId == "") return;

        foreach (var item in questIds)
        {
            int questIdOfInt = Convert.ToInt32(item);
            var data = DataTableManager.Instance.GetQuestData(questIdOfInt);

            userQuestList.Add(data);
        }
    }

    public void SetDetachAdventureListData()
    {
        userAdventureList.Clear();

        var adventureId = PlayerPrefs.GetString("AdventureId");
        var adventureIds = adventureId.Split(',');

        if (adventureId == "") return;

        foreach(var item in adventureIds)
        {
            int adventureIdOfInt = Convert.ToInt32(item);
            if (usingAdventureList.Contains(adventureIdOfInt)) continue;

            var data = DataTableManager.Instance.GetAdventureData(adventureIdOfInt);

            userAdventureList.Add(data);
        }
    }

    public void SetAdventureListData()
    {
        userAdventureList.Clear();

        var adventureId = PlayerPrefs.GetString("AdventureId");
        var adventureIds = adventureId.Split(',');

        if (adventureId == "") return;

        foreach (var item in adventureIds)
        {
            int adventureIdOfInt = Convert.ToInt32(item);
            var data = DataTableManager.Instance.GetAdventureData(adventureIdOfInt);

            userAdventureList.Add(data);
        }
    }

    public void UsingQuestData()   // 선택된 의뢰 사용중이라고 표현
    {
        usingQuestList.Add(QuestData.questSelectedId);
    }

    public void UsingAdventureData()   // 선택된 모험가 사용중이라고 표현
    {
        foreach(int i in AdventureData.adventureSelectId)
        {
            usingAdventureList.Add(i);
        }
    }

    public void BtnActive(int index)
    {
        questBtn[index - 1].interactable = true;
        adventureBtn[index - 1].interactable = true;
    }

    protected override void Dispose()
    {
        base.Dispose();

        // 브론즈
        PlayerPrefs.SetInt("bronzAd", bronzAd);

        // 실버
        PlayerPrefs.SetInt("silverAd", silverAd);

        // 골드
        PlayerPrefs.SetInt("goldAd", goldAd);

        // 플래티넘
        PlayerPrefs.SetInt("platinumAd", platinumAd);

        // 다이아
        PlayerPrefs.SetInt("diaAd", diaAd);
    }

    public void SetFalseIsNotTutorialTouch()
    {
        isNotTutorialTouch = false;
    }
}
