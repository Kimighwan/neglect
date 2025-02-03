using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneButton : MonoBehaviour
{
    public void OnClickStartButton()
    {
        Fade.Instance.DoFade(Color.black, 0f, 1f, 1f, 0f, false, true, () =>
        {
            SceneManager.LoadScene(1);
        });

    }

    public void OnClickQuitButton()
    {
        Application.Quit();
    }
}
