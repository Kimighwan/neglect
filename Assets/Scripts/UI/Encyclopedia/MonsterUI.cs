using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterUI : BaseUI
{
    public GameObject goblin;
    public GameObject slime;
    public GameObject pixie;
    public GameObject soul;
    public GameObject oak;
    public GameObject undead;
    public GameObject golem;
    public GameObject cupid;
    public GameObject gargoyle;
    public GameObject ogre;
    public GameObject devil;
    public GameObject angel;

    [SerializeField]
    private Sprite pressedImg;
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
        UIManager.Instance.CloseUI(this);
        UIManager.Instance.OpenUI<MonsterDescUI>(monsterDescUI);
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

        if (PlayerPrefs.GetInt("고블린") == 1)
        {
            goblin.GetComponent<Button>().interactable = true;
            goblin.GetComponentInChildren<TextMeshProUGUI>().text = "고블린";
            goblin.GetComponent<Image>().sprite = pressedImg;
        }
        else
        {
            goblin.GetComponent<Button>().interactable = false;
            goblin.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
            goblin.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("슬라임") == 1)
        {
            slime.GetComponent<Button>().interactable = true;
            slime.GetComponentInChildren<TextMeshProUGUI>().text = "슬라임";
            slime.GetComponent<Image>().sprite = pressedImg;
        }
        else
        {
            slime.GetComponent<Button>().interactable = false;
            slime.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
            slime.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("픽시") == 1)
        {
            pixie.GetComponent<Button>().interactable = true;
            pixie.GetComponentInChildren<TextMeshProUGUI>().text = "픽시";
            pixie.GetComponent<Image>().sprite = pressedImg;
        }
        else
        {
            pixie.GetComponent<Button>().interactable = false;
            pixie.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
            pixie.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("정령") == 1)
        {
            soul.GetComponent<Button>().interactable = true;
            soul.GetComponentInChildren<TextMeshProUGUI>().text = "정령";
            soul.GetComponent<Image>().sprite = pressedImg;
        }
        else
        {
            soul.GetComponent<Button>().interactable = false;
            soul.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
            soul.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("오크") == 1)
        {
            oak.GetComponent<Button>().interactable = true;
            oak.GetComponentInChildren<TextMeshProUGUI>().text = "오크";
            oak.GetComponent<Image>().sprite = pressedImg;
        }
        else
        {
            oak.GetComponent<Button>().interactable = false;
            oak.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
            oak.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("언데드") == 1)
        {
            undead.GetComponent<Button>().interactable = true;
            undead.GetComponentInChildren<TextMeshProUGUI>().text = "언데드";
            undead.GetComponent<Image>().sprite = pressedImg;
        }
        else
        {
            undead.GetComponent<Button>().interactable = false;
            undead.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
            undead.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("골렘") == 1)
        {
            golem.GetComponent<Button>().interactable = true;
            golem.GetComponentInChildren<TextMeshProUGUI>().text = "골렘";
            golem.GetComponent<Image>().sprite = pressedImg;
        }
        else
        {
            golem.GetComponent<Button>().interactable = false;
            golem.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
            golem.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("큐피트") == 1)
        {
            cupid.GetComponent<Button>().interactable = true;
            cupid.GetComponentInChildren<TextMeshProUGUI>().text = "큐피트";
            cupid.GetComponent<Image>().sprite = pressedImg;
        }
        else
        {
            cupid.GetComponent<Button>().interactable = false;
            cupid.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
            cupid.GetComponent<Image>().sprite = unpressedImg;
        }


        if (PlayerPrefs.GetInt("가고일") == 1)
        {
            gargoyle.GetComponent<Button>().interactable = true;
            gargoyle.GetComponentInChildren<TextMeshProUGUI>().text = "가고일";
            gargoyle.GetComponent<Image>().sprite = pressedImg;
        }
        else
        {
            gargoyle.GetComponent<Button>().interactable = false;
            gargoyle.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
            gargoyle.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("오우거") == 1)
        {
            ogre.GetComponent<Button>().interactable = true;
            ogre.GetComponentInChildren<TextMeshProUGUI>().text = "오우거";
            ogre.GetComponent<Image>().sprite = pressedImg;
        }
        else
        {
            ogre.GetComponent<Button>().interactable = false;
            ogre.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
            ogre.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("악마") == 1)
        {
            devil.GetComponent<Button>().interactable = true;
            devil.GetComponentInChildren<TextMeshProUGUI>().text = "악마";
            devil.GetComponent<Image>().sprite = pressedImg;
        }
        else
        {
            devil.GetComponent<Button>().interactable = false;
            devil.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
            devil.GetComponent<Image>().sprite = unpressedImg;
        }

        if (PlayerPrefs.GetInt("천사") == 1)
        {
            angel.GetComponent<Button>().interactable = true;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "천사";
            angel.GetComponent<Image>().sprite = pressedImg;
        }
        else
        {
            angel.GetComponent<Button>().interactable = false;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
            angel.GetComponent<Image>().sprite = unpressedImg;
        }
    }
    public override void OnClickCloseButton()
    {
        AudioManager.Instance.PlaySFX(SFX.CloseBook);
        base.OnClickCloseButton();
    }
}
