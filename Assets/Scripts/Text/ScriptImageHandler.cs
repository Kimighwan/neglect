using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptImageHandler : MonoBehaviour
{
    private List<string> preName = new List<string> { "", "", "" };
    public Image BackGround;
    public Image LeftSpeaker;  // UI에서 캐릭터를 표시할 Image 컴포넌트
    public Image MiddleSpeaker;
    public Image RightSpeaker;
    private Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();

    public void SetCharacter(string name, string exp, string inout, string pos)
    {
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
                AudioManager.Instance.ChangeBGMVolume(6f);
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
        AudioManager.Instance.PlayBGM(BGM.Main6);
        AudioManager.Instance.ChangeBGMVolume(10f);
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
            break;
        case "middle":
            ActiveImage(1);
            if (preName[0] == name) UnActiveImage(0);
            if (preName[2] == name) UnActiveImage(2);
            MiddleSpeaker.sprite = sprite;
            preName[1] = name;
            break;
        case "right":
            ActiveImage(2);
            if (preName[0] == name) UnActiveImage(0);
            if (preName[1] == name) UnActiveImage(1);
            RightSpeaker.sprite = sprite;
            preName[2] = name;
            break;
        }
    }
}
