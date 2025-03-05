using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class RandomQuestSelectUI : MonoBehaviour
{
    public RawImage rankImage;

    public TextMeshProUGUI m_name;
    public TextMeshProUGUI level;
    public TextMeshProUGUI time;
    public TextMeshProUGUI reward;

    public Button btn;
    public TextMeshProUGUI btnTxt;

    private QuestData questData;

    private int questId;
    private int questTime;
    private int questReward;
    private int questMonsterDescId;

    private int j;
    private int resultId;


    private string questName;
    private string questLevel;

    private Transform pos;  // 의뢰 종이 위치


    private void Awake()
    {
        var rectTransform = GetComponent<RectTransform>();

        pos = GameObject.FindGameObjectWithTag("QeustSelectGroup").transform;

        this.transform.SetParent(pos);
        rectTransform.sizeDelta = new Vector2(300, 400);
        rectTransform.localScale = new Vector3(1f, 1f, 1f);

        GetQuestData();
        InitData();
    }

    public void OnClickSelected()
    {
        if (CheckHaveAdventureID(questId))  // 선택된 의뢰가 이미 있음
            return;

        if(CheckMaxQuest())
        {
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK;
            uiData.descTxt = "의뢰 최대치";
            uiData.okBtnTxt = "확인";
            AudioManager.Instance.PlaySFX(SFX.Denied);
            UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            return;
        }

        string pre = PlayerPrefs.GetString("QuestId");  // 저장된 quest ID 불러오기

        // 선택한 quest ID 저장
        if (pre == "")
        {
            PlayerPrefs.SetString("QuestId", questId.ToString());
        }
        else
        {
            PlayerPrefs.SetString("QuestId", pre + "," + questId.ToString());
        }

        if (questLevel == "브론즈")
            PoolManager.Instance.bronzQ++;
        else if (questLevel == "실버")
            PoolManager.Instance.silverQ++;
        else if (questLevel == "골드")
            PoolManager.Instance.goldQ++;
        else if (questLevel == "플래티넘")
            PoolManager.Instance.platinumQ++;
        else if (questLevel == "다이아")
            PoolManager.Instance.diaQ++;


        btnTxt.text = "선택 완료";
        btn.interactable = false;

        if (((questId / 100) % 10) == 8)
        {
            // 특수 의뢰 받고 나서 실행
            PoolManager.Instance.checkHavesSpecialQuest = questId;
        }
    }

    private void GetQuestData()
    {
        questData = DataTableManager.Instance.GetQuestDataUsingIndex(RandomIndexMake());

        questId = questData.questId;
        this.questName = questData.questName;
        this.questLevel = questData.questLevel;
        this.questTime = questData.questTime;
        this.questReward = questData.questReward;
        this.questMonsterDescId = questData.questMonsterDescId;
    }

    private void InitData()
    {
        m_name.text = questName;
        level.text = questLevel;
        time.text = questTime.ToString();
        reward.text = questReward.ToString();

        if(questLevel == "브론즈")
            rankImage.texture = Resources.Load("Arts/QuestRank/bronze_quest") as Texture2D;
        else if (questLevel == "실버")
            rankImage.texture = Resources.Load("Arts/QuestRank/silver_quest") as Texture2D;
        else if (questLevel == "골드")
            rankImage.texture = Resources.Load("Arts/QuestRank/gold_quest") as Texture2D;
        else if (questLevel == "플래티넘")
            rankImage.texture = Resources.Load("Arts/QuestRank/platinum_quest") as Texture2D;
        else if (questLevel == "다이아")
            rankImage.texture = Resources.Load("Arts/QuestRank/diamond_quest") as Texture2D;

        if ((questId / 100) % 10 == 8)
            rankImage.texture = Resources.Load("Arts/QuestRank/special_quest") as Texture2D;
    }

    private int RandomIndexMake()   // 무작위 숫자
    {
        int randomIndexA = UnityEngine.Random.Range(1, 10);     // 브론즈   9개
        int randomIndexB = GetQuestindex(Tier.Silver);          // 실버     10개
        int randomIndexC = GetQuestindex(Tier.Gold);            // 골드     10개
        int randomIndexD = GetQuestindex(Tier.Platinum);        // 플래티넘 7개
        int randomIndexE = UnityEngine.Random.Range(38, 41);    // 다이아   3개

        int resultValue = UnityEngine.Random.Range(1, 101);
        Debug.Log(resultValue);
        int[] probability = new int[5];

        switch (GameInfo.gameInfo.Level)
        {
            case 1:
                probability[0] = 80; probability[1] = 20; probability[2] = 0; probability[3] = 0; probability[4] = 0;
                break;
            case 2:
                probability[0] = 45; probability[1] = 40; probability[2] = 15; probability[3] = 0; probability[4] = 0;
                break;
            case 3:
                probability[0] = 20; probability[1] = 45; probability[2] = 30; probability[3] = 5; probability[4] = 0;
                break;
            case 4:
                probability[0] = 0; probability[1] = 35; probability[2] = 50; probability[3] = 10; probability[4] = 5;
                break;
            case 5:
                probability[0] = 0; probability[1] = 10; probability[2] = 60; probability[3] = 25; probability[4] = 5;
                break;
        }

        int cumulativeProbability = 0;      // 누적된 확률

        for(j = 0; j < 5; j++)
        {
            cumulativeProbability += probability[j];

            if (resultValue <= cumulativeProbability)
            {
                break;
            }
        }

        Debug.Log($"j : {j}");

        switch (j)
        {
            case 0: // 브론즈
                resultId = randomIndexA;
                break;
            case 1: // 실버
                resultId = randomIndexB;
                break;
            case 2: // 골드
                resultId = randomIndexC;
                break;
            case 3: // 플래티넘
                resultId = randomIndexD;
                break;
            case 4: // 다이아
                resultId = randomIndexE;
                break;
            default:
                break;
        }

        if (!PoolManager.Instance.userQuestIndex.Contains(resultId) && !CheckHaveRandomIndex(resultId))
        {
            Debug.Log($"{resultId} 의뢰 뽑음"); 
            PoolManager.Instance.userQuestIndex.Add(resultId);
        }
        else
        {
            Debug.Log($"{resultId} 이미 있어서 뺑뺑이 돌림");
            // 길드레벨 1, 2
            // 의뢰가 부족해서 다른 티어 의뢰 보여줄 일이 없음
            if (GameInfo.gameInfo.Level == 1 || GameInfo.gameInfo.Level == 2)
            {
                int tmp = resultId;
                // 브론즈 의뢰 보여줄 예정이였다면
                if (j == 0)
                {
                    // 브론즈 의뢰 중 보여지지 않는 의뢰 선택
                    if(PoolManager.Instance.bronzQ < 6)
                    {
                        do
                        {
                            tmp = UnityEngine.Random.Range(1, 10);
                        } while (PoolManager.Instance.userQuestIndex.Contains(tmp) || CheckHaveRandomIndex(tmp));

                    }
                    else
                    {
                        for(int i = 1; i < 10; i++)
                        {
                            tmp = i;
                            if (!PoolManager.Instance.userQuestIndex.Contains(i) && !CheckHaveRandomIndex(tmp))
                                break;
                        }
                    }
                    
                    resultId = tmp;
                    PoolManager.Instance.userQuestIndex.Add(tmp);
                }
                else if (j == 1) // 실버 의뢰 보여줄 예정이였다면
                {
                    // 실버 의뢰 중 보여지지 않는 의뢰 선택
                    if (PoolManager.Instance.silverQ < 7)
                    {
                        do
                        {
                            tmp = GetQuestindex(Tier.Silver);          // 실버     10개
                        } while (PoolManager.Instance.userQuestIndex.Contains(tmp) || CheckHaveRandomIndex(tmp));

                    }
                    else
                    {
                        for (int i = 10; i < 20; i++)
                        {
                            tmp = i;
                            if (!PoolManager.Instance.userQuestIndex.Contains(i) && !CheckHaveRandomIndex(tmp))
                                break;
                        }
                    }

                    resultId = tmp;
                    PoolManager.Instance.userQuestIndex.Add(tmp);
                }
                else if (j == 2) // 골드 의뢰 보여줄 예정이였다면
                {
                    // 골드 의뢰 중 보여지지 않는 의뢰 선택
                    if (PoolManager.Instance.goldQ < 7)
                    {
                        do
                        {
                            tmp = GetQuestindex(Tier.Gold);            // 골드     10개
                        } while (PoolManager.Instance.userQuestIndex.Contains(tmp) || CheckHaveRandomIndex(tmp));

                    }
                    else
                    {
                        for (int i = 20; i < 30; i++)
                        {
                            tmp = i;
                            if (!PoolManager.Instance.userQuestIndex.Contains(i) && !CheckHaveRandomIndex(tmp))
                                break;
                        }
                    }

                    resultId = tmp;
                    PoolManager.Instance.userQuestIndex.Add(tmp);
                }
            }
            // 길드레벨 3
            else if (GameInfo.gameInfo.Level == 3)
            {
                int tmp = resultId;

                if(j == 0) // 브론즈
                {
                    // 남는 브론즈 의뢰가 없음
                    if (PoolManager.Instance.bronzQ >= 9)
                    {
                        // 실버 주자
                        if (PoolManager.Instance.silverQ < 7)
                        {
                            do
                            {
                                tmp = GetQuestindex(Tier.Silver);          // 실버     10개
                            } while (PoolManager.Instance.userQuestIndex.Contains(tmp) || CheckHaveRandomIndex(tmp));

                        }
                        else
                        {
                            for (int i = 10; i < 20; i++)
                            {
                                tmp = i;
                                if (!PoolManager.Instance.userQuestIndex.Contains(i) && !CheckHaveRandomIndex(tmp))
                                    break;
                            }
                        }

                        resultId = tmp;
                        PoolManager.Instance.userQuestIndex.Add(tmp);
                    }
                    else // 남는 브론즈 의뢰가 있음
                    {
                        // 브론즈 의뢰 중 보여지지 않는 의뢰 선택
                        if (PoolManager.Instance.bronzQ < 6)
                        {
                            do
                            {
                                tmp = UnityEngine.Random.Range(1, 10);
                            } while (PoolManager.Instance.userQuestIndex.Contains(tmp) || CheckHaveRandomIndex(tmp));

                        }
                        else
                        {
                            for (int i = 1; i < 10; i++)
                            {
                                tmp = i;
                                if (!PoolManager.Instance.userQuestIndex.Contains(i) && !CheckHaveRandomIndex(tmp))
                                    break;
                            }
                        }

                        resultId = tmp;
                        PoolManager.Instance.userQuestIndex.Add(tmp);
                    }
                }
                else if(j == 1) // 실버 의뢰를 줘야할 예정이라면
                {
                    // 실버 주자
                    if (PoolManager.Instance.silverQ < 7)
                    {
                        do
                        {
                            tmp = GetQuestindex(Tier.Silver);          // 실버     10개
                        } while (PoolManager.Instance.userQuestIndex.Contains(tmp) || CheckHaveRandomIndex(tmp));

                    }
                    else
                    {
                        for (int i = 10; i < 20; i++)
                        {
                            tmp = i;
                            if (!PoolManager.Instance.userQuestIndex.Contains(i) && !CheckHaveRandomIndex(tmp))
                                break;
                        }
                    }

                    resultId = tmp;
                    PoolManager.Instance.userQuestIndex.Add(tmp);

                }
                else if (j == 2) // 골드 의뢰를 줘야할 예정이라면
                {
                    // 골드 주자
                    if (PoolManager.Instance.goldQ < 7)
                    {
                        do
                        {
                            tmp = GetQuestindex(Tier.Gold);          // 골드     10개
                        } while (PoolManager.Instance.userQuestIndex.Contains(tmp) || CheckHaveRandomIndex(tmp));

                    }
                    else
                    {
                        for (int i = 20; i < 30; i++)
                        {
                            tmp = i;
                            if (!PoolManager.Instance.userQuestIndex.Contains(i) && !CheckHaveRandomIndex(tmp))
                                break;
                        }
                    }

                    resultId = tmp;
                    PoolManager.Instance.userQuestIndex.Add(tmp);
                }
            }
            // 길드레벨 4, 5
            else if(GameInfo.gameInfo.Level == 4 || GameInfo.gameInfo.Level == 5)
            {
                int tmp = resultId;

                if(j == 1)  // 실버 의뢰 줘야했다면 남는 실버 또는 골드 주기
                {
                    if(PoolManager.Instance.silverAd < 10)
                    {
                        // 실버 의뢰 남아서 실버 의뢰 주기
                        // 실버 의뢰 중 보여지지 않는 의뢰 선택
                        if (PoolManager.Instance.silverAd < 7)
                        {
                            do
                            {
                                tmp = GetQuestindex(Tier.Silver);          // 실버     10개
                            } while (PoolManager.Instance.userQuestIndex.Contains(tmp) || CheckHaveRandomIndex(tmp));

                        }
                        else
                        {
                            for (int i = 10; i < 20; i++)
                            {
                                tmp = i;
                                if (!PoolManager.Instance.userQuestIndex.Contains(i) && !CheckHaveRandomIndex(tmp))
                                    break;
                            }
                        }

                        resultId = tmp;
                        PoolManager.Instance.userQuestIndex.Add(tmp);
                    }
                    else
                    {
                        // 실버 의뢰 없어서 골드 의뢰 주기
                        // 골드 의뢰 중 보여지지 않는 의뢰 선택
                        if (PoolManager.Instance.goldQ < 7)
                        {
                            do
                            {
                                tmp = GetQuestindex(Tier.Gold);            // 골드     10개
                            } while (PoolManager.Instance.userQuestIndex.Contains(tmp) || CheckHaveRandomIndex(tmp));

                        }
                        else
                        {
                            for (int i = 20; i < 30; i++)
                            {
                                tmp = i;
                                if (!PoolManager.Instance.userQuestIndex.Contains(i) && !CheckHaveRandomIndex(tmp))
                                    break;
                            }
                        }

                        resultId = tmp;
                        PoolManager.Instance.userQuestIndex.Add(tmp);
                    }
                }
                else if (j == 2) // 골드 의뢰 줘야했다면 남는 골드 또는 실버 주기
                {
                    if(PoolManager.Instance.goldQ < 10)
                    {
                        // 골드 의뢰가 남아서 골드 주기
                        // 골드 의뢰 중 보여지지 않는 의뢰 선택
                        if (PoolManager.Instance.goldQ < 7)
                        {
                            do
                            {
                                tmp = GetQuestindex(Tier.Gold);            // 골드     10개
                            } while (PoolManager.Instance.userQuestIndex.Contains(tmp) || CheckHaveRandomIndex(tmp));

                        }
                        else
                        {
                            for (int i = 20; i < 30; i++)
                            {
                                tmp = i;
                                if (!PoolManager.Instance.userQuestIndex.Contains(i) && !CheckHaveRandomIndex(tmp))
                                    break;
                            }
                        }

                        resultId = tmp;
                        PoolManager.Instance.userQuestIndex.Add(tmp);
                    }
                    else
                    {
                        // 골드 의뢰 없어서 실버 주기
                        // 실버 의뢰 중 보여지지 않는 의뢰 선택
                        if (PoolManager.Instance.silverAd < 7)
                        {
                            do
                            {
                                tmp = GetQuestindex(Tier.Silver);          // 실버     10개
                            } while (PoolManager.Instance.userQuestIndex.Contains(tmp) || CheckHaveRandomIndex(tmp));

                        }
                        else
                        {
                            for (int i = 10; i < 20; i++)
                            {
                                tmp = i;
                                if (!PoolManager.Instance.userQuestIndex.Contains(i) && !CheckHaveRandomIndex(tmp))
                                    break;
                            }
                        }

                        resultId = tmp;
                        PoolManager.Instance.userQuestIndex.Add(tmp);
                    }
                }
                else if (j == 3) // 플래 의뢰 줘야했다면 남는 플래 또는 골드
                {
                    if (PoolManager.Instance.platinumQ < 7)
                    {
                        // 플래 의뢰가 남아서 플래 주기
                        // 플래 의뢰 중 보여지지 않는 의뢰 선택
                        if (PoolManager.Instance.platinumQ < 5)
                        {
                            do
                            {
                                tmp = GetQuestindex(Tier.Platinum);            // 플래 7개
                            } while (PoolManager.Instance.userQuestIndex.Contains(tmp) || CheckHaveRandomIndex(tmp));

                        }
                        else
                        {
                            for (int i = 31; i < 38; i++)
                            {
                                tmp = i;
                                if (!PoolManager.Instance.userQuestIndex.Contains(i) && !CheckHaveRandomIndex(tmp))
                                    break;
                            }
                        }

                        resultId = tmp;
                        PoolManager.Instance.userQuestIndex.Add(tmp);
                    }
                    else if (PoolManager.Instance.goldQ < 10)
                    {
                        // 플래 의뢰가 없어서 골드 주기
                        // 골드 의뢰 중 보여지지 않는 의뢰 선택
                        if (PoolManager.Instance.goldQ < 7)
                        {
                            do
                            {
                                tmp = GetQuestindex(Tier.Gold);            // 골드     10개
                            } while (PoolManager.Instance.userQuestIndex.Contains(tmp) || CheckHaveRandomIndex(tmp));

                        }
                        else
                        {
                            for (int i = 20; i < 30; i++)
                            {
                                tmp = i;
                                if (!PoolManager.Instance.userQuestIndex.Contains(i) && !CheckHaveRandomIndex(tmp))
                                    break;
                            }
                        }

                        resultId = tmp;
                        PoolManager.Instance.userQuestIndex.Add(tmp);
                    }
                }
                else if (j == 4) // 다이아 의뢰 줘야했다면 남는 다이아 또는 플래 또는 골드
                {
                    Debug.Log($"{resultId}가 다이아라서 남은 다이아 또는 플래 또는 골드 선택");
                    if (PoolManager.Instance.diaQ < 3)
                    {
                        for (int i = 38; i < 41; i++)
                        {
                            tmp = i;
                            if (!PoolManager.Instance.userQuestIndex.Contains(i) && !CheckHaveRandomIndex(tmp))
                            {
                                Debug.Log($"{tmp} 뺑뻉이로 다이아 의뢰가 선택됨");
                                break;
                            }
                        }

                        resultId = tmp;
                        PoolManager.Instance.userQuestIndex.Add(tmp);
                    }
                    else if (PoolManager.Instance.platinumQ < 7)
                    {
                        // 플래 의뢰가 남아서 플래 주기
                        // 플래 의뢰 중 보여지지 않는 의뢰 선택
                        if (PoolManager.Instance.platinumQ < 5)
                        {
                            do
                            {
                                tmp = GetQuestindex(Tier.Platinum);            // 플래 7개
                            } while (PoolManager.Instance.userQuestIndex.Contains(tmp) || CheckHaveRandomIndex(tmp));

                        }
                        else
                        {
                            for (int i = 31; i < 38; i++)
                            {
                                tmp = i;
                                if (!PoolManager.Instance.userQuestIndex.Contains(i) && !CheckHaveRandomIndex(tmp))
                                    break;
                            }
                        }

                        resultId = tmp;
                        PoolManager.Instance.userQuestIndex.Add(tmp);
                    }
                    else if (PoolManager.Instance.goldQ < 10)
                    {
                        // 플래 의뢰가 없어서 골드 주기
                        // 골드 의뢰 중 보여지지 않는 의뢰 선택
                        if (PoolManager.Instance.goldQ < 7)
                        {
                            do
                            {
                                tmp = GetQuestindex(Tier.Gold);            // 골드     10개
                            } while (PoolManager.Instance.userQuestIndex.Contains(tmp) || CheckHaveRandomIndex(tmp));

                        }
                        else
                        {
                            for (int i = 20; i < 30; i++)
                            {
                                tmp = i;
                                if (!PoolManager.Instance.userQuestIndex.Contains(i) && !CheckHaveRandomIndex(tmp))
                                    break;
                            }
                        }

                        resultId = tmp;
                        PoolManager.Instance.userQuestIndex.Add(tmp);
                    }
                }
            }
        }

        return resultId;
    }

    

    private int GetQuestindex(Tier tier)
    {
        if(tier == Tier.Silver)
        {
            int tmpResultValue = UnityEngine.Random.Range(1, 101);
            if (tmpResultValue <= 90 || PlayerPrefs.GetInt("설녀") == 1)
            {
                return UnityEngine.Random.Range(10, 19);
            }
            else
                return 19;
        }
        else if(tier == Tier.Gold)
        {
            int tmpResultValue = UnityEngine.Random.Range(1, 101);
            if (tmpResultValue <= 90 || PlayerPrefs.GetInt("호문쿨루스") == 1)
            {
                return UnityEngine.Random.Range(20, 29);
            }
            else
                return 29;
        }
        else if(tier == Tier.Platinum )
        {
            int tmpResultValue = UnityEngine.Random.Range(1, 101);
            if (tmpResultValue <= 90 || PlayerPrefs.GetInt("헤츨링") == 1)
            {
                return UnityEngine.Random.Range(31, 37);
            }
            else
                return 37;
        }

        return -1;
    }

    private void SetQuestDetailID()
    {
        DataTableManager.Instance.questDetailId = questId;
    }

    public void OnClickDetailBtn()  // 의뢰 세부사항 UI 열기
    {
        DataTableManager.Instance.monsterDescId = questMonsterDescId;
        SetQuestDetailID();

        var questDetailUI = new BaseUIData();
        UIManager.Instance.CloseUI(UIManager.Instance.GetActiveUI<TodayQuestUI>());
        UIManager.Instance.OpenUI<QuestDetailUI>(questDetailUI);
    }

    public void OnClickDetailBtnOfTodayQuestUI()  // 의뢰 세부사항 UI 열기
    {
        DataTableManager.Instance.monsterDescId = questMonsterDescId;
        SetQuestDetailID();

        var data = new BaseUIData();
        UIManager.Instance.CloseUI(UIManager.Instance.GetActiveUI<TodayQuestUI>());
        UIManager.Instance.OpenUI<QuestDetailUIOfTodayQuest>(data);
    }

    private bool CheckHaveAdventureID(int questId)                  // 매개변수의 ID를 가졌는지 확인
    {
        string questIdOfString = PlayerPrefs.GetString("QuestId");  // 현재 ID 가져오기
        string[] questIdOfInt = questIdOfString.Split(',');         // 구분자 ID 분리

        if (questIdOfString == "") return false;

        for (int index = 0; index < questIdOfInt.Length; index++)        // 모든 ID 순회
        {
            if (questId == Convert.ToInt32(questIdOfInt[index]))     // 매개변수와 같은 ID 검색
            {
                Debug.Log("해당 의뢰가 이미 있습니다.");
                return true;    // 있음
            }
        }

        return false;           // 없음
    }

    private bool CheckHaveRandomIndex(int mid)
    {
        int tmp = DataTableManager.Instance.GetQuestDataUsingIndex(mid).questId;


        string questOfString = PlayerPrefs.GetString("QuestId");  // 현재 의뢰 ID 가져오기
        string[] questIdOfInt = questOfString.Split(',');         // 구분자 의뢰 ID 분리

        if (questOfString == "") return false;

        for (int index = 0; index < questIdOfInt.Length; index++)        // 모든 의뢰 ID 순회
        {
            if (tmp == Convert.ToInt32(questIdOfInt[index]))     // 매개변수와 같은 의뢰 ID 검색
            {
                return true;    // 해당 의뢰 있음
            }
        }

        return false;           // 해당 의뢰 없음
    }

    private bool CheckMaxQuest()    // 의뢰 최대 수량 체크
    {
        int tmpCount = 0;
        string tmpQuest = PlayerPrefs.GetString("QuestId");
        foreach(var i in tmpQuest.Split(","))
        {
            if (i != "")
                tmpCount++;
        }

        if(GameInfo.gameInfo.Level == 1)
        {
            return tmpCount >= 6;
        }
        else if(GameInfo.gameInfo.Level == 2)
        {
            return tmpCount >= 8;
        }
        else if(GameInfo.gameInfo.Level == 3)
        {
            return tmpCount >= 10;
        }
        else if(GameInfo.gameInfo.Level == 4)
        {
            return tmpCount >= 12;
        }
        else if(GameInfo.gameInfo.Level == 5)
        {
            return tmpCount >= 14;
        }
        
        return false;
    }
}

public enum Tier
{
    Bronze,
    Silver,
    Gold,
    Platinum,
    Dia,
}
