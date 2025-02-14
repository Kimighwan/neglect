using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdvemtureDetailDescUI : BaseUI
{
    public TextMeshProUGUI txt;

    public override void Init(Transform anchor)
    {
        base.Init(anchor);

        txt.text = UIManager.Instance.advemtureDetailDescText;
    }
}
