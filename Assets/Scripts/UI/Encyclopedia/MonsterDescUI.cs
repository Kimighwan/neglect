using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterDescUI : BaseUI
{
    public MonsterData monsterData;

    private int monsterId;
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
    public RawImage image;

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
        this.monsterId = monsterData.monsterId;
    }

    private void InitData()
    {
        txtName.text = monsterName;
        txtTier.text = monsterTier + " 등급";
        txtWeekness.text = "약점 : " + monsterWeekness;
        txtStrength.text = "강점 : " + monsterStrength;
        txtDesc.text = monsterDesc;
       SetMonsterImage(monsterId);
    }

    public void BackBtnOfMonsterDescUI()
    {
        AudioManager.Instance.PlaySFX(SFX.BookFlip4);
        UIManager.Instance.CloseUI(this);

        var monsterUI = new BaseUIData();
        UIManager.Instance.OpenUI<MonsterUI>(monsterUI);
    }

    private void SetMonsterImage(int id)
    {
        image.texture = Resources.Load($"Arts/Monster/{id}") as Texture2D;

        //string s = "";
        //switch (id)
        //{
        //    case 110011:
        //        s = "goblin";
        //        break;
        //    case 110012:
        //        s = "slime";
        //        break;
        //    case 110021:
        //        s = "fici";
        //        break;
        //    case 110022:
        //        s = "spirit";
        //        break;
        //    case 110031:
        //        s = "oak";
        //        break;
        //    case 110032:
        //        s = "undead";
        //        break;
        //    case 110033:
        //        s = "golem";
        //        break;
        //    case 110041:
        //        s = "cupid";
        //        break;
        //    case 110042:
        //        s = "gagoil";
        //        break;
        //    case 110043:
        //        s = "dyurahan";
        //        break;
        //    case 110051:
        //        s = "ouga";
        //        break;
        //    case 110052:
        //        s = "devil";
        //        break;
        //    case 110053:
        //        s = "angel";
        //        break;
        //}

        //if (s != "") image.sprite = Resources.Load<Sprite>($"Arts/Monsters/{s}");
        //else image.sprite = null;
    }


    public override void OnClickCloseButton()
    {
        AudioManager.Instance.PlaySFX(SFX.CloseBook);
        base.OnClickCloseButton();
    }
}
