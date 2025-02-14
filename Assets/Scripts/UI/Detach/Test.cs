using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public void OnClickQuestTestBtn()
    {
        var detachQuestUI = new BaseUIData();
        UIManager.Instance.OpenUI<DetachQuestListUI>(detachQuestUI);
    }

    public void OnClickAdventureTestBtn()
    {
        var detachAdventureUI = new BaseUIData();
        UIManager.Instance.OpenUI<DetachAdventureListUI>(detachAdventureUI);
    }

    public void OnClickQuestResultTestBtn()
    {
        var questResult = new BaseUIData();
        UIManager.Instance.OpenUI<QuestResult>(questResult);
    }
}
