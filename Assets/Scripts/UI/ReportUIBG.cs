using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReportUIBG : MouseDrag
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(SFX.Stamp);
    }
}
