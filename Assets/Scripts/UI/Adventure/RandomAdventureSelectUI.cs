
using TMPro;
using UnityEngine;

public class RandomAdventureSelectUI : BaseUI
{
    private AdventureData adventureData;

    private string adventureName;
    private string adventurePosition;
    private string adventureClass;     // 계열
    private string adventureType;        // 타입
    private string adventureTier;        // 테두리 표현

    public TextMeshProUGUI name;
    public TextMeshProUGUI position;
    public TextMeshProUGUI m_class;
    public TextMeshProUGUI type;

    private Transform pos;

    public override void Init(Transform anchor)
    {
        base.Init(anchor);

        var rectTransform = GetComponent<RectTransform>();

        pos = GameObject.FindGameObjectWithTag("SelectGroup").transform;

        this.transform.SetParent(pos);
        rectTransform.sizeDelta = new Vector2(311.3f, 515.6f);
    }

    public void OnClickSelected()
    {
        // 영입
        // 골드 차감
    }

    private void GetAdventureData()
    {
        adventureData = DataTableManager.Instance.GetAdventureData(1);

        adventureName = adventureData.adventureName;
        adventurePosition = adventureData.adventurePosition;
        adventureClass = adventureData.adventureClass;
        adventureType = adventureData.adventureType;
        adventureTier = adventureData.adventureTier;
    }

    private void InitData()
    {
        name.text = adventureName;
        position.text = adventurePosition;
        type.text = adventureType;
        m_class.text = adventureClass;
    }
}
