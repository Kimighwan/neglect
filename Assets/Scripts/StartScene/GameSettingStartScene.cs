using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GameSettingStartScene : MonoBehaviour
{
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

    void UpdateVolumes()
    {
        // 최종 볼륨은 마스터 볼륨과 개별 볼륨의 곱으로 계산
        float finalBGMVolume = masterVolume.value * BGMVolume.value / 100.0f;
        float finalSFXVolume = masterVolume.value * SFXVolume.value / 100.0f;

        master.text = ((int)masterVolume.value).ToString();
        bgm.text = ((int)BGMVolume.value).ToString();
        sfx.text = ((int)SFXVolume.value).ToString();

        AudioManager.Instance.ChangeBGMVolume(finalBGMVolume);
        AudioManager.Instance.ChangeSFXVolume(finalSFXVolume);
    }

    public void OnClickGameSetting()
    {
        this.gameObject.SetActive(true);
    }
    public void OnClickCloseButton()
    {
        this.gameObject.SetActive(false);
    }
}
