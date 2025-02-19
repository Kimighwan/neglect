using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScriptObjectData : MonoBehaviour
{
    public static ScriptObjectData data;
    private Camera mainCamera;

    private void Awake()
    {
        data = this;
        ActiveAllDialogObject();
    }
    private void Start()
    {
        mainCamera = Camera.main;
        PlaceDialog();
        UnActiveAllDialogObject();
    }

    #region ScriptMode

    [Header("ScriptMode")]
    public GameObject background;
    public GameObject panel;
    public GameObject skipBtn;
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

    public GameObject malpungseon11;
    public GameObject malpungseon22;
    public TextMeshProUGUI speaker1Text;
    public TextMeshProUGUI speaker2Text;
    public TextMeshProUGUI malpungseon1Text;
    public TextMeshProUGUI malpungseon2Text;
    public RectTransform speaker1RT;
    public RectTransform speaker2RT;
    public RectTransform malpungseon1RT;
    public RectTransform malpungseon2RT;

    private List<int> dialogStartId = new List<int>{ 101811, 101821, 101831, 101911, 101921, 101931 };
    private List<int> dialogEndId = new List<int>{ 101816, 101827, 101835, 101915, 101927, 101938 };
    private List<int> dialogStartDay = new List<int> {3, 5, 7, 9, 11, 13};
    private List<float> dialogStartTime = new List<float> {9f, 8f, 10f, 10f, 8f, 9f};
    public int GetDialogStartId(int i) { return dialogStartId[i]; }
    public int GetDialogEndId(int i) { return dialogEndId[i]; }
    public int GetDialogStartDay(int i) { return dialogStartDay[i]; }
    public float GetDialogStartTime(int i) { return dialogStartTime[i]; }

    private void ActiveAllDialogObject() {
        malpungseon11.SetActive(true);
        malpungseon22.SetActive(true);
        malpungseon1.SetActive(true);
        malpungseon2.SetActive(true);
    }
    private void UnActiveAllDialogObject() {
        malpungseon11.SetActive(false);
        malpungseon22.SetActive(false);
        malpungseon1.SetActive(false);
        malpungseon2.SetActive(false);
    }
    private void PlaceDialog() {
        Vector3 screenPos;
        screenPos = mainCamera.WorldToScreenPoint(background1.position);
        malpungseon1RT.position = screenPos;
        screenPos = mainCamera.WorldToScreenPoint(speaker1.position);
        speaker1RT.position = screenPos;
        screenPos = mainCamera.WorldToScreenPoint(background2.position);
        malpungseon2RT.position = screenPos;
        screenPos = mainCamera.WorldToScreenPoint(speaker2.position);
        speaker2RT.position = screenPos;
    }
    
    #endregion DialogMode
}
