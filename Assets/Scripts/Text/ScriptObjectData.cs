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
    public GameObject malpungseon3;
    public GameObject malpungseon4;
    public Transform speaker1;
    public Transform speaker2;
    public Transform speaker3;
    public Transform speaker4;
    public Transform background1;
    public Transform background2;
    public Transform background3;
    public Transform background4;

    public GameObject malpungseon11;
    public GameObject malpungseon22;
    public GameObject malpungseon33;
    public GameObject malpungseon44; 
    public TextMeshProUGUI speaker1Text;
    public TextMeshProUGUI speaker2Text;
    public TextMeshProUGUI speaker3Text;
    public TextMeshProUGUI speaker4Text;
    public TextMeshProUGUI malpungseon1Text;
    public TextMeshProUGUI malpungseon2Text;
    public TextMeshProUGUI malpungseon3Text;
    public TextMeshProUGUI malpungseon4Text;
    public RectTransform speaker1RT;
    public RectTransform speaker2RT;
    public RectTransform speaker3RT;
    public RectTransform speaker4RT;
    public RectTransform malpungseon1RT;
    public RectTransform malpungseon2RT;
    public RectTransform malpungseon3RT;
    public RectTransform malpungseon4RT;
    private void ActiveAllDialogObject() {
        malpungseon11.SetActive(true);
        malpungseon22.SetActive(true);
        malpungseon33.SetActive(true);
        malpungseon44.SetActive(true);
        malpungseon1.SetActive(true);
        malpungseon2.SetActive(true);
        malpungseon3.SetActive(true);
        malpungseon4.SetActive(true);
    }
    private void UnActiveAllDialogObject() {
        malpungseon11.SetActive(false);
        malpungseon22.SetActive(false);
        malpungseon33.SetActive(false);
        malpungseon44.SetActive(false);
        malpungseon1.SetActive(false);
        malpungseon2.SetActive(false);
        malpungseon3.SetActive(false);
        malpungseon4.SetActive(false);
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
        screenPos = mainCamera.WorldToScreenPoint(background3.position);
        malpungseon3RT.position = screenPos;
        screenPos = mainCamera.WorldToScreenPoint(speaker3.position);
        speaker3RT.position = screenPos;
        screenPos = mainCamera.WorldToScreenPoint(background4.position);
        malpungseon4RT.position = screenPos;
        screenPos = mainCamera.WorldToScreenPoint(speaker4.position);
        speaker4RT.position = screenPos;
    }
    
    #endregion DialogMode
}
