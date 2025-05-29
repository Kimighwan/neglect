using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    [Tooltip("반복해서 사용할 배경 Transform들을 순서대로 할당 (둘 이상)")]
    public Transform[] backgrounds;

    [Tooltip("스크롤 속도 (단위: 월드 유닛/초)")]
    public float speed = 2f;

    private float width;  // 배경 하나의 너비

    void Start()
    {
        if (backgrounds == null || backgrounds.Length < 2)
        {
            Debug.LogError("Backgrounds 배열에 2개 이상의 오브젝트를 할당해야 합니다.");
            enabled = false;
            return;
        }

        // 첫 번째 배경의 너비 계산
        var sr = backgrounds[0].GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("배경 오브젝트에 SpriteRenderer가 없습니다.");
            enabled = false;
            return;
        }
        width = sr.bounds.size.x;
    }

    void Update()
    {
        float move = speed * Time.deltaTime;

        // 1) 모든 배경을 왼쪽으로 이동
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].position += Vector3.left * move;
        }

        // 2) 완전히 왼쪽으로 나간 배경을 오른쪽 끝으로 이동
        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (backgrounds[i].position.x <= -width)
            {
                // 화면 오른쪽 끝에 있는 배경의 x 좌표를 찾고,
                float rightmostX = backgrounds[0].position.x;
                for (int j = 1; j < backgrounds.Length; j++)
                    rightmostX = Mathf.Max(rightmostX, backgrounds[j].position.x);

                // 해당 배경을 오른쪽 끝 바로 다음 위치로 옮김
                backgrounds[i].position = new Vector3(
                    rightmostX + width,
                    backgrounds[i].position.y,
                    backgrounds[i].position.z
                );
            }
        }
    }
}
