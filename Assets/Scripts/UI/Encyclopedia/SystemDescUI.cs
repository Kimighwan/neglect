using TMPro;
using UnityEngine;
public class SystemDescUI : SystemUI
{
    public SystemDescData systemDescData;

    //public TextMeshProUGUI title;
    public TextMeshProUGUI txtDesc;

    public GameObject beforeObj;
    public GameObject nextObj;


    private string script1;
    private string script2;
    private string script3;
    private string script4;
    private string script5;

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
        switch (pageIndex)
        {
            case 1:
                txtDesc.text = script1;
                break;
            case 2:
                txtDesc.text = script2;
                break;
            case 3:
                txtDesc.text = script3;
                break;
            case 4:
                txtDesc.text = script4;
                break;
            case 5:
                txtDesc.text = script5;
                break;
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
