using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    private bool adTutorialOnce = false;
    public GameObject image;
    public GameObject resultBtn;


    public void OnClickQuestTestBtn(int index)
    {
        var detachQuestUI = new QuestIndex(index);
        UIManager.Instance.OpenUI<DetachQuestListUI>(detachQuestUI);
    }

    public void OnClickAdventureTestBtn(int index)
    {
        var detachAdventureUI = new AdventureIndexClass(index);
        UIManager.Instance.OpenUI<DetachAdventureListUI>(detachAdventureUI);
        if (!adTutorialOnce) {
            adTutorialOnce = true;
            transform.parent.parent.gameObject.GetComponent<Request>().ClearAdTutorial();
            GameManager.gameManager.OpenTutorial(590007);
        }
    }
    public void SetAdTutorialOnceTrue() {
        adTutorialOnce = true;
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
        resultBtn.SetActive(true);
    }

    public void OnClickAwakeBtn(int index)
    {
        // Quest Awake
        if (PoolManager.Instance.usingQuestList.Count != 0)
        {
            PoolManager.Instance.usingQuestList.Remove(QuestManager.Instance.questData[index].questId);
            QuestData.questSelectedId = 0;

            QuestManager.Instance.questData.Remove(index);

            QuestManager.Instance.questTxt[index - 1].text = "의뢰 선택";
        }

        // Adventure Awake
        if (PoolManager.Instance.usingAdventureList.Count != 0)
        {
            for (int i = 0; i < 4; i++)
            {
                PoolManager.Instance.usingAdventureList.Clear();
            }

            AdventureData.adventureSelectId.Clear();

            QuestManager.Instance.adventureDatas[index].Clear();

            QuestManager.Instance.adventureTxt[index - 1].text = "모험가 선택";
        } 
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

public class AdventurerUIWithDesk : BaseUIData {
    public Desk desk;
    public AdventurerUIWithDesk(Desk d) {
        desk = d;
    }
}

public class RoomIndex : BaseUIData {
    public int index;
    public RoomIndex(int i, bool b) {
        index = i;
    }
}

public class StringInfo : BaseUIData {
    public string str;
    public StringInfo(string s) {
        str = s;
    }
}