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

    public void OnClickAwakeBtn(int index)
    {
        // Quest Awake
        if (PoolManager.Instance.usingQuestList.Count != 0)
        {
            PoolManager.Instance.usingQuestList.Remove(QuestManager.Instance.questData[index].questId);
            QuestData.questSelectedId = 0;

            QuestManager.Instance.questData.Remove(index);
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