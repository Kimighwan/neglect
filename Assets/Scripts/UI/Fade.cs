using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : SingletonBehaviour<Fade>
{
    public Image fadeImg;
    public Image illImg;

    protected override void Init()
    {
        base.Init();

        fadeImg.transform.localScale = Vector3.zero;    // 일단 fade Image 안 보이게 설정
    }

    public void DoFade(Color color, float startAlpha, float endAlpha, float duration, float startDelay, bool deactivateOnFinish, bool ill, Action onFinish = null)
    {
        StartCoroutine(FadeCo(color, startAlpha, endAlpha, duration, startDelay, deactivateOnFinish, ill, onFinish));
    }

    /// <summary>
    /// startAlpha로 알파값이 시작하여 duration 시간에 걸쳐서 endAlpha 알파값으로 도달
    /// 
    /// 컬러는 Color.black 사용
    /// 
    /// DoFade(Color.black, 0f, 1f, 0.5f, 0f, false, () => {} );
    /// 람다 함수는 딱히 필요없으면 안 적어도 됨(굳이 람다 함수일 이유는 없지만 편함)
    /// fade 함수가 작동이 끝나고 동작할 행위가 있다면 사용
    /// 
    /// deactivate은 화면이 밝아질 때 true로 설정하여 마지막에 fade image가 안 보이게 설정
    /// 
    /// ill 변수는 true이면 일러스트 보이게 false이면 사라지게
    /// </summary>
    private IEnumerator FadeCo(Color color, float startAlpha, float endAlpha, float duration, float startDelay, bool deactivateOnFinish, bool ill, Action onFinish)
    {
        if (!ill)
        {
            illImg.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(startDelay);    // Delay...

        fadeImg.transform.localScale = Vector3.one;
        fadeImg.color = new Color(color.r, color.g, color.b, startAlpha);

        var startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < duration)
        {
            fadeImg.color = new Color(color.r, color.g, color.b, Mathf.Lerp(startAlpha, endAlpha, (Time.realtimeSinceStartup - startTime) / duration));
            yield return null;
        }

        if (ill)
        {
            illImg.gameObject.SetActive(true);
        }


        fadeImg.color = new Color(color.r, color.g, color.b, endAlpha);

        if (deactivateOnFinish)
            fadeImg.transform.localScale = Vector3.zero;

        onFinish?.Invoke();
    }
}
