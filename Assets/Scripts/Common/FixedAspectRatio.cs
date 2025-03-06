using UnityEngine;

public class FixedAspectRatio : MonoBehaviour
{
    // 목표 비율 (여기서는 16:9)
    public float targetAspect = 16.0f / 9.0f;

    void Start()
    {
        // 현재 창의 비율 계산
        float windowAspect = (float)Screen.width / (float)Screen.height;
        // 현재 창 비율과 목표 비율의 상대적 높이 스케일 계산
        float scaleHeight = windowAspect / targetAspect;

        Camera camera = GetComponent<Camera>();

        if (scaleHeight < 1.0f)
        {
            // 화면의 높이가 목표보다 작으면 letterboxing 적용
            Rect rect = camera.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            camera.rect = rect;
        }
        else
        {
            // 화면의 폭이 목표보다 작으면 pillarboxing 적용
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = camera.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            camera.rect = rect;
        }
    }
}
