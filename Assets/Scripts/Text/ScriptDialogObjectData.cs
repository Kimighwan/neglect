using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScriptDialogObjectData : MonoBehaviour
{
    public static ScriptDialogObjectData data;
    private Camera mainCamera;

    private void Awake()
    {
        data = this;
        ActiveAllDialogObject(true, true);
    }
    private void Start()
    {
        mainCamera = Camera.main;
        PlaceDialog();
        ActiveAllDialogObject(false, false);
    }

    #region ScriptMode

    [Header("ScriptMode")]
    public GameObject background;
    public GameObject panel;
    public GameObject skipBtn;
    [TextArea]
    public TextMeshProUGUI scr;
    public TextMeshProUGUI scrSpeaker;

    #endregion ScriptMode

    #region DialogMode

    [Header("DialogMode")]
    public GameObject malpungseon1;
    public GameObject malpungseon2;
    public Transform speaker1;
    public Transform speaker2;
    public Transform background1;
    public Transform background2;

    public GameObject malpungseonUI1;
    public GameObject malpungseonUI2;
    public TextMeshProUGUI speaker1Text;
    public TextMeshProUGUI speaker2Text;
    public TextMeshProUGUI malpungseon1Text;
    public TextMeshProUGUI malpungseon2Text;
    public RectTransform speaker1RT;
    public RectTransform speaker2RT;
    public RectTransform malpungseon1RT;
    public RectTransform malpungseon2RT;

    public void ActiveAllDialogObject(bool a, bool b) {
        malpungseonUI1.SetActive(a);
        malpungseonUI2.SetActive(b);
        malpungseon1.SetActive(a);
        malpungseon2.SetActive(b);
    }
    private void PlaceDialog() {
        Vector3 screenPos;
        screenPos = mainCamera.WorldToScreenPoint(background1.position);
        screenPos += new Vector3(0, 7.5f, 0f);
        malpungseon1RT.position = screenPos;
        screenPos = mainCamera.WorldToScreenPoint(speaker1.position);
        speaker1RT.position = screenPos;
        screenPos = mainCamera.WorldToScreenPoint(background2.position);
        screenPos += new Vector3(0, 7.5f, 0f);
        malpungseon2RT.position = screenPos;
        screenPos = mainCamera.WorldToScreenPoint(speaker2.position);
        speaker2RT.position = screenPos;
    }
    
    #endregion DialogMode
}
