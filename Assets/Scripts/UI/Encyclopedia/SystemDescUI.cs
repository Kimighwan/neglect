using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SystemDescUI : SystemUI
{
    public SystemDescData systemDescData;

    public RectTransform imageObject;
    public RawImage image;

    public RectTransform descGameOject;
    public TextMeshProUGUI txtDesc;

    public GameObject beforeObj;
    public GameObject nextObj;

    private const string PATH = "Arts/Encyclopedia/System";

    private string script1;
    private string script2;
    private string script3;
    private string script4;
    private string script5;

    private string page1;
    private string page2;
    private string page3;
    private string page4;
    private string page5;

    private int pageIndex = 1;
    private int endPage;

    private void OnEnable()
    {
        pageIndex = 1;
        GetSystemDescData();
    }

    private void Update()
    {

        SetScriptOfPage();
    }

    private void GetSystemDescData()    // 데이터 가져오기
    {
        systemDescData = DataTableManager.Instance.GetSystemDescData(DataTableManager.Instance.systemDescId);

        script1 = systemDescData.systemScript1;
        script2 = systemDescData.systemScript2;
        script3 = systemDescData.systemScript3;
        script4 = systemDescData.systemScript4;
        script5 = systemDescData.systemScript5;

        endPage = systemDescData.endPage;

        page1 = systemDescData.page1;
        page2 = systemDescData.page2;
        page3 = systemDescData.page3;
        page4 = systemDescData.page4;
        page5 = systemDescData.page5;

        //title.text = systemDescData.systemName;
    }

    public void BackBtnOfSystemDescUI()   // 뒤로가기 버튼
    {
        AudioManager.Instance.PlaySFX(SFX.BookFlip5);
        UIManager.Instance.CloseUI(this);

        var systemUI = new BaseUIData();
        UIManager.Instance.OpenUI<SystemUI>(systemUI);
    }

    public void SetScriptOfPage()
    {
        image.color = new Color(1, 1, 1, 1);
        descGameOject.anchoredPosition = new Vector3(0f, -241f, 0f);
        imageObject.sizeDelta = new Vector2(1920f, 1080f);
        switch (pageIndex)
        {
            case 1:
                txtDesc.text = script1;
                image.texture = Resources.Load($"{PATH}/{page1}") as Texture2D;
                if (page1 == "")
                {
                    image.color = new Color(0, 0, 0, 0);
                    descGameOject.anchoredPosition = new Vector3(0f, 0f, 0f);
                }
                break;
            case 2:
                txtDesc.text = script2;
                image.texture = Resources.Load($"{PATH}/{page2}") as Texture2D;
                if (page2 == "")
                {
                    image.color = new Color(0, 0, 0, 0);
                    descGameOject.anchoredPosition = new Vector3(0f, 0f, 0f);
                }
                break;
            case 3:
                txtDesc.text = script3;
                image.texture = Resources.Load($"{PATH}/{page3}") as Texture2D;
                if (page3 == "")
                {
                    image.color = new Color(0, 0, 0, 0);
                    descGameOject.anchoredPosition = new Vector3(0f, 0f, 0f);
                }
                break;
            case 4:
                txtDesc.text = script4;
                image.texture = Resources.Load($"{PATH}/{page4}") as Texture2D;
                if (page4 == "")
                {
                    image.color = new Color(0, 0, 0, 0);
                    descGameOject.anchoredPosition = new Vector3(0f, 0f, 0f);
                }
                break;
            case 5:
                txtDesc.text = script5;
                image.texture = Resources.Load($"{PATH}/{page5}") as Texture2D;
                if (page5 == "")
                {
                    image.color = new Color(0, 0, 0, 0);
                    descGameOject.anchoredPosition = new Vector3(0f, 0f, 0f);
                }
                break;
        }

        if(DataTableManager.Instance.systemDescId == 590001)
        {
            imageObject.sizeDelta = new Vector2(2048f, 1134f);
        }

        if(pageIndex == 1)
        {
            beforeObj.SetActive(false);
            nextObj.SetActive(true);

            if(pageIndex == endPage)
                nextObj.SetActive(false);
        }
        else if(pageIndex == endPage)
        {
            beforeObj.SetActive(true);
            nextObj.SetActive(false);
        }
        else
        {
            beforeObj.SetActive(true);
            nextObj.SetActive(true);
        }
    }

    public void NextBtn()
    {
        pageIndex++;
        AudioManager.Instance.PlaySFX(SFX.BookFlip3);
    }

    public void BeforeBtn()
    {
        pageIndex--;
        AudioManager.Instance.PlaySFX(SFX.BookFlip4);
    }

    public override void OnClickCloseButton()
    {
        AudioManager.Instance.PlaySFX(SFX.CloseBook);
        base.OnClickCloseButton();
    }
}
