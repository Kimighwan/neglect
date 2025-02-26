using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneButton : MonoBehaviour
{
    public void OnClickStartButton()
    {
        AudioManager.Instance.ChangeBGMVolume(0.1f);
        AudioManager.Instance.PlaySFX(SFX.GameStart);
        Fade.Instance.DoFade(Color.black, 0f, 1f, 1f, 0f, false, () =>
        {
            SceneManager.LoadScene(1);
        });

    }

    public void OnClickQuitButton()
    {
        Application.Quit();
    }
}
