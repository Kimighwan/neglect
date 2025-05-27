using UnityEngine;

public class ReceptionUI : BaseUI
{
    // private bool adTutorial = false;
    // private bool qTutorial = false;
    public override void Init(Transform anchor, RectTransform canvasRT = null)
    {
        base.Init(anchor);

        var rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
        rectTransform.sizeDelta = new Vector2(1920f, 1080f);
    }

    public void OnClickAdventrueListBtn()
    {
        var adventurerListUI = new BaseUIData();
        UIManager.Instance.CloseUI(this);
        UIManager.Instance.OpenUI<AdventurerListUI>(adventurerListUI);
        // if (!adTutorial) {
        //     adTutorial = true;
        //     GameManager.gameManager.OpenTutorial(590005);
        // }
    }

    public void OnClickQuestListBtn()
    {
        var questListUI = new BaseUIData();
        UIManager.Instance.CloseUI(this);
        UIManager.Instance.OpenUI<QuestListUI>(questListUI);
        // if (!qTutorial) {
        //     qTutorial = true;
        //     GameManager.gameManager.OpenTutorial(590003);
        // }
    }

    public void OnClickNewQuestListBtn()
    {
        var questListUI = new BaseUIData();
        UIManager.Instance.CloseUI(this);
        UIManager.Instance.OpenUI<TodayQuestUI>(questListUI);
        // if (!qTutorial) {
        //     qTutorial = true;
        //     GameManager.gameManager.OpenTutorial(590003);
        // }
    }
}
