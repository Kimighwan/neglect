using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomAdventureSelectUI : MonoBehaviour
{
    public RawImage rankImage;

    public TextMeshProUGUI c_name;
    public TextMeshProUGUI position;
    public TextMeshProUGUI m_class;
    public TextMeshProUGUI type;
    public TextMeshProUGUI needGoldText;

    public RawImage posImg;
    public RawImage classImg;
    public RawImage typeImg;

    public Button selectBtn;

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
        rectTransform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        GetAdventureData();
        InitData();
    }

    public void OnClickSelected()
    {
        if (CheckHaveAdventureID(adventureId))  // 선택된 모험가가 이미 있음
        {
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK;
            uiData.descTxt = "이미 영입된 모험가 입니다.";
            uiData.okBtnTxt = "확인";
            AudioManager.Instance.PlaySFX(SFX.Denied);
            UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            needGoldText.text = "영입 완료";
            return;
        }

        // 모험가 더 이상 영입 불가능
        if (CheckMaxAdventureCounts())
        {
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK;
            uiData.descTxt = "모험가 최대치";
            uiData.okBtnTxt = "확인";
            AudioManager.Instance.PlaySFX(SFX.Denied);
            UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            return;
        }

        if (!GameInfo.gameInfo.ChangeGold(-needGold))
        {
            var uiData = new ConfirmUIData();
            uiData.confirmType = ConfirmType.OK;
            uiData.descTxt = "골드가 부족합니다.";
            uiData.okBtnTxt = "확인";
            AudioManager.Instance.PlaySFX(SFX.Denied);
            UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            return;
        }

        needGoldText.text = "영입 완료";
        selectBtn.interactable = false;

        if (adventureTier == "브론즈")
        {
            PoolManager.Instance.bronzAd++;
        }
        else if (adventureTier == "실버")
        {
            PoolManager.Instance.silverAd++;
        }
        else if (adventureTier == "골드")
        {
            PoolManager.Instance.goldAd++;
        }
        else if (adventureTier == "플래티넘")
        {
            PoolManager.Instance.platinumAd++;
        }
        else if (adventureTier == "다이아")
        {
            PoolManager.Instance.diaAd++;
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
            rankImage.texture = Resources.Load("Arts/Rank/RankBronze") as Texture2D;
            needGoldText.text = "영입 200 골드";
        }
        else if (adventureTier == "실버")
        {
            needGold = 500;
            rankImage.texture = Resources.Load("Arts/Rank/RankSilver") as Texture2D;
            needGoldText.text = "영입 500 골드";
        }
        else if (adventureTier == "골드")
        {
            needGold = 1000;
            rankImage.texture = Resources.Load("Arts/Rank/RankGold") as Texture2D;
            needGoldText.text = "영입 1000 골드";
        }
        else if (adventureTier == "플래티넘")
        {
            needGold = 2000;
            rankImage.texture = Resources.Load("Arts/Rank/RankPlatinum") as Texture2D;
            needGoldText.text = "영입 2000 골드";
        }
        else if (adventureTier == "다이아")
        {
            needGold = 5000;
            rankImage.texture = Resources.Load("Arts/Rank/RankDiamond") as Texture2D;
            needGoldText.text = "영입 5000 골드";
        }
    }

    private void InitData()
    {
        c_name.text = adventureName;
        position.text = adventurePosition;
        type.text = adventureType;
        m_class.text = adventureClass;

        // Postion Image
        if (adventurePosition == "전위")
            posImg.texture = Resources.Load("Arts/Icon/Pos/FrontIcon") as Texture2D;
        else if (adventurePosition == "중위")
            posImg.texture = Resources.Load("Arts/Icon/Pos/MiddleIcon") as Texture2D;
        else
            posImg.texture = Resources.Load("Arts/Icon/Pos/BackIcon") as Texture2D;

        // Class Image
        if (adventureClass == "공격")
            classImg.texture = Resources.Load("Arts/Icon/Class/AttackIcon") as Texture2D;
        else if (adventureClass == "방어")
            classImg.texture = Resources.Load("Arts/Icon/Class/DefenceIcon") as Texture2D;
        else
            classImg.texture = Resources.Load("Arts/Icon/Class/SupportIcon") as Texture2D;

        // Type Image
        if (adventureType == "물리")
            typeImg.texture = Resources.Load("Arts/Icon/Type/PhysicIcon") as Texture2D;
        else if (adventureType == "마법")
            typeImg.texture = Resources.Load("Arts/Icon/Type/MagicIcon") as Texture2D;
        else
            typeImg.texture = Resources.Load("Arts/Icon/Type/HolyIcon") as Texture2D;
    }

    private int RandomIndexMake()
    {
        int randomIndexA = UnityEngine.Random.Range(101, 129);     // 브론즈   28개
        int randomIndexB = UnityEngine.Random.Range(201, 242);    // 실버     41개
        int randomIndexC = UnityEngine.Random.Range(300, 344);    // 골드     44개
        int randomIndexD = UnityEngine.Random.Range(400, 424);    // 플래티넘 24개
        int randomIndexE = UnityEngine.Random.Range(500, 507);    // 다이아   6개

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

        // 보여주지 않고 있거나 && 아직 가지고 있지 않다면
        if (!PoolManager.Instance.userAdventureIndex.Contains(resultId) && !CheckHaveRandomIndex(resultId))
        {
            PoolManager.Instance.userAdventureIndex.Add(resultId);
        }
        else
        {
            if(CheckHaveRandomIndex(resultId)) Debug.Log($"이미 갖고 있음: {resultId} ");
            if (GameInfo.gameInfo.Level == 1)    // 레벨 1
            {
                if (j == 0) // 브론즈 모험가를 보여주기로 했다면
                {
                    // 아직 보여주지 않은 브론즈 주기
                    int tmp = resultId;
                    if(PoolManager.Instance.bronzAd < 28)
                    {
                        if (PoolManager.Instance.bronzAd > 20)
                        {
                            for (int i = 101; i < 129; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = UnityEngine.Random.Range(101, 129); i < 129; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                    }

                    // 브론즈 줄 게 없다면 실버 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))  
                    {
                        if (PoolManager.Instance.silverAd < 41)
                        {
                            if (PoolManager.Instance.silverAd > 38)
                            {
                                for (int i = 201; i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(201, 242); i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    resultId = tmp;
                }
                else if(j == 1) // 실버 모험가를 보여주기로 했다면
                {
                    // 우선은 남은 실버 모험가 확인
                    int tmp = resultId;
                    if (PoolManager.Instance.silverAd < 41)
                    {
                        if (PoolManager.Instance.silverAd > 38)
                        {
                            for (int i = 201; i < 242; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = UnityEngine.Random.Range(201, 242); i < 242; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                    }
                    

                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))  // 실버가 없다면 브론즈 주기
                    {
                        if (PoolManager.Instance.bronzAd < 28)
                        {
                            if (PoolManager.Instance.bronzAd > 20)
                            {
                                for (int i = 101; i < 129; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(101, 129); i < 129; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 브론즈도 없다면 골드 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))  
                    {
                        if (PoolManager.Instance.goldAd < 44)
                        {
                            if (PoolManager.Instance.goldAd > 40)
                            {
                                for (int i = 300; i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(300, 344); i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    resultId = tmp;
                }
                else if (j == 2) // 골드 모험가를 보여주기로 했다면
                {
                    // 남은 골드 확인
                    int tmp = resultId;
                    if (PoolManager.Instance.goldAd < 44)
                    {
                        if (PoolManager.Instance.goldAd > 40)
                        {
                            for (int i = 300; i < 344; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = UnityEngine.Random.Range(300, 344); i < 344; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                    }

                    // 골드 줄 게 없다면 실버 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.silverAd < 41)
                        {
                            if (PoolManager.Instance.silverAd > 38)
                            {
                                for (int i = 201; i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(201, 242); i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 실버 줄게 없다면 브론즈 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))  
                    {
                        if (PoolManager.Instance.bronzAd < 28)
                        {
                            if (PoolManager.Instance.bronzAd > 20)
                            {
                                for (int i = 101; i < 129; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(101, 129); i < 129; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    resultId = tmp;
                }

            }
            else if (GameInfo.gameInfo.Level == 2)  // 레벨 2
            {
                if (j == 0) // 브론즈 모험가를 주기로 했다면
                {
                    // 아직 보여주지 않은 브론즈 주기
                    int tmp = resultId;
                    if (PoolManager.Instance.bronzAd < 28)
                    {
                        if (PoolManager.Instance.bronzAd > 20)
                        {
                            for (int i = 101; i < 129; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = UnityEngine.Random.Range(101, 129); i < 129; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                    }

                    // 브론즈 줄 게 없다면 실버 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.silverAd < 41)
                        {
                            if (PoolManager.Instance.silverAd > 38)
                            {
                                for (int i = 201; i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(201, 242); i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    resultId = tmp;
                }
                else if (j == 1) // 실버 모험가를 주기로 했다면
                {
                    // 우선은 남은 실버 모험가 확인
                    int tmp = resultId;
                    if (PoolManager.Instance.silverAd < 41)
                    {
                        if (PoolManager.Instance.silverAd > 38)
                        {
                            for (int i = 201; i < 242; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = UnityEngine.Random.Range(201, 242); i < 242; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                    }

                    // 실버가 없다면 브론즈 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.bronzAd < 28)
                        {
                            if (PoolManager.Instance.bronzAd > 20)
                            {
                                for (int i = 101; i < 129; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(101, 129); i < 129; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 브론즈도 없다면 골드 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))  
                    {
                        if (PoolManager.Instance.goldAd < 44)
                        {
                            if (PoolManager.Instance.goldAd > 40)
                            {
                                for (int i = 300; i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(300, 344); i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    resultId = tmp;
                }
                else if (j == 2) // 골드 모험가를 주기로 했다면
                {
                    // 남은 골드 모험가 주기
                    int tmp = resultId;
                    if (PoolManager.Instance.goldAd < 44)
                    {
                        if (PoolManager.Instance.goldAd > 40)
                        {
                            for (int i = 300; i < 344; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = UnityEngine.Random.Range(300, 344); i < 344; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                    }

                    // 골드 없다면 실버 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp)) 
                    {
                        if (PoolManager.Instance.silverAd < 41)
                        {
                            if (PoolManager.Instance.silverAd > 38)
                            {
                                for (int i = 201; i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(201, 242); i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 실버도 없다면 브론즈 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.bronzAd < 28)
                        {
                            if (PoolManager.Instance.bronzAd > 20)
                            {
                                for (int i = 101; i < 129; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(101, 129); i < 129; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    resultId = tmp;
                }
                else if (j == 3) // 플래티넘 모험가를 주기로 했다면
                {
                    // 남은 플래티넘 주기
                    int tmp = resultId;
                    if (PoolManager.Instance.platinumAd < 24)
                    {
                        if (PoolManager.Instance.platinumAd > 20)
                        {
                            for (int i = 400; i < 424; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = UnityEngine.Random.Range(400, 424); i < 424; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                    }

                    // 플래티넘 없다면 골드 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.goldAd < 44)
                        {
                            if (PoolManager.Instance.goldAd > 40)
                            {
                                for (int i = 300; i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(300, 344); i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 골드 없다면 실버 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.silverAd < 41)
                        {
                            if (PoolManager.Instance.silverAd > 38)
                            {
                                for (int i = 201; i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(201, 242); i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 실버도 없다면 브론즈 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.bronzAd < 28)
                        {
                            if (PoolManager.Instance.bronzAd > 20)
                            {
                                for (int i = 101; i < 129; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(101, 129); i < 129; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    resultId = tmp;
                }
            }
            else if (GameInfo.gameInfo.Level == 3)  // 레벨 3
            {
                if (j == 0) // 브론즈 모험가를 주기로 했다면
                {
                    // 아직 보여주지 않은 브론즈 주기
                    int tmp = resultId;
                    if (PoolManager.Instance.bronzAd < 28)
                    {
                        if (PoolManager.Instance.bronzAd > 20)
                        {
                            for (int i = 101; i < 129; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = UnityEngine.Random.Range(101, 129); i < 129; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                    }

                    // 브론즈 줄 게 없다면 실버 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))  
                    {
                        if (PoolManager.Instance.silverAd < 41)
                        {
                            if (PoolManager.Instance.silverAd > 38)
                            {
                                for (int i = 201; i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(201, 242); i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    resultId = tmp;
                }
                else if (j == 1) // 실버 모험가 보여주기로 했다면
                {
                    // 우선은 남은 실버 모험가 확인
                    int tmp = resultId;
                    if (PoolManager.Instance.silverAd < 41)
                    {
                        if (PoolManager.Instance.silverAd > 38)
                        {
                            for (int i = 201; i < 242; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = UnityEngine.Random.Range(201, 242); i < 242; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                    }

                    // 실버가 없다면 브론즈 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.bronzAd < 28)
                        {
                            if (PoolManager.Instance.bronzAd > 20)
                            {
                                for (int i = 101; i < 129; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(101, 129); i < 129; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 브론즈도 없다면 골드 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))  
                    {
                        if (PoolManager.Instance.goldAd < 44)
                        {
                            if (PoolManager.Instance.goldAd > 40)
                            {
                                for (int i = 300; i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(300, 344); i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    resultId = tmp;
                }
                else if (j == 2) // 골드 모험가 보여주기로 했다면
                {
                    // 남은 골드 모험가 주기
                    int tmp = resultId;
                    if (PoolManager.Instance.goldAd < 44)
                    {
                        if (PoolManager.Instance.goldAd > 40)
                        {
                            for (int i = 300; i < 344; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = UnityEngine.Random.Range(300, 344); i < 344; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                    }

                    // 골드 없다면 실버 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.silverAd < 41)
                        {
                            if (PoolManager.Instance.silverAd > 38)
                            {
                                for (int i = 201; i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(201, 242); i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 실버도 없다면 브론즈 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.bronzAd < 28)
                        {
                            if (PoolManager.Instance.bronzAd > 20)
                            {
                                for (int i = 101; i < 129; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(101, 129); i < 129; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    resultId = tmp;
                }
                else if (j == 3) // 플래티넘 모험가를 보여주기로 했다면
                {
                    // 남은 플래티넘 확인
                    int tmp = resultId;
                    if (PoolManager.Instance.platinumAd < 24)
                    {
                        if (PoolManager.Instance.platinumAd > 20)
                        {
                            for (int i = 400; i < 424; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = UnityEngine.Random.Range(400, 424); i < 424; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                    }

                    // 플래티넘 없다면 골드 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.goldAd < 44)
                        {
                            if (PoolManager.Instance.goldAd > 40)
                            {
                                for (int i = 300; i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(300, 344); i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 골드 없다면 실버 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.silverAd < 41)
                        {
                            if (PoolManager.Instance.silverAd > 38)
                            {
                                for (int i = 201; i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(201, 242); i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 실버도 없다면 브론즈 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.bronzAd < 28)
                        {
                            if (PoolManager.Instance.bronzAd > 20)
                            {
                                for (int i = 101; i < 129; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(101, 129); i < 129; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    resultId = tmp;
                }
                else if(j == 4) // 다이아 모험가를 보여주기로 했다면
                {
                    // 남은 다이아 확인
                    int tmp = resultId;
                    if (PoolManager.Instance.diaAd < 6)
                    {
                        for (int i = UnityEngine.Random.Range(500, 507); i < 507; i++)
                        {
                            if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                            {
                                PoolManager.Instance.userAdventureIndex.Add(i);
                                tmp = i;
                                break;
                            }
                        }
                    }

                    // 다이아없다면 플래티넘 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.platinumAd < 24)
                        {
                            if (PoolManager.Instance.platinumAd > 20)
                            {
                                for (int i = 400; i < 424; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(400, 424); i < 424; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 플래티넘 없다면 골드 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.goldAd < 44)
                        {
                            if (PoolManager.Instance.goldAd > 40)
                            {
                                for (int i = 300; i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(300, 344); i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 골드 없다면 실버 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.silverAd < 41)
                        {
                            if (PoolManager.Instance.silverAd > 38)
                            {
                                for (int i = 201; i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(201, 242); i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    resultId = tmp;
                }
            }
            else if (GameInfo.gameInfo.Level == 4)  // 레벨 4
            {
                if (j == 1) // 실버 모험가를 보여주기로 했다면
                {
                    // 우선은 남은 실버 모험가 확인
                    int tmp = resultId;
                    if (PoolManager.Instance.silverAd < 41)
                    {
                        if (PoolManager.Instance.silverAd > 38)
                        {
                            for (int i = 201; i < 242; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = UnityEngine.Random.Range(201, 242); i < 242; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                    }

                    // 실버도 없다면 골드 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))  
                    {
                        if (PoolManager.Instance.goldAd < 44)
                        {
                            if (PoolManager.Instance.goldAd > 40)
                            {
                                for (int i = 300; i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(300, 344); i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 골드도 없다면 플래 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))  
                    {
                        if (PoolManager.Instance.platinumAd < 24)
                        {
                            if (PoolManager.Instance.platinumAd > 20)
                            {
                                for (int i = 400; i < 424; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(400, 424); i < 424; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    resultId = tmp;
                }
                else if (j == 2) // 골드 모험가를 보여주기로 했다면
                {
                    // 남은 골드 모험가 주기
                    int tmp = resultId;
                    if (PoolManager.Instance.goldAd < 44)
                    {
                        if (PoolManager.Instance.goldAd > 40)
                        {
                            for (int i = 300; i < 344; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = UnityEngine.Random.Range(300, 344); i < 344; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                    }

                    // 골드 없다면 실버 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.silverAd < 41)
                        {
                            if (PoolManager.Instance.silverAd > 38)
                            {
                                for (int i = 201; i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(201, 242); i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 실버도 없다면 플래 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.platinumAd < 24)
                        {
                            if (PoolManager.Instance.platinumAd > 20)
                            {
                                for (int i = 400; i < 424; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(400, 424); i < 424; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    resultId = tmp;
                }
                else if (j == 3) // 플래티넘 모험가를 보여주기로 했다면
                {
                    // 남은 플래티넘 확인
                    int tmp = resultId;
                    if (PoolManager.Instance.platinumAd < 24)
                    {
                        if (PoolManager.Instance.platinumAd > 20)
                        {
                            for (int i = 400; i < 424; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = UnityEngine.Random.Range(400, 424); i < 424; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                    }

                    // 플래티넘 없다면 골드 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.goldAd < 44)
                        {
                            if (PoolManager.Instance.goldAd > 40)
                            {
                                for (int i = 300; i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(300, 344); i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 골드 없다면 실버 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.silverAd < 41)
                        {
                            if (PoolManager.Instance.silverAd > 38)
                            {
                                for (int i = 201; i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(201, 242); i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    resultId = tmp;
                }
                else if (j == 4) // 다이아 부족
                {
                    // 남은 다이아 확인
                    int tmp = resultId;
                    if (PoolManager.Instance.diaAd < 6)
                    {
                        for (int i = UnityEngine.Random.Range(500, 507); i < 507; i++)
                        {
                            if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                            {
                                PoolManager.Instance.userAdventureIndex.Add(i);
                                tmp = i;
                                break;
                            }
                        }
                    }

                    // 다이아 없다면 플래 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.platinumAd < 24)
                        {
                            if (PoolManager.Instance.platinumAd > 20)
                            {
                                for (int i = 400; i < 424; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(400, 424); i < 424; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 플래티넘 없다면 골드 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.goldAd < 44)
                        {
                            if (PoolManager.Instance.goldAd > 40)
                            {
                                for (int i = 300; i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(300, 344); i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 골드 없다면 실버 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.silverAd < 41)
                        {
                            if (PoolManager.Instance.silverAd > 38)
                            {
                                for (int i = 201; i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(201, 242); i < 242; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    resultId = tmp;
                }
            }
            else if (GameInfo.gameInfo.Level == 5)  // 레벨 5
            {
                if (j == 2) // 골드 모험가를 보여주기로 했다면
                {
                    // 남은 골드 모험가 주기
                    int tmp = resultId;
                    if (PoolManager.Instance.goldAd < 44)
                    {
                        if (PoolManager.Instance.goldAd > 40)
                        {
                            for (int i = 300; i < 344; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = UnityEngine.Random.Range(300, 344); i < 344; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                    }

                    // 골드도 없다면 플래 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.platinumAd < 24)
                        {
                            if (PoolManager.Instance.platinumAd > 20)
                            {
                                for (int i = 400; i < 424; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(400, 424); i < 424; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    resultId = tmp;
                }
                else if (j == 3) // 플래티넘 모험가를 보여주기로 했다면
                {
                    // 남은 플래티넘 확인
                    int tmp = resultId;
                    if (PoolManager.Instance.platinumAd < 24)
                    {
                        if (PoolManager.Instance.platinumAd > 20)
                        {
                            for (int i = 400; i < 424; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = UnityEngine.Random.Range(400, 424); i < 424; i++)
                            {
                                if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                {
                                    PoolManager.Instance.userAdventureIndex.Add(i);
                                    tmp = i;
                                    break;
                                }
                            }
                        }
                    }

                    // 플래티넘 없다면 골드 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.goldAd < 44)
                        {
                            if (PoolManager.Instance.goldAd > 40)
                            {
                                for (int i = 300; i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(300, 344); i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    resultId = tmp;
                }
                else if (j == 4) // 다이아 부족
                {
                    // 남은 다이아 확인
                    int tmp = resultId;
                    if (PoolManager.Instance.diaAd < 6)
                    {
                        for (int i = UnityEngine.Random.Range(500, 507); i < 507; i++)
                        {
                            if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                            {
                                PoolManager.Instance.userAdventureIndex.Add(i);
                                tmp = i;
                                break;
                            }
                        }
                    }

                    // 다이아 없다면 플래 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.platinumAd < 24)
                        {
                            if (PoolManager.Instance.platinumAd > 20)
                            {
                                for (int i = 400; i < 424; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(400, 424); i < 424; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // 플래티넘 없다면 골드 주기
                    if (PoolManager.Instance.userAdventureIndex.Contains(tmp) || CheckHaveRandomIndex(tmp))
                    {
                        if (PoolManager.Instance.goldAd < 44)
                        {
                            if (PoolManager.Instance.goldAd > 40)
                            {
                                for (int i = 300; i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = UnityEngine.Random.Range(300, 344); i < 344; i++)
                                {
                                    if (!PoolManager.Instance.userAdventureIndex.Contains(i) && !CheckHaveRandomIndex(i))
                                    {
                                        PoolManager.Instance.userAdventureIndex.Add(i);
                                        tmp = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

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

    private bool CheckHaveRandomIndex(int mid)
    {
        int tmp = DataTableManager.Instance.GetRandomAdventureData(mid).adventureId;


        string adventureIdOfString = PlayerPrefs.GetString("AdventureId");  // 현재 모험가 ID 가져오기
        string[] adventureIdOfInt = adventureIdOfString.Split(',');         // 구분자 모험가 ID 분리

        if (adventureIdOfString == "") return false;

        for (int index = 0; index < adventureIdOfInt.Length; index++)        // 모든 모험가 ID 순회
        {
            if (tmp == Convert.ToInt32(adventureIdOfInt[index]))     // 매개변수와 같은 모험가 ID 검색
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
                for (int i = 400; i < 424; i++)
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
