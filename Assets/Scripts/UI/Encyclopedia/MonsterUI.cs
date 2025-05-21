using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : BaseUI
{
    private enum monsterOrder
    {
        고블린 = 110011,
        슬라임 = 110012,
        픽시 = 110021,
        정령 = 110022,
        오크 = 110031,
        언데드 = 110032,
        골렘 = 110033,
        큐피트 = 110041,
        가고일 = 110042,
        오우거 = 110051,
        악마 = 110052,
        천사 = 110053,
        bopalrabbit = 110050,
        durahan = 110043,
        goblinJusul = 110044,
        goblinKing = 110054,
        goblinWarrior = 110034,
        honrabbit = 110013,
        spector = 110024,
        hopGoblin = 110023,
    };

    [Header("Obj")]
    public GameObject goblin;
    public GameObject slime;
    public GameObject pixie;
    public GameObject race;
    public GameObject oak;
    public GameObject undead;
    public GameObject golem;
    public GameObject cupid;
    public GameObject gargoyle;
    public GameObject ogre;
    public GameObject devil;
    public GameObject angel;
    public GameObject bopalrabbit;
    public GameObject durahan;
    public GameObject goblinJusul;
    public GameObject goblinKing;
    public GameObject goblinWarrior;
    public GameObject honrabbit;
    public GameObject hopGoblin;
    public GameObject spector;


    [Header("Red Dot")]
    [SerializeField]
    GameObject goblinRedDot;
    [SerializeField]
    GameObject slimeRedDot;
    [SerializeField]
    GameObject pixieRedDot;
    [SerializeField]
    GameObject raceRedDot;
    [SerializeField]
    GameObject oakRedDot;
    [SerializeField]
    GameObject undeadRedDot;
    [SerializeField]
    GameObject golemRedDot;
    [SerializeField]
    GameObject cupidRedDot;
    [SerializeField]
    GameObject gargoyleRedDot;
    [SerializeField]
    GameObject ogreRedDot;
    [SerializeField]
    GameObject devilRedDot;
    [SerializeField]
    GameObject angelRedDot;
    [SerializeField]
    GameObject bopalrabbitRedDot;
    [SerializeField]
    GameObject durahanRedDot;
    [SerializeField]
    GameObject goblinJusulRedDot;
    [SerializeField]
    GameObject goblinKingRedDot;
    [SerializeField]
    GameObject goblinWarriorRedDot;
    [SerializeField]
    GameObject honrabbitRedDot;
    [SerializeField]
    GameObject hopGoblinRedDot;
    [SerializeField]
    GameObject spectorRedDot;


    [Header("Image")]
    [SerializeField]
    private Sprite goblinImg;
    [SerializeField]
    private Sprite slimeImg;
    [SerializeField]
    private Sprite pixieImg;
    [SerializeField]
    private Sprite raceImg;
    [SerializeField]
    private Sprite oakImg;
    [SerializeField]
    private Sprite golemImg;
    [SerializeField]
    private Sprite cupidImg;
    [SerializeField]
    private Sprite gargoyleImg;
    [SerializeField]
    private Sprite ogreImg;
    [SerializeField]
    private Sprite devilImg;
    [SerializeField]
    private Sprite angelImg;
    [SerializeField]
    private Sprite undeadImg;
    [SerializeField]
    private Sprite bopalrabbitImg;
    [SerializeField]
    private Sprite durahanImg;
    [SerializeField]
    private Sprite goblinJusulImg;
    [SerializeField]
    private Sprite goblinKingImg;
    [SerializeField]
    private Sprite goblinWarriorImg;
    [SerializeField]
    private Sprite honrabbitImg;
    [SerializeField]
    private Sprite hopGoblinImg;
    [SerializeField]
    private Sprite spectorImg;
    [SerializeField]
    private Sprite unpressedImg;

    private const string PATH = "Arts/MainScene/UI";

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
    }

    private void OnEnable()
    {
        BtnActive();
    }

    public void OnClickAllUICloseBtn()
    {
        AudioManager.Instance.PlaySFX(SFX.CloseBook);
        UIManager.Instance.CloseAllOpenUI();
    }

    public void OnClickSystemBtn()
    {
        var systemUI = new BaseUIData();
        AudioManager.Instance.PlaySFX(SFX.OpenBook);
        CloseUI();
        UIManager.Instance.OpenUI<SystemUI>(systemUI);
    }

    public void OnClickMonsterDescBtn(int id)
    {
        DataTableManager.Instance.monsterDescId = id;
        var monsterDescUI = new BaseUIData();
        AudioManager.Instance.PlaySFX(SFX.BookFlip3);
        //UIManager.Instance.CloseUI(this);
        UIManager.Instance.OpenUI<MonsterDescUI>(monsterDescUI);
        PlayerPrefs.SetInt($"{((monsterOrder)id)}", 1);
    }

    public void BackBtn()
    {
        AudioManager.Instance.PlaySFX(SFX.CloseBook);
        UIManager.Instance.CloseUI(this);

        var encyclopediaUI = new BaseUIData();
        UIManager.Instance.OpenUI<EncyclopediaUI>(encyclopediaUI);
    }

    private void BtnActive()
    {
        // 안 깼으면 버튼 비활성화
        // 안 깼으면 Text ???? 표시

        if (PlayerPrefs.GetInt("고블린") != 0)
        {
            goblin.GetComponent<Button>().interactable = true;
            goblin.GetComponentInChildren<TextMeshProUGUI>().text = "고블린";
            goblin.GetComponent<Image>().sprite = goblinImg;

            if(PlayerPrefs.GetInt("고블린") == -1) goblinRedDot.SetActive(true);
            else goblinRedDot.SetActive(false);
        }
        else
        {
            goblin.GetComponent<Button>().interactable = false;
            goblin.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            goblin.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("슬라임") != 0)
        {
            slime.GetComponent<Button>().interactable = true;
            slime.GetComponentInChildren<TextMeshProUGUI>().text = "슬라임";
            slime.GetComponent<Image>().sprite = slimeImg;

            if (PlayerPrefs.GetInt("슬라임") == -1) slimeRedDot.SetActive(true);
            else slimeRedDot.SetActive(false);
        }
        else
        {
            slime.GetComponent<Button>().interactable = false;
            slime.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            slime.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("픽시") != 0)
        {
            pixie.GetComponent<Button>().interactable = true;
            pixie.GetComponentInChildren<TextMeshProUGUI>().text = "픽시";
            pixie.GetComponent<Image>().sprite = pixieImg;

            if (PlayerPrefs.GetInt("픽시") == -1) pixieRedDot.SetActive(true);
            else pixieRedDot.SetActive(false);
        }
        else
        {
            pixie.GetComponent<Button>().interactable = false;
            pixie.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            pixie.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("정령") != 0)
        {
            race.GetComponent<Button>().interactable = true;
            race.GetComponentInChildren<TextMeshProUGUI>().text = "정령";
            race.GetComponent<Image>().sprite = raceImg;

            if (PlayerPrefs.GetInt("정령") == -1) raceRedDot.SetActive(true);
            else raceRedDot.SetActive(false);
        }
        else
        {
            race.GetComponent<Button>().interactable = raceImg;
            race.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            race.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("오크") != 0)
        {
            oak.GetComponent<Button>().interactable = true;
            oak.GetComponentInChildren<TextMeshProUGUI>().text = "오크";
            oak.GetComponent<Image>().sprite = oakImg;

            if (PlayerPrefs.GetInt("오크") == -1) oakRedDot.SetActive(true);
            else oakRedDot.SetActive(false);
        }
        else
        {
            oak.GetComponent<Button>().interactable = false;
            oak.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            oak.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("언데드") != 0)
        {
            undead.GetComponent<Button>().interactable = true;
            undead.GetComponentInChildren<TextMeshProUGUI>().text = "언데드";
            undead.GetComponent<Image>().sprite = undeadImg;

            if (PlayerPrefs.GetInt("언데드") == -1) undeadRedDot.SetActive(true);
            else undeadRedDot.SetActive(false);
        }
        else
        {
            undead.GetComponent<Button>().interactable = false;
            undead.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            undead.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("골렘") != 0)
        {
            golem.GetComponent<Button>().interactable = true;
            golem.GetComponentInChildren<TextMeshProUGUI>().text = "골렘";
            golem.GetComponent<Image>().sprite = golemImg;

            if (PlayerPrefs.GetInt("골렘") == -1) golemRedDot.SetActive(true);
            else golemRedDot.SetActive(false);
        }
        else
        {
            golem.GetComponent<Button>().interactable = false;
            golem.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            golem.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("큐피트") != 0)
        {
            cupid.GetComponent<Button>().interactable = true;
            cupid.GetComponentInChildren<TextMeshProUGUI>().text = "큐피트";
            cupid.GetComponent<Image>().sprite = cupidImg;

            if (PlayerPrefs.GetInt("큐피트") == -1) cupidRedDot.SetActive(true);
            else cupidRedDot.SetActive(false);
        }
        else
        {
            cupid.GetComponent<Button>().interactable = false;
            cupid.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            cupid.GetComponent<Image>().sprite = unpressedImg;
        }


        if (PlayerPrefs.GetInt("가고일") != 0)
        {
            gargoyle.GetComponent<Button>().interactable = true;
            gargoyle.GetComponentInChildren<TextMeshProUGUI>().text = "가고일";
            gargoyle.GetComponent<Image>().sprite = gargoyleImg;

            if (PlayerPrefs.GetInt("가고일") == -1) gargoyleRedDot.SetActive(true);
            else gargoyleRedDot.SetActive(false);
        }
        else
        {
            gargoyle.GetComponent<Button>().interactable = false;
            gargoyle.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            gargoyle.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("오우거") != 0)
        {
            ogre.GetComponent<Button>().interactable = true;
            ogre.GetComponentInChildren<TextMeshProUGUI>().text = "오우거";
            ogre.GetComponent<Image>().sprite = ogreImg;

            if (PlayerPrefs.GetInt("오우거") == -1) ogreRedDot.SetActive(true);
            else ogreRedDot.SetActive(false);
        }
        else
        {
            ogre.GetComponent<Button>().interactable = false;
            ogre.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            ogre.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("악마") != 0)
        {
            devil.GetComponent<Button>().interactable = true;
            devil.GetComponentInChildren<TextMeshProUGUI>().text = "악마";
            devil.GetComponent<Image>().sprite = devilImg;

            if (PlayerPrefs.GetInt("악마") == -1) devilRedDot.SetActive(true);
            else devilRedDot.SetActive(false);
        }
        else
        {
            devil.GetComponent<Button>().interactable = false;
            devil.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            devil.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("천사") != 0)
        {
            angel.GetComponent<Button>().interactable = true;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "천사";
            angel.GetComponent<Image>().sprite = angelImg;

            if (PlayerPrefs.GetInt("천사") == -1) angelRedDot.SetActive(true);
            else angelRedDot.SetActive(false);
        }
        else
        {
            angel.GetComponent<Button>().interactable = false;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            angel.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("bopalrabbit") != 0)
        {
            angel.GetComponent<Button>().interactable = true;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "bopalrabbit";
            angel.GetComponent<Image>().sprite = angelImg;

            if (PlayerPrefs.GetInt("bopalrabbit") == -1) angelRedDot.SetActive(true);
            else angelRedDot.SetActive(false);
        }
        else
        {
            angel.GetComponent<Button>().interactable = false;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            angel.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("durahan") != 0)
        {
            angel.GetComponent<Button>().interactable = true;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "durahan";
            angel.GetComponent<Image>().sprite = angelImg;

            if (PlayerPrefs.GetInt("durahan") == -1) angelRedDot.SetActive(true);
            else angelRedDot.SetActive(false);
        }
        else
        {
            angel.GetComponent<Button>().interactable = false;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            angel.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("durahan") != 0)
        {
            angel.GetComponent<Button>().interactable = true;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "durahan";
            angel.GetComponent<Image>().sprite = angelImg;

            if (PlayerPrefs.GetInt("durahan") == -1) angelRedDot.SetActive(true);
            else angelRedDot.SetActive(false);
        }
        else
        {
            angel.GetComponent<Button>().interactable = false;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            angel.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("goblinKing") != 0)
        {
            angel.GetComponent<Button>().interactable = true;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "goblinKing";
            angel.GetComponent<Image>().sprite = angelImg;

            if (PlayerPrefs.GetInt("goblinKing") == -1) angelRedDot.SetActive(true);
            else angelRedDot.SetActive(false);
        }
        else
        {
            angel.GetComponent<Button>().interactable = false;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            angel.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("goblinWarrior") != 0)
        {
            angel.GetComponent<Button>().interactable = true;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "goblinWarrior";
            angel.GetComponent<Image>().sprite = angelImg;

            if (PlayerPrefs.GetInt("goblinWarrior") == -1) angelRedDot.SetActive(true);
            else angelRedDot.SetActive(false);
        }
        else
        {
            angel.GetComponent<Button>().interactable = false;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            angel.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("honrabbit") != 0)
        {
            angel.GetComponent<Button>().interactable = true;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "honrabbit";
            angel.GetComponent<Image>().sprite = angelImg;

            if (PlayerPrefs.GetInt("honrabbit") == -1) angelRedDot.SetActive(true);
            else angelRedDot.SetActive(false);
        }
        else
        {
            angel.GetComponent<Button>().interactable = false;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            angel.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("spector") != 0)
        {
            angel.GetComponent<Button>().interactable = true;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "spector";
            angel.GetComponent<Image>().sprite = angelImg;

            if (PlayerPrefs.GetInt("spector") == -1) angelRedDot.SetActive(true);
            else angelRedDot.SetActive(false);
        }
        else
        {
            angel.GetComponent<Button>().interactable = false;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            angel.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("hopGoblin") != 0)
        {
            angel.GetComponent<Button>().interactable = true;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "hopGoblin";
            angel.GetComponent<Image>().sprite = angelImg;

            if (PlayerPrefs.GetInt("hopGoblin") == -1) angelRedDot.SetActive(true);
            else angelRedDot.SetActive(false);
        }
        else
        {
            angel.GetComponent<Button>().interactable = false;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "?";
            angel.GetComponent<Image>().sprite = unpressedImg;
        }
    }
    public override void OnClickCloseButton()
    {
        AudioManager.Instance.PlaySFX(SFX.CloseBook);
        base.OnClickCloseButton();
    }
}
