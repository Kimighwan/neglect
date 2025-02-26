using System;
using TMPro;
using UnityEngine;

public class RandomAdventureSelectUI : MonoBehaviour
{
    public TextMeshProUGUI c_name;
    public TextMeshProUGUI position;
    public TextMeshProUGUI m_class;
    public TextMeshProUGUI type;
    public TextMeshProUGUI needGoldText;


    private AdventureData adventureData;

    private int adventureId;

    private int j;
    private int resultId;
    private int needGold;

    private string adventureName;
    private string adventurePosition;
    private string adventureClass;      // 계열
    private string adventureType;       // 타입
    private string adventureTier;       // 테두리 표현

    private Transform pos;              // 모험가 종이 위치


    private void Awake()
    {
        var rectTransform = GetComponent<RectTransform>();

        pos = GameObject.FindGameObjectWithTag("AdventureSelectGroup").transform;

        this.transform.SetParent(pos);
        rectTransform.sizeDelta = new Vector2(311.3f, 515.6f);

        GetAdventureData();
        InitData();
    }

    public void OnClickSelected()
    {
        if (GameInfo.gameInfo.Gold < needGold)
        {
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK;
            uiData.descTxt = "골드가 부족합니다.";
            uiData.okBtnTxt = "확인";
            UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            return;
        }

        // 모험가 더 이상 영입 불가능
        if(CheckMaxAdventureCounts())
        {
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK;
            uiData.descTxt = "모험가 최대치";
            uiData.okBtnTxt = "확인";
            UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            return;
        }

        // 골드 차감

        if (CheckHaveAdventureID(adventureId))  // 선택된 모험가가 이미 있음
        {
            Debug.Log("해당 모험가를 이미 가지고 있음");
            return;
        }

        string pre = PlayerPrefs.GetString("AdventureId");  // 저장된 모험가 ID 불러오기

        // 영입된 모험가 저장하기
        if (pre == "")
        {
            PlayerPrefs.SetString("AdventureId", adventureId.ToString());
        }
        else
        {
            PlayerPrefs.SetString("AdventureId", pre + "," + adventureId.ToString());
        }
    }

    private void GetAdventureData()
    {
        adventureData = DataTableManager.Instance.GetRandomAdventureData(RandomIndexMake());

        adventureId = adventureData.adventureId;
        this.adventureName = adventureData.adventureName;
        this.adventurePosition = adventureData.adventurePosition;
        this.adventureClass = adventureData.adventureClass;
        this.adventureType = adventureData.adventureType;
        this.adventureTier = adventureData.adventureTier;
        
        if(adventureTier == "브론즈")
        {
            needGold = 200;
            needGoldText.text = "영입 200 골드";
        }
        else if (adventureTier == "실버")
        {
            needGold = 500;
            needGoldText.text = "영입 500 골드";
        }
        else if (adventureTier == "골드")
        {
            needGold = 1000;
            needGoldText.text = "영입 100 골드";
        }
        else if (adventureTier == "플래티넘")
        {
            needGold = 2000;
            needGoldText.text = "영입 2000 골드";
        }
        else if (adventureTier == "다이아")
        {
            needGold = 5000;
            needGoldText.text = "영입 5000 골드";
        }
    }

    private void InitData()
    {
        c_name.text = "이름 : " + adventureName;
        position.text = adventurePosition;
        type.text = adventureType;
        m_class.text = adventureClass;
    }

