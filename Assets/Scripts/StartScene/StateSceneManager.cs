using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StateSceneManager : MonoBehaviour
{
    public Button GameEnd;
    void Start()
    {
        Debug.Log($"{GetType()} 시작");
        PlayerPrefs.DeleteAll();
        StartCoroutine(PlayTitleBGM());
        GameEnd.interactable = false;

        if (!PlayerPrefs.HasKey("고블린"))
            PlayerPrefs.SetInt("고블린", 0);

        if (!PlayerPrefs.HasKey("슬라임"))
            PlayerPrefs.SetInt("슬라임", 0);

        if (!PlayerPrefs.HasKey("픽시"))
            PlayerPrefs.SetInt("픽시", 0);

        if (!PlayerPrefs.HasKey("정령"))
            PlayerPrefs.SetInt("정령", 0);

        if (!PlayerPrefs.HasKey("오크"))
            PlayerPrefs.SetInt("오크", 0);

        if (!PlayerPrefs.HasKey("언데드"))
            PlayerPrefs.SetInt("언데드", 0);

        if (!PlayerPrefs.HasKey("골렘"))
            PlayerPrefs.SetInt("골렘", 0);

        if (!PlayerPrefs.HasKey("큐피트"))
            PlayerPrefs.SetInt("큐피트", 0);

        if (!PlayerPrefs.HasKey("가고일"))
            PlayerPrefs.SetInt("가고일", 0);

        if (!PlayerPrefs.HasKey("오우거"))
            PlayerPrefs.SetInt("오우거", 0);

        if (!PlayerPrefs.HasKey("악마"))
            PlayerPrefs.SetInt("악마", 0);

        if (!PlayerPrefs.HasKey("천사"))
            PlayerPrefs.SetInt("천사", 0);

        if (!PlayerPrefs.HasKey("보팔래빗"))
            PlayerPrefs.SetInt("보팔래빗", 0);

        if (!PlayerPrefs.HasKey("듀라한"))
            PlayerPrefs.SetInt("듀라한", 0);

        if (!PlayerPrefs.HasKey("고블린주술사"))
            PlayerPrefs.SetInt("고블린주술사", 0);

        if (!PlayerPrefs.HasKey("고블린킹"))
            PlayerPrefs.SetInt("고블린킹", 0);

        if (!PlayerPrefs.HasKey("고블린워리어"))
            PlayerPrefs.SetInt("고블린워리어", 0);

        if (!PlayerPrefs.HasKey("혼래빗"))
            PlayerPrefs.SetInt("혼래빗", 0);

        if (!PlayerPrefs.HasKey("스펙터"))
            PlayerPrefs.SetInt("스펙터", 0);

        if (!PlayerPrefs.HasKey("홉고블린"))
            PlayerPrefs.SetInt("홉고블린", 0);



        if (!PlayerPrefs.HasKey("호문쿨루스"))
            PlayerPrefs.SetInt("호문쿨루스", 0);

        if (!PlayerPrefs.HasKey("설녀"))
            PlayerPrefs.SetInt("설녀", 0);

        if (!PlayerPrefs.HasKey("헤츨링"))
            PlayerPrefs.SetInt("헤츨링", 0);

        if (!PlayerPrefs.HasKey("ReSelectConfirm"))
            PlayerPrefs.SetInt("ReSelectConfirm", 0);

        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) AudioManager.Instance.PlaySFX(SFX.Click1);
    }

    private IEnumerator PlayTitleBGM() {
        yield return new WaitForSeconds(0.1f);
        if (AudioManager.Instance != null) {
            Debug.Log("AudioManager Instance 존재");
            AudioManager.Instance.UnMute();
            AudioManager.Instance.UpdateVolume();
            AudioManager.Instance.PlayBGM(BGM.Start);
        }
        else {
            Debug.Log("AudioManager Instance 없음...");
            StartCoroutine(PlayTitleBGM());
        }
        yield return null;
    }
}
