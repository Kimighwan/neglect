using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestOverRay : MouseDrag
{
    public GameObject targetObject;

    private Button currentQuestBtn;

    private void Awake()
    {
        currentQuestBtn = GetComponent<Button>();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if(!currentQuestBtn.interactable)
            targetObject.SetActive(true);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        targetObject.SetActive(false);
    }
}
