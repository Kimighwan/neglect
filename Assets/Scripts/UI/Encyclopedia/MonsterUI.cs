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
        }
        else
        {
            goblin.GetComponent<Button>().interactable = false;
            goblin.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
        }

        if (PlayerPrefs.GetInt("슬라임") == 1)
        {
            slime.GetComponent<Button>().interactable = true;
            slime.GetComponentInChildren<TextMeshProUGUI>().text = "슬라임";
        }
        else
        {
            slime.GetComponent<Button>().interactable = false;
            slime.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
        }

        if (PlayerPrefs.GetInt("픽시") == 1)
        {
            pixie.GetComponent<Button>().interactable = true;
            pixie.GetComponentInChildren<TextMeshProUGUI>().text = "픽시";
        }
        else
        {
            pixie.GetComponent<Button>().interactable = false;
            pixie.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
        }

        if (PlayerPrefs.GetInt("정령") == 1)
        {
            soul.GetComponent<Button>().interactable = true;
            soul.GetComponentInChildren<TextMeshProUGUI>().text = "정령";
        }
        else
        {
            soul.GetComponent<Button>().interactable = false;
            soul.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
        }

        if (PlayerPrefs.GetInt("오크") == 1)
        {
            oak.GetComponent<Button>().interactable = true;
            oak.GetComponentInChildren<TextMeshProUGUI>().text = "오크";
        }
        else
        {
            oak.GetComponent<Button>().interactable = false;
            oak.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
        }

        if (PlayerPrefs.GetInt("언데드") == 1)
        {
            undead.GetComponent<Button>().interactable = true;
            undead.GetComponentInChildren<TextMeshProUGUI>().text = "언데드";
        }
        else
        {
            undead.GetComponent<Button>().interactable = false;
            undead.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
        }

        if (PlayerPrefs.GetInt("골렘") == 1)
        {
            golem.GetComponent<Button>().interactable = true;
            golem.GetComponentInChildren<TextMeshProUGUI>().text = "골렘";
        }
        else
        {
            golem.GetComponent<Button>().interactable = false;
            golem.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
        }

        if (PlayerPrefs.GetInt("큐피트") == 1)
        {
            cupid.GetComponent<Button>().interactable = true;
            cupid.GetComponentInChildren<TextMeshProUGUI>().text = "큐피트";
        }
        else
        {
            cupid.GetComponent<Button>().interactable = false;
            cupid.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
        }

        if (PlayerPrefs.GetInt("가고일") == 1)
        {
            gargoyle.GetComponent<Button>().interactable = true;
            gargoyle.GetComponentInChildren<TextMeshProUGUI>().text = "가고일";
        }
        else
        {
            gargoyle.GetComponent<Button>().interactable = false;
            gargoyle.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
        }

        if (PlayerPrefs.GetInt("오우거") == 1)
        {
            ogre.GetComponent<Button>().interactable = true;
            ogre.GetComponentInChildren<TextMeshProUGUI>().text = "오우거";
        }
        else
        {
            ogre.GetComponent<Button>().interactable = false;
            ogre.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
        }

        if (PlayerPrefs.GetInt("악마") == 1)
        {
            devil.GetComponent<Button>().interactable = true;
            devil.GetComponentInChildren<TextMeshProUGUI>().text = "악마";
        }
        else
        {
            devil.GetComponent<Button>().interactable = false;
            devil.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
        }

        if (PlayerPrefs.GetInt("천사") == 1)
        {
            angel.GetComponent<Button>().interactable = true;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "천사";
        }
        else
        {
            angel.GetComponent<Button>().interactable = false;
            angel.GetComponentInChildren<TextMeshProUGUI>().text = "?????";
        }
    }
    public override void OnClickCloseButton()
    {
        AudioManager.Instance.PlaySFX(SFX.CloseBook);
        base.OnClickCloseButton();
    }
}
