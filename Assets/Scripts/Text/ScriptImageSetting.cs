using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptImageSetting : MonoBehaviour
{
    private List<string> preName = new List<string> { "", "", "" };
    public Image BackGround;
    public Image LeftSpeaker;  // UI에서 캐릭터를 표시할 Image 컴포넌트
    public Image MiddleSpeaker;
    public Image RightSpeaker;
    private Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();

    public void SetCharacter(string name, string exp, string pos)
    {
        LeftSpeaker.color = new Color(1f, 1f, 1f);
        MiddleSpeaker.color = new Color(1f, 1f, 1f);
        RightSpeaker.color = new Color(1f, 1f, 1f);
        LeftSpeaker.rectTransform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        MiddleSpeaker.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        RightSpeaker.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        string fileName = name + '_' + exp;
        // 캐시된 이미지가 있는지 확인
        if (!spriteCache.TryGetValue(fileName, out Sprite sprite))
        { // 캐시에 없으면 Resources 폴더에서 로드
            string path = $"Arts/Characters/{name}/{fileName}";
            sprite = Resources.Load<Sprite>(path);
            if (sprite != null) spriteCache[name] = sprite;
            else {
                Debug.LogWarning($"[SetCharacter] {path} 경로에서 스프라이트를 찾을 수 없음!");
                return;
            }
        }
        ReplaceCharacter(name, sprite, pos);
    }
    public void SetIllImage(string fileName) {
        if (fileName == "0") {
            AudioManager.Instance.StopBGM();
            BackGround.sprite = null;
            BackGround.color = new Color(0f, 0f, 0f);
            return;
        }
        else BackGround.color = new Color(1f, 1f, 1f);
        if (!spriteCache.TryGetValue(fileName, out Sprite sprite))
        { // 캐시에 없으면 Resources 폴더에서 로드
            string path = $"Arts/Illustration/{fileName}";
            sprite = Resources.Load<Sprite>(path);
            if (sprite != null) spriteCache[fileName] = sprite;
            else {
                Debug.LogWarning($"[SetCharacter] {path} 경로에서 스프라이트를 찾을 수 없음!");
                return;
            }
        }
        switch (fileName) {
            case "0_1":
                AudioManager.Instance.PlayBGM(BGM.ScriptIntro);
                break;
            case "0_2":
                AudioManager.Instance.PlayBGM(BGM.Script0_2);
                break;
        }
        BackGround.sprite = sprite;
    }
    public void EndTheScripts() {
        preName = new List<string> { "", "", "" };
        LeftSpeaker.sprite = null;
        MiddleSpeaker.sprite = null;
        RightSpeaker.sprite = null;
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

    private void ReplaceCharacter(string name, Sprite sprite, string pos) {
        switch (pos) {
        case "left":
            ActiveImage(0);
            if (preName[1] == name) UnActiveImage(1);
            if (preName[2] == name) UnActiveImage(2);
            LeftSpeaker.sprite = sprite;
            preName[0] = name;
            SelectSpeaker(0);
            break;
        case "middle":
            ActiveImage(1);
            if (preName[0] == name) UnActiveImage(0);
            if (preName[2] == name) UnActiveImage(2);
            MiddleSpeaker.sprite = sprite;
            preName[1] = name;
            SelectSpeaker(1);
            break;
        case "right":
            ActiveImage(2);
            if (preName[0] == name) UnActiveImage(0);
            if (preName[1] == name) UnActiveImage(1);
            RightSpeaker.sprite = sprite;
            preName[2] = name;
            SelectSpeaker(2);
            break;
        }
    }

    private void SelectSpeaker(int i) {
        switch (i) {
            case 0:
                LeftSpeaker.color = new Color(1f, 1f, 1f);
                LeftSpeaker.rectTransform.localScale = new Vector3(-1.2f, 1.2f, 1.2f);
                if (MiddleSpeaker.sprite != null) {
                    MiddleSpeaker.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    MiddleSpeaker.color = new Color(0.75f, 0.75f, 0.75f);
                }
                if (RightSpeaker.sprite != null) {
                    RightSpeaker.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    RightSpeaker.color = new Color(0.75f, 0.75f, 0.75f);
                }
                break;
            case 1:
                MiddleSpeaker.color = new Color(1f, 1f, 1f);
                MiddleSpeaker.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                if (LeftSpeaker.sprite != null) {
                    LeftSpeaker.rectTransform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    LeftSpeaker.color = new Color(0.75f, 0.75f, 0.75f);
                }
                if (RightSpeaker.sprite != null) {
                    RightSpeaker.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    RightSpeaker.color = new Color(0.75f, 0.75f, 0.75f);
                }
                break;
            case 2:
                RightSpeaker.color = new Color(1f, 1f, 1f);
                RightSpeaker.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                if (LeftSpeaker.sprite != null) {
                    LeftSpeaker.rectTransform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    LeftSpeaker.color = new Color(0.75f, 0.75f, 0.75f);
                }
                if (MiddleSpeaker.sprite != null) {
                    MiddleSpeaker.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    MiddleSpeaker.color = new Color(0.75f, 0.75f, 0.75f);
                }
                break;
        }
    }

    public void OutSpeaker(int i) {
        UnActiveImage(i);
    }
}
