using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingUI : BaseUI
{
    private bool wasPause = true;
    private bool wasFastMode = false;
    public Slider masterVolume;
    public Slider BGMVolume;
    public Slider SFXVolume;

    public TextMeshProUGUI master;
    public TextMeshProUGUI bgm;
    public TextMeshProUGUI sfx;

    void Start()
    {
        masterVolume.onValueChanged.AddListener(delegate { UpdateVolumes(); });
        BGMVolume.onValueChanged.AddListener(delegate { UpdateVolumes(); });
        SFXVolume.onValueChanged.AddListener(delegate { UpdateVolumes(); });
    }

    void OnEnable()
    {
        GetUpdateAudioManager();
        if (!GameManager.gameManager.Pause) {
            wasPause = false;
            if (GameManager.gameManager.FastMode) {
                wasFastMode = true;
            }
            GameManager.gameManager.PauseGame();
        }
    }

    void UpdateVolumes()
    {
        AudioManager.Instance.MasterVol = masterVolume.value / 10.0f;
        AudioManager.Instance.BgmVol = BGMVolume.value / 10.0f;
        AudioManager.Instance.SfxVol = SFXVolume.value / 10.0f;
        AudioManager.Instance.UpdateVolume();
        master.text = ((int)masterVolume.value).ToString();
        bgm.text = ((int)BGMVolume.value).ToString();
        sfx.text = ((int)SFXVolume.value).ToString();
    }

    //public void GoToTitle() {
    //    GameManager.gameManager.GoToTitle();
    //}

    public void OnClickGameQuit()
    {
        var uiData = new ConfirmUIData();
        uiData.confirmType = ConfirmType.OK_CANCEL;
        uiData.descTxt = "종료하시겠습니까?";
        uiData.okBtnTxt = "종료";
        uiData.cancelBtnTxt = "취소";
        uiData.onClickOKBtn = () =>
        {
            Fade.Instance.DoFade(Color.black, 0f, 1f, 1f, 0f, false, () =>
            {
                Application.Quit();
            });
        };
        UIManager.Instance.OpenUI<ConfirmUI>(uiData);
    }

    public override void OnClickCloseButton()
    {
        if (!wasPause) GameManager.gameManager.PauseGame();
        if (wasFastMode) GameManager.gameManager.QuickGame();
        wasFastMode = false;
        wasPause = true;
        base.OnClickCloseButton();
    }

    private void GetUpdateAudioManager() {
        masterVolume.value = AudioManager.Instance.MasterVol * 10f;
        BGMVolume.value = AudioManager.Instance.BgmVol * 10f;
        SFXVolume.value = AudioManager.Instance.SfxVol * 10f;
        master.text = ((int)masterVolume.value).ToString();
        bgm.text = ((int)BGMVolume.value).ToString();
        sfx.text = ((int)SFXVolume.value).ToString();
    }
}
