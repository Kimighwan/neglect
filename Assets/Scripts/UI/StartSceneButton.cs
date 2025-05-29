using TMPro;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneButton : MonoBehaviour
{
    public TextMeshProUGUI gameVersionTxt;
    public GameObject pressToStart;
    public Image startButton;
    public Image quitButton;

    private bool isPressAnyKey;

    private void Awake()
    {
        gameVersionTxt.text = $"Version : {Application.version}";
    }

    void OnEnable()
    {
        isPressAnyKey = false;
    }

    private void Update()
    {
        if (!isPressAnyKey && Input.anyKeyDown)
        {
            isPressAnyKey = true;
            OnClickPressAnyKey();
        }
    }

    public void OnClickStartButton()
    {
        AudioManager.Instance.BgmVol = AudioManager.Instance.BgmVol / 10.0f;
        AudioManager.Instance.UpdateVolume();
        AudioManager.Instance.PlaySFX(SFX.GameStart);

        StartFadeButtons(1f, 1f, 0f, () =>
        {
            Transitioner.Instance.TransitionToScene(1);
        });
    }

    public void OnClickQuitButton()
    {
        Fade.Instance.DoFade(Color.black, 0f, 1f, 1f, 0f, false, () =>
        {
            Application.Quit();
        });
    }

    public void OnClickPressAnyKey()
    {
        isPressAnyKey = true;
        pressToStart.GetComponent<FadingText>().StartFadeOut(1f);
        PressToStart(2f);
    }

    public void PressToStart(float duration)
    {
        StartCoroutine(CameraMove(duration));
    }

    public GameObject clouds;
    public SpriteRenderer logo;
    private IEnumerator CameraMove(float duration)
    {
        float elapsed = 0f;
        Camera camera = Camera.main;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float a = Mathf.Lerp(7.5f, 0f, elapsed / duration);
            camera.transform.position = new Vector3(0f, a, -10f);
            clouds.transform.position = new Vector3(0f, a, 0f);

            float b = Mathf.Lerp(1f, 0f, elapsed / duration);
            logo.color = new Color(1f, 1f, 1f, b);
            yield return null;
        }

        camera.transform.position = new Vector3(0f, 0f, -10f);
        clouds.transform.position = new Vector3(0f, 0f, 0f);
        logo.color = new Color(1f, 1f, 1f, 0f);
        StartFadeButtons(2f, 0f, 1f);
    }

    public void StartFadeButtons(float duration, float startAlpha, float endAlpha, Action onFinish = null)
    {
        StartCoroutine(FadeButtons(duration, startAlpha, endAlpha, onFinish));
    }

    private IEnumerator FadeButtons(float duration, float startAlpha, float endAlpha, Action onFinish = null)
    {
        float elapsed = 0f;
        Color startColor = new Color(1f, 1f, 1f, startAlpha);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float a = Mathf.Lerp(startColor.a, endAlpha, elapsed / duration);
            startButton.color = new Color(startColor.r, startColor.g, startColor.b, a);
            quitButton.color = new Color(startColor.r, startColor.g, startColor.b, a);
            yield return null;
        }

        startButton.color = new Color(startColor.r, startColor.g, startColor.b, endAlpha);
        quitButton.color = new Color(startColor.r, startColor.g, startColor.b, endAlpha);

        onFinish?.Invoke();
    }
}
