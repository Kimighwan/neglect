using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptImageHandler : MonoBehaviour
{
    public Image BackGround;
    public Image LeftSpeaker;  // UI에서 캐릭터를 표시할 Image 컴포넌트
    public Image MiddleSpeaker;
    public Image RightSpeaker;
    private Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();

    public void SetCharacter(string c, string e, int pos) // 0 left, 1 middle, 2 right
    {
        string fileName = c + '_' + e;
        // 캐시된 이미지가 있는지 확인
        if (!spriteCache.TryGetValue(fileName, out Sprite sprite))
        { // 캐시에 없으면 Resources 폴더에서 로드
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
        switch (pos) {
            case 0 :
                ActiveImage(pos);
                LeftSpeaker.sprite = sprite;
                Vector3 scale = LeftSpeaker.rectTransform.localScale;
                scale.x = -1; 
                LeftSpeaker.rectTransform.localScale = scale;
                break;
            case 1 :
                ActiveImage(pos);
                MiddleSpeaker.sprite = sprite;
                break;
            case 2 :
                ActiveImage(pos);
                RightSpeaker.sprite = sprite;
                break;
        }
    }
    public void EndTheScripts() {
        LeftSpeaker.gameObject.SetActive(false);
        MiddleSpeaker.gameObject.SetActive(false);
        RightSpeaker.gameObject.SetActive(false);
    }

    private void ActiveImage(int pos) {
        switch (pos) {
            case 0 :
                LeftSpeaker.gameObject.SetActive(true);
                break;
            case 1 :
                MiddleSpeaker.gameObject.SetActive(true);
                break;
            case 2 :
                RightSpeaker.gameObject.SetActive(true);
                break;
        }
    }

    private void UnActiveImage(int pos) {
        switch (pos) {
            case 0 :
                LeftSpeaker.gameObject.SetActive(false);
                break;
            case 1 :
                MiddleSpeaker.gameObject.SetActive(false);
                break;
            case 2 :
                RightSpeaker.gameObject.SetActive(false);
                break;
        }
    }
}
