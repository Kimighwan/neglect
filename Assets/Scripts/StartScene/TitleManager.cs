using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {

    }

    void OnEnable()
    {
        Invoke("PlayTitleBGM", 0.25f);
    }

    private void PlayTitleBGM() {
        if (AudioManager.Instance != null) {
            AudioManager.Instance.UnMute();
            AudioManager.Instance.ChangeBGMVolume(0.5f);
            AudioManager.Instance.ChangeSFXVolume(0.5f);
            AudioManager.Instance.PlayBGM(BGM.Start);
        }
        else Invoke("PlayTitleBGM", 0.25f);
    }
}
