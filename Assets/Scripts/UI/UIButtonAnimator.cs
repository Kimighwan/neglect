using UnityEngine;
using UnityEngine.EventSystems;

public enum ButtonAnim {
    ScaleChange,
}

public class UIButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ButtonAnim currentAnim;
    public float transitionDuration = 0.1f;
    private bool unActiveAnim = false;

    // Scale Change Animation
    private Vector3 originalScale;
    public GameObject setImage;
    public float scaleFactor = 1.1f;
    

    private void Start()
    {
        // 초기 스케일 저장
        if (setImage == null) originalScale = transform.localScale;
        else originalScale = setImage.transform.localScale;
    }

    // 마우스가 버튼 위로 들어왔을 때 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (unActiveAnim) return;
        // 즉시 스케일을 변경하는 경우
        // transform.localScale = originalScale * scaleFactor;

        // 또는 코루틴을 통해 부드럽게 변경
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale * scaleFactor));
    }

    // 마우스가 버튼에서 나갔을 때 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        if (unActiveAnim) return;
        // 즉시 원래 스케일로 복원
        // transform.localScale = originalScale;

        // 부드럽게 원래 스케일로 복원
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale));
    }

    // 스케일을 부드럽게 변경하는 코루틴
    private System.Collections.IEnumerator ScaleTo(Vector3 targetScale)
    {
        Vector3 currentScale;
        if (setImage == null) currentScale = transform.localScale;
        else currentScale = setImage.transform.localScale;

        float timer = 0f;
        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            if (setImage == null) transform.localScale = Vector3.Lerp(currentScale, targetScale, timer / transitionDuration);
            else setImage.transform.localScale = Vector3.Lerp(currentScale, targetScale, timer / transitionDuration);
            yield return null;
        }

        if (setImage == null) transform.localScale = targetScale;
        else setImage.transform.localScale = targetScale;
        
    }

    public void SetUnActiveAnim(bool b) {
        unActiveAnim = b;
    }
}
