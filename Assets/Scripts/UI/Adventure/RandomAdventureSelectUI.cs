using System;
using TMPro;
using UnityEngine;

public class RandomAdventureSelectUI : MonoBehaviour
{
    public TextMeshProUGUI c_name;
    public TextMeshProUGUI position;
    public TextMeshProUGUI m_class;
    public TextMeshProUGUI type;


    private AdventureData adventureData;

    private int adventureId;
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
        if (CheckHaveAdventureID(adventureId))  // 선택된 모험가가 이미 있음
            return;

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
        
        // 골드 차감

    }

    private void GetAdventureData()
    {
        adventureData = DataTableManager.Instance.GetAdventureData(RandomIndexMake());

        adventureId = adventureData.adventureId;
        this.adventureName = adventureData.adventureName;
        this.adventurePosition = adventureData.adventurePosition;
        this.adventureClass = adventureData.adventureClass;
        this.adventureType = adventureData.adventureType;
        this.adventureTier = adventureData.adventureTier;
    }

    private void InitData()
    {
        c_name.text = "이름 : " + adventureName;
        position.text = adventurePosition;
        type.text = adventureType;
        m_class.text = adventureClass;
    }

    private int RandomIndexMake()   // 무작위 숫자   // 좀 안 좋은 랜덤 숫자인 듯...
    {
        int randomId;

        randomId = UnityEngine.Random.Range(120001, 120091);
        
        return randomId;
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
                Debug.Log("해당 모험가가 이미 있습니다.");
                return true;    // 해당 모험가가 있음
            }
        }

        return false;           // 해당 모험가가 없음
    }
}
