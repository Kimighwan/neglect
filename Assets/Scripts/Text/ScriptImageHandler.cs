using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptImageHandler : MonoBehaviour
{
    public Image BackGround;
    public Image LeftSpeaker;  // UI에서 캐릭터를 표시할 Image 컴포넌트
    public Image RightSpeaker;
    private Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();

    public void SetCharacter(string c, string e, bool isLeft)
    {
        // 캐시된 이미지가 있는지 확인
        if (!spriteCache.TryGetValue(c, out Sprite sprite))
        { // 캐시에 없으면 Resources 폴더에서 로드
            string fileName = c + '_' + e;
            string path = $"Arts/Characters/{c}/{fileName}";
            sprite = Resources.Load<Sprite>(path);

            if (sprite != null)
            {
                spriteCache[c] = sprite;
            }
            else
            {
                Debug.LogWarning($"[SetCharacter] {path} 경로에서 스프라이트를 찾을 수 없음!");
                return;
            }
        }
        if (isLeft) LeftSpeaker.sprite = sprite; // 캐싱된 이미지 사용
        else RightSpeaker.sprite = sprite;
    }
}
