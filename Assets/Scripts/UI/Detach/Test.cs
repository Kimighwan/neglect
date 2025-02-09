using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public void OnClickQuestTestBtn()
    {
        var detachQuestUI = new BaseUIData();
        UIManager.Instance.OpenUI<DetachSelectQuestUI>(detachQuestUI);
    }
}
