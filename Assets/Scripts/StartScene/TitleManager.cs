using UnityEngine;

public class TitleManager : MonoBehaviour
{
    void OnEnable()
    {
        Invoke("PlayTitleBGM", 0.1f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) AudioManager.Instance.PlaySFX(SFX.Click2);
    }

    private void PlayTitleBGM() {
        if (AudioManager.Instance != null) {
            AudioManager.Instance.UnMute();
            AudioManager.Instance.PlayBGM(BGM.Start);
        }
        else Invoke("PlayTitleBGM", 0.1f);
    }
}
