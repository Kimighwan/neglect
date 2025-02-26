using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            Debug.Log("의뢰를 더 이상 수용할 수 없음");
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

        // 골드 차감


        btnTxt.text = "영입 완료";
        btn.interactable = false;
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
    }

    private int RandomIndexMake()   // 무작위 숫자
    {
        int randomIndexA = UnityEngine.Random.Range(1, 10);     // 브론즈   9개
        int randomIndexB = GetQuestindex(Tier.Silver);          // 실버     10개
        int randomIndexC = GetQuestindex(Tier.Gold);            // 골드     10개
        int randomIndexD = GetQuestindex(Tier.Platinum);        // 플래티넘 7개
        int randomIndexE = UnityEngine.Random.Range(38, 41);    // 다이아   3개

        int resultValue = UnityEngine.Random.Range(1, 101);

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

        if (!PoolManager.Instance.userQuestIndex.Contains(resultId))
        {
            PoolManager.Instance.userQuestIndex.Add(resultId);
        }
        else
        {
            // 길드레벨 3
            if (GameInfo.gameInfo.Level == 3)
            {
                if(j == 0) // 브론즈
                {
                    // 실버 의뢰로 승격
                    resultId = GetQuestindex(Tier.Silver);   // 실버
                }
                else if(j == 3) // 플래티넘 의뢰 부족
                {
                    // 아쉽게 골드 중 갖고 있지 않는 의뢰 반환
                    int tmp;
                    do
                    {
                        tmp = GetQuestindex(Tier.Gold);
                    } while (PoolManager.Instance.userQuestIndex.Contains(tmp));

                    resultId = tmp;
                }
            }
            // 길드레벨 4, 5
            else
            {
                if (j == 1) // 실버 의뢰 부족
                {
                    // 골드 의뢰로 승격
                    int tmp;
                    do
                    {
                        tmp = GetQuestindex(Tier.Gold); // 골드
                    } while (PoolManager.Instance.userQuestIndex.Contains(tmp));

                    resultId = tmp;
                }
                else if (j == 2) // 골드 의뢰 부족
                {
                    // 실버 의뢰 주기
                    int tmp;
                    do
                    {
                        tmp = GetQuestindex(Tier.Silver); // 실버
                    } while (PoolManager.Instance.userQuestIndex.Contains(tmp));

                    resultId = tmp;
                }
                else if (j == 3) // 플래티넘 의뢰 부족
                {
                    // 골드 의뢰 주기
                    int tmp;
                    do
                    {
                        tmp = GetQuestindex(Tier.Gold); // 골드
                    } while (PoolManager.Instance.userQuestIndex.Contains(tmp));
                }
                else // 다이아 의뢰 부족
                {
                    if (!CheckQuestFullOfTier(Tier.Platinum))   // 플래티넘 의뢰가 아직 있다면
                    {
                        // 플래티넘 의뢰 주기
                        int tmp;
                        do
                        {
                            tmp = GetQuestindex(Tier.Platinum); // 플래티넘
                        } while (PoolManager.Instance.userQuestIndex.Contains(tmp));
                    }
                    else // 플래티넘도 없다면 골드 의뢰 주기
                    {
                        int tmp;
                        do
                        {
                            tmp = GetQuestindex(Tier.Gold); // 골드
                        } while (PoolManager.Instance.userQuestIndex.Contains(tmp));
                    }
                }
            }
        }

        return resultId;
    }

    private bool CheckQuestFullOfTier(Tier tier)    // 랭크에 해당하는 의뢰가 더이상 없는가
    {
        switch (tier)
        {
            case Tier.Bronze:
                for(int i = 1; i < 10; i++)
                {
                    if (!PoolManager.Instance.userQuestIndex.Contains(i))
                    {
                        return false;
                    }
                }
                break;
            case Tier.Silver:
                for (int i = 10; i < 20; i++)
                {
                    if (!PoolManager.Instance.userQuestIndex.Contains(i))
                    {
                        return false;
                    }
                }
                break;
            case Tier.Gold:
                for (int i = 20; i < 30; i++)
                {
                    if (!PoolManager.Instance.userQuestIndex.Contains(i))
                    {
                        return false;
                    }
                }
                break;
            case Tier.Platinum:
                for (int i = 31; i < 38; i++)
                {
                    if (!PoolManager.Instance.userQuestIndex.Contains(i))
                    {
                        return false;
                    }
                }
                break;
            case Tier.Dia:
                for (int i = 38; i < 41; i++)
                {
                    if (!PoolManager.Instance.userQuestIndex.Contains(i))
                    {
                        return false;
                    }
                }
                break;
            default:
                break;
        }

        return true;    // 없다
    }

    private int GetQuestindex(Tier tier)
    {
        if(tier == Tier.Silver)
        {
            int tmpResultValue = UnityEngine.Random.Range(1, 101);
            if (tmpResultValue <= 90)
            {
                return UnityEngine.Random.Range(10, 19);
            }
            else
                return 19;
        }
        else if(tier == Tier.Gold)
        {
            int tmpResultValue = UnityEngine.Random.Range(1, 101);
            if (tmpResultValue <= 90)
            {
                return UnityEngine.Random.Range(20, 29);
            }
            else
                return 29;
        }
        else if(tier == Tier.Platinum)
        {
            int tmpResultValue = UnityEngine.Random.Range(1, 101);
            if (tmpResultValue <= 90)
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
