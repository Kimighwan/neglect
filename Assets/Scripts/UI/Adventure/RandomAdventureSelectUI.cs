
using System;
using System.Data;
using TMPro;
using UnityEngine;

public class RandomAdventureSelectUI : MonoBehaviour
{
    private AdventureData adventureData;

    private string adventureName;
    private string adventurePosition;
    private string adventureClass;     // 계열
    private string adventureType;        // 타입
    private string adventureTier;        // 테두리 표현


    public TextMeshProUGUI c_name;
    public TextMeshProUGUI position;
    public TextMeshProUGUI m_class;
    public TextMeshProUGUI type;

    private Transform pos;

    private void Awake()
    {
        var rectTransform = GetComponent<RectTransform>();

        pos = GameObject.FindGameObjectWithTag("SelectGroup").transform;

        this.transform.SetParent(pos);
        rectTransform.sizeDelta = new Vector2(311.3f, 515.6f);

        GetAdventureData();
        InitData();
    }

    public void OnClickSelected()
    {
        // 영입
        // 골드 차감
    }

    private void GetAdventureData()
    {
        adventureData = DataTableManager.Instance.GetAdventureData(RandomIndexMake());

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

    private int RandomIndexMake()
    {
        int randomId;
        UnityEngine.Random.InitState((int)(DateTime.Now.Ticks));
        randomId = UnityEngine.Random.Range(1, 91);

        return randomId;
    }
}
