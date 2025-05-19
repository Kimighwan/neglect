using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterDescUI : BaseUI
{
    public MonsterData monsterData;

    private int monsterId;
    private string monsterName;
    private string monsterWeekness;
    private string monsterStrength;
    private string monsterDesc;

    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtWeekness;
    public TextMeshProUGUI txtStrength;
    public TextMeshProUGUI txtDesc;
    public RawImage backGround;

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
        this.monsterWeekness = monsterData.monsterWeekness;
        this.monsterStrength = monsterData.monsterStrength;
        this.monsterDesc = monsterData.monsterDesc;
        this.monsterId = monsterData.monsterId;
    }

    private void InitData()
    {
        txtName.text = monsterName;
        txtWeekness.text = "약점 : " + monsterWeekness;
        txtStrength.text = "강점 : " + monsterStrength;
        txtDesc.text = monsterDesc;
       SetMonsterImage(monsterId);
    }

    //public void BackBtnOfMonsterDescUI()
    //{
    //    AudioManager.Instance.PlaySFX(SFX.BookFlip4);
    //    UIManager.Instance.CloseUI(this);

    //    var monsterUI = new BaseUIData();
    //    UIManager.Instance.OpenUI<MonsterUI>(monsterUI);
    //}

    private void SetMonsterImage(int id)
    {
        backGround.texture = Resources.Load($"Arts/Monsters/Paper/{id}") as Texture2D;
    }


    public override void OnClickCloseButton()
    {
        AudioManager.Instance.PlaySFX(SFX.CloseBook);
        base.OnClickCloseButton();
    }

    public void OnClickSystemBtn()
    {
        var systemUI = new BaseUIData();
        AudioManager.Instance.PlaySFX(SFX.OpenBook);
        CloseUI();
        UIManager.Instance.OpenUI<SystemUI>(systemUI);
    }
}
