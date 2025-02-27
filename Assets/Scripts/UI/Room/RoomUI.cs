using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class RoomUI : MonoBehaviour {
    public TextMeshProUGUI title;
    public TextMeshProUGUI level;
    public TextMeshProUGUI other;
    public TextMeshProUGUI neededGold;
    public Button button;

    private int index; 
    private bool isActive;
    public bool isUINow = false;

    void Start()
    {
        button.interactable = true;
        isUINow = true;
    }

    void OnEnable()
    {
        StartCoroutine(ScaleChange(Vector3.zero, Vector3.one, 0.5f));
    }

    public void SetInfo(int i, bool b) {
        index = i;
        isActive = b;
        if (!isActive) {
            title.text = "객실 개방";
            level.text = "";
            other.text = "모험가 수 +2\n하루 수익 +300";
            neededGold.text = GameInfo.gameInfo.firstPurchase ?  "1번 무료!!!" : "필요 골드 1000";
        }
        else {
            title.text = "객실 레벨업";
            int l = GameInfo.gameInfo.GetRoomLevel(index);
            if (l == 1) {
                level.text = "1 >> 2";
                other.text = "모험가 수 2 >> 4\n하루 수익 300 >> 1000";
                neededGold.text = "필요 골드 3000";
            }
            else if (l == 2) {
                level.text = "2 >> 3";
                other.text = "모험가 수 4 >> 6\n하루 수익 1000 >> 4000";
                neededGold.text = "필요 골드 10000";
            }
            else if (l == 3) {
                title.text = "";
                level.text = "최고 등급 객실";
                other.text = "";
                neededGold.text = "추가 레벨업X";
                button.interactable = false;
            }
            else OnClickCloseButton();
        }
    }

    public void OnClickActivateRoom() {
        if (GameInfo.gameInfo.RoomActive(index)) {
            if (GameInfo.gameInfo.firstPurchase) GameInfo.gameInfo.firstPurchase = false;
            AudioManager.Instance.PlaySFX(SFX.LevelUp);
            OnClickCloseButton();
        }
    }

    public void OnClickLevelUpButton() {
        if (!isActive) OnClickActivateRoom();
        else {
            if (GameInfo.gameInfo.RoomLevelUp(index)) {
                AudioManager.Instance.PlaySFX(SFX.LevelUp);
                OnClickCloseButton();
            }
        }
    }

    public void OnClickCloseButton()
    {
        title.text = "";
        level.text = "";
        other.text = "";
        neededGold.text = "";
        isUINow = false;
        this.gameObject.SetActive(false);
    }

    IEnumerator ScaleChange(Vector3 originalScale ,Vector3 targetScale, float animationDuration) {
        float elapsedTime = 0f;
        
        transform.localScale = originalScale;
        
        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            float smoothStep = Mathf.SmoothStep(0f, 1f, t);
            transform.localScale = Vector3.Lerp(originalScale, targetScale, smoothStep);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.localScale = targetScale;
    }
}
