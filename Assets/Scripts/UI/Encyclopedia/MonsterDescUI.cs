using TMPro;

public class MonsterDescUI : MonsterUI
{
    public MonsterData monsterData;

    private string monsterName;
    private string monsterTier;
    private string monsterWeekness;
    private string monsterStrength;
    private string monsterDesc;

    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtTier;
    public TextMeshProUGUI txtWeekness;
    public TextMeshProUGUI txtStrength;
    public TextMeshProUGUI txtDesc;

    private void Awake()
    {
        GetMonsterDescData();
        InitData();
    }

    private void OnEnable()
    {
        GetMonsterDescData();
        InitData();
    }

    private void GetMonsterDescData()
    {
        monsterData = DataTableManager.Instance.GetMonsterData(DataTableManager.Instance.monsterDescId);

        this.monsterName = monsterData.monsterName;
        this.monsterTier = monsterData.monsterTier;
        this.monsterWeekness = monsterData.monsterWeekness;
        this.monsterStrength = monsterData.monsterStrength;
        this.monsterDesc = monsterData.monsterDesc;
    }

    private void InitData()
    {
        txtName.text = monsterName;
        txtTier.text = monsterTier + " 등급";
        txtWeekness.text = "약점 : " + monsterWeekness;
        txtStrength.text = "강점 : " + monsterStrength;
        txtDesc.text = monsterDesc;
    }

    public void BackBtnOfMonsterDescUI()
    {
        UIManager.Instance.CloseUI(this);

        var monsterUI = new BaseUIData();
        UIManager.Instance.OpenUI<MonsterUI>(monsterUI);
    }
}