    private int RandomIndexMake()
    {
        int randomIndexA = UnityEngine.Random.Range(1, 19);     // 브론즈   18개
        int randomIndexB = UnityEngine.Random.Range(19, 44);    // 실버     25개
        int randomIndexC = UnityEngine.Random.Range(44, 73);    // 골드     29개
        int randomIndexD = UnityEngine.Random.Range(73, 88);    // 플래티넘 15개
        int randomIndexE = UnityEngine.Random.Range(88, 91);    // 다이아   3개

        int resultValue = UnityEngine.Random.Range(1, 101);

        int[] probability = new int[5];

        switch (GameInfo.gameInfo.Level)
        {
            case 1:
                probability[0] = 79; probability[1] = 20; probability[2] = 1; probability[3] = 0; probability[4] = 0;
                break;
            case 2:
                probability[0] = 44; probability[1] = 40; probability[2] = 15; probability[3] = 1; probability[4] = 0;
                break;
            case 3:
                probability[0] = 19; probability[1] = 40; probability[2] = 30; probability[3] = 10; probability[4] = 1;
                break;
            case 4:
                probability[0] = 0; probability[1] = 20; probability[2] = 50; probability[3] = 25; probability[4] = 5;
                break;
            case 5:
                probability[0] = 0; probability[1] = 0; probability[2] = 50; probability[3] = 40; probability[4] = 10;
                break;
        }

        int cumulativeProbability = 0;      // 누적된 확률

        for (j = 0; j < 5; j++)
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

        if (!PoolManager.Instance.userAdventureIndex.Contains(resultId))
        {
            PoolManager.Instance.userAdventureIndex.Add(resultId);
        }
        else
        {
            if (j == 0) // 브론즈 부족
            {
                // 실버 주기
                int tmp;
                do
                {
                    tmp = UnityEngine.Random.Range(19, 44);
                } while (PoolManager.Instance.userAdventureIndex.Contains(tmp));
            }
            else if (j == 3) // 플래티넘 부족
            {
                // 아쉽게 골드 중 갖고 있지 않는 모험가 반환
                int tmp;
                do
                {
                    tmp = UnityEngine.Random.Range(44, 73);
                } while (PoolManager.Instance.userAdventureIndex.Contains(tmp));

                resultId = tmp;
            }
            else if (j == 4) // 다이아 부족
            {
                if (!CheckAdventureFullOfTier(Tier.Platinum))
                {
                    // 아쉽게 플래티넘 중 갖고 있지 않는 모험가 반환
                    int tmp;
                    do
                    {
                        tmp = UnityEngine.Random.Range(73, 88);
                    } while (PoolManager.Instance.userAdventureIndex.Contains(tmp));

                    resultId = tmp;
                }
                else // 플래티넘도 부족하기에 골드를 반환
                {
                    int tmp;
                    do
                    {
                        tmp = UnityEngine.Random.Range(44, 73);
                    } while (PoolManager.Instance.userAdventureIndex.Contains(tmp));

                    resultId = tmp;
                }
            }
        }

        return resultId;
    }

    private bool CheckHaveAdventureID(int adventureId)                      // 매개변수의 모험가 ID를 가졌는지 확인
    {
        string adventureIdOfString = PlayerPrefs.GetString("AdventureId");  // 현재 모험가 ID 가져오기
        string[] adventureIdOfInt = adventureIdOfString.Split(',');         // 구분자 모험가 ID 분리

        if (adventureIdOfString == "") return false;

        for (int index = 0; index < adventureIdOfInt.Length; index++)        // 모든 모험가 ID 순회
        {
            if(adventureId == Convert.ToInt32(adventureIdOfInt[index]))     // 매개변수와 같은 모험가 ID 검색
            {
                return true;    // 해당 모험가가 있음
            }
        }

        return false;           // 해당 모험가가 없음
    }
    
    private bool CheckMaxAdventureCounts()    // 객실 수 및 레벨 확인하여 최대 모험가 수를 넘는지 체크
    {
        int tmpCount = 0;
        string adventrueHave = PlayerPrefs.GetString("AdventureId");
        foreach(var i in adventrueHave.Split(","))
        {
            if (i != "")
                tmpCount++;
        }
        return tmpCount >= GameInfo.gameInfo.GetMaxAdventurerCounts();    // true : 더이상 수용 불가능
    }

    private bool CheckAdventureFullOfTier(Tier tier)    // 랭크에 해당하는 모험가가 더이상 없는가
    {
        switch (tier)
        {
            case Tier.Platinum:
                for (int i = 73; i < 88; i++)
                {
                    if (!PoolManager.Instance.userAdventureIndex.Contains(i))
                    {
                        return false;   // 아직 여유가 있다.
                    }
                }
                break;
            default:
                break;
        }

        return true;    // 없다
    }
}
