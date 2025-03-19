using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public Image background;
    public GameObject backPanel;
    public GameObject panel;
    public GameObject skipBtn;
    public TextMeshProUGUI scr;
    public TextMeshProUGUI scrSpeaker;
    private List<int> scrStartId = new List<int> { 100001, 100018, 100101, 100201, 100301 };
    private List<int> scrEndId = new List<int> { 100017, 100036, 100120, 100219, 100315 };
    private List<int> scrStartDay = new List<int> { 1, 1, 5, 10, 15};
    private List<int> scrStartTime = new List<int> { 8, 8, 12, 12, 12};
    public int GetScrStartId(int i) { return scrStartId[i]; }
    public int GetScrEndId(int i) { return scrEndId[i]; }
    public int GetScrStartDay(int i) { return scrStartDay[i]; }
    public int GetScrStartTime(int i) { return scrStartTime[i]; }
    public void SetScriptOnly() {
        background.sprite = null;
        background.color = new Color(0.75f, 0.75f, 0.75f, 0.75f);
    }
    public void SetScriptWithIll(string fileName) {
        if (fileName == "0") {
            AudioManager.Instance.StopBGM();
            background.sprite = null;
            background.color = new Color(0f, 0f, 0f, 1f);
            return;
        }
    }

    #endregion ScriptMode

    #region DialogMode

    [Header("DialogMode")]
    private List<int> diaStartId = new List<int> { 101911, 101921, 101931 };
    private List<int> diaEndId = new List<int> { 101915, 101927, 101938 };
    private List<int> diaStartDay = new List<int> { 4, 9, 14 };
    private List<int> diaStartTime = new List<int> { 9, 9, 9 };
    public int GetDiaStartId(int i) { return diaStartId[i]; }
    public int GetDiaEndId(int i) { return diaEndId[i]; }
    public int GetDiaStartDay(int i) { return diaStartDay[i]; }
    public int GetDiaStartTime(int i) { return diaStartTime[i]; }
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
