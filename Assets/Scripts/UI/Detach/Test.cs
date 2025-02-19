using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject image;
    public GameObject btn;
    public void OnClickQuestTestBtn(int index)
    {
        var detachQuestUI = new QuestIndex(index);
        UIManager.Instance.OpenUI<DetachQuestListUI>(detachQuestUI);
    }

    public void OnClickAdventureTestBtn(int index)
    {
        var detachAdventureUI = new AdventureIndexClass(index);
        UIManager.Instance.OpenUI<DetachAdventureListUI>(detachAdventureUI);
    }

    public void OnClickQuestResultTestBtn(int index)
    {
        var questResult = new QuestResultIndex(index);
        UIManager.Instance.OpenUI<QuestResult>(questResult);
    }

    public void OnClickQuestStart(int index)
    {
        QuestManager.Instance.OnClickQusetStart(index);
        image.SetActive(true);
        StartCoroutine(testt());
    }

    private IEnumerator testt()
    {
        yield return new WaitForSeconds(1.5f);
        image.SetActive(false);
        btn.SetActive(true);
    }
}

public class AdventureIndexClass : BaseUIData
{
    public int index;

    public AdventureIndexClass(int i)
    {
        index = i;
    }
}

public class QuestIndex : BaseUIData
{
    public int index;

    public QuestIndex(int i)
    {
        index = i;
    }
}

public class QuestResultIndex : BaseUIData
{
    public int index;

    public QuestResultIndex(int i)
    {
        index = i;
    }
}