using UnityEngine;

public class TitleManager : MonoBehaviour
{
    void OnEnable()
    {
        Invoke("PlayTitleBGM", 0.1f);
    }

    private void PlayTitleBGM() {
        if (AudioManager.Instance != null) {
            AudioManager.Instance.UnMute();
            AudioManager.Instance.PlayBGM(BGM.Start);
        }
        else Invoke("PlayTitleBGM", 0.1f);
    }
}
