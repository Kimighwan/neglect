using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
게임 진행에 필요한 모든 정보(데이터 베이스 제외)를 사용하려는 목적
날짜 정보 / 골드 정보 / 시간 정보 / 레벨 정보
*/

public class GameInfo : MonoBehaviour
{
    public static GameInfo gameInfo;
    public Image fadeInOut;
    public Image pauseButton;
    public Image fastButton;
    public Button closeButton;
    public List<Sprite> pauseAndGo;
    public List<GameObject> AnimatedObj;

    public float gameSpeed = 1f;
    private bool alarmOnce = false;
    private int playerScore;
    public int PlayerScore { get {return playerScore; } set {} }

    private void Awake() {
        gameInfo = this;
    }
    public void StartGameInfo() {
        gold = 0;
        day = 1;
        timer = 80.0f;
        level = 1;
        requests = 2;
        plusGold = 0;
        playerScore = 0;
        fadeInOut.gameObject.SetActive(true);
    }
    public void UpdateGameInfo() {
        timer += Time.deltaTime * gameSpeed;
        if (timer >= 240f) {
            timer %= 240f;
            day += 1;
            alarmOnce = false;
            gold += plusGold;
        }
        else if (timer >= 200f) {
            EndToday();
        }
        else if (!alarmOnce && timer >= 180f) {
            AudioManager.Instance.PlaySFX(SFX.Alarm);
            alarmOnce = true;
        }
    }

    #region GoldInfo
    private int gold;
    public int Gold { get { return gold; } set { gold = value; } }
    private int plusGold;
    public bool ChangeGold(int g) {
        if (gold + g >= 0) {
            gold += g;
            return true;
        }
        return false;
    }

    #endregion

    #region DayAndTimeInfo
    private int day;
    public int Day { get { return day; } set { day = value; } }
    private float timer;
    public float Timer { get { return timer; } set { timer = value; } }
    public void EndToday() // 하루 끝
    {
        if (!GameManager.gameManager.Pause) GameManager.gameManager.PauseGame();
        AudioManager.Instance.StopBGM();
        ScriptDialogHandler.handler.dialog.KillDialog();
        UIManager.Instance.CloseAllOpenUI();    // 모든 UI 창 닫기
        fadeInOut.gameObject.SetActive(true);
        StartCoroutine(ComeNight(1.2f, 0f, 0f, 1f));
    }

    private IEnumerator ComeNight(float duration, float startDelay, float startAlpha, float endAlpha)
    {

        closeButton.interactable = false;
        pauseButton.gameObject.GetComponent<Button>().interactable = false;
        fastButton.gameObject.GetComponent<Button>().interactable = false;
        playerScore += CalculateTodayScore();
        yield return StartCoroutine(FadeBlack(duration, startDelay, startAlpha, endAlpha)); // FadeIn이 끝날 때까지 대기
        UIManager.Instance.OnClickEndToday();
    }

    public IEnumerator FadeBlack(float duration, float startDelay, float startAlpha, float endAlpha)
    {
        yield return new WaitForSeconds(startDelay);

        fadeInOut.color = new Color(0f, 0f, 0f, startAlpha);
        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - startTime < duration)
        {
            if (clickedSkipBut) break;
            fadeInOut.color = new Color(0f, 0f, 0f, Mathf.Lerp(startAlpha, endAlpha, (Time.realtimeSinceStartup - startTime) / duration));
            yield return null;
        }
        fadeInOut.color = new Color(0f, 0f, 0f, endAlpha);

        if (endAlpha == 0f) fadeInOut.gameObject.SetActive(false);
    }

    public void ComeMorning()
    {
        AudioManager.Instance.PlayBGM(BGM.Main6);
        GameManager.gameManager.cameraTransform.position = new Vector3(0f, 0f, -10f);
        GameManager.gameManager.PauseGame();
        timer = 320f;
        StartCoroutine(FadeBlack(1.2f, 0f, 1f, 0f));
        Invoke("ActiveDayButtons", 1.2f);
    }
    private void ActiveDayButtons() {
        closeButton.interactable = true;
        pauseButton.gameObject.GetComponent<Button>().interactable = true;
        fastButton.gameObject.GetComponent<Button>().interactable = true;
    }

    public IEnumerator FadeBlackInOut(float duration, float startDelay) {
        yield return StartCoroutine(FadeBlack(duration, startDelay, 0f, 1f));
        yield return StartCoroutine(FadeBlack(duration, startDelay, 1f, 0f));
    }

    #endregion

    #region RoomInfo
    private List<Room> rooms = new List<Room> { null, null, null, null };
    public bool roomTutorial = false;
    public int GetRoomLevel(int i) {
        if (rooms[i] != null) return rooms[i].level;
        return 0;
    }
    public void AllocateRoom(int i, Room r) { // 룸 오브젝트 배정
        rooms[i] = r;
    }
    public bool isRoomActivated(int index) { // 룸 개방 되었는지
        return rooms[index].isActive;
    }
    // 객실 개방 버튼 누름
    public bool RoomActive(int index) {
        if (CheckMaxRoomActivated()) {
            if (ChangeGold(-1000)) {
                rooms[index].isActive = true;
                CalculatePlusGold();
                rooms[index].ActiveRoom();
                UIManager.Instance.OpenSimpleInfoUI("객실 개방!");
                return true;
            }
            AudioManager.Instance.PlaySFX(SFX.Denied);
            UIManager.Instance.OpenSimpleInfoUI("골드가\n부족합니다!");
            return false;
        }
        AudioManager.Instance.PlaySFX(SFX.Denied);
        UIManager.Instance.OpenSimpleInfoUI("객실 개방\n한도 초과!");
        return false;
    }
    // 최대 개방 가능 객실 수 알려줌
    private bool CheckMaxRoomActivated() {
        int k = 0;
        switch (level) {
            case 1:
                for (int i = 0; i < 4; i++) if (rooms[i].isActive) k++;
                if (k >= 1) return false;
                break;
            case 2:
                for (int i = 0; i < 4; i++) if (rooms[i].isActive) k++;
                if (k >= 2) return false;
                break;
            case 3:
                for (int i = 0; i < 4; i++) if (rooms[i].isActive) k++;
                if (k >= 3) return false;
                break;
            case 4:
            case 5:
                for (int i = 0; i < 4; i++) if (rooms[i].isActive) k++;
                if (k >= 4) return false;
                break;
        }
        return true;
    }
    // 객실 레벨 업 버튼 누름
    public bool RoomLevelUp(int index) {
        int l = rooms[index].level;
        if (l < 3) {
            if ((l == 1 && ChangeGold(-3000)) || (l == 2 && ChangeGold(-10000))) {
                rooms[index].level++;
                CalculatePlusGold();
                UIManager.Instance.OpenSimpleInfoUI("객실 레벨업!");
                return true;
            }
            AudioManager.Instance.PlaySFX(SFX.Denied);
            UIManager.Instance.OpenSimpleInfoUI("골드가\n부족합니다!");
            return false;
        }
        AudioManager.Instance.PlaySFX(SFX.Denied);
        UIManager.Instance.OpenSimpleInfoUI("최고 레벨\n객실입니다!");
        return false;
    }
    // 룸에서 얻는 골드 다시 계산해줌
    private void CalculatePlusGold() {
        int sum = 0;
        for (int i = 0; i < 4; i++) {
            if (rooms[i].isActive) {
                int l = rooms[i].level;
                switch (l) {
                    case 1:
                        sum += 300;
                        break;
                    case 2:
                        sum += 1000;
                        break;
                    case 3:
                        sum += 4000;
                        break;
                }
            }
        }
        plusGold = sum;
    }
    // 최대 모험가수 계산해줌
    

    #endregion

    #region GuildInfo
    private int level;
    public int Level { get { return level; } set { level = value; } }
    private List<int> neededGold = new List<int> { 500, 2000, 20000, 100000 }; // 필요 골드
    public int GetNeededGold() {
        if (level == 5) return 0;
        return neededGold[level - 1];
    }
    // 레벨 업 Yes 버튼 누름
    public bool OnClickLevelUpYes() {
        if (level < 5) {
            if (level == 1 && ChangeGold(-neededGold[0])) {}
            else if (level == 2 && ChangeGold(-neededGold[1])) {}
            else if (level == 3 && ChangeGold(-neededGold[2])) {}
            else if (level == 4 && ChangeGold(-neededGold[3])) {}
            else {
                AudioManager.Instance.PlaySFX(SFX.Denied);
                UIManager.Instance.OpenSimpleInfoUI("골드가 부족합니다!");
                return false;
            }
            request.ActiveRequest();
            gameInfo.Level++;
            return true;
        }
        AudioManager.Instance.PlaySFX(SFX.Denied);
        UIManager.Instance.OpenSimpleInfoUI("최고 레벨 입니다!");
        return false;
    }

    #endregion

    #region RequestInfo
    public Request request;
    private int requests;
    public int Requests { get { return requests; } set { requests = value; } }
    private List<int> successTiers = new List<int> { 0, 0, 0, 0, 0 };
    public void CheckSuccessTier(string tier) {
        switch (tier) {
            case "브론즈":
                successTiers[0]++;
                break;
            case "실버":
                successTiers[1]++;
                break;
            case "골드":
                successTiers[2]++;
                break;
            case "플래티넘":
                successTiers[3]++;
                break;
            case "다이아":
                successTiers[4]++;
                break;
        }
    }
    private int CalculateTodayScore() {
        int sum = 0;
        for (int i = 0; i < 5; i++) {
            sum = successTiers[i] * (i + 1) * 100;
            successTiers[i] = 0;
        }
        return sum;
    }

    #endregion

    #region Other

    public bool clickedSkipBut = false;
    public void PrepareShowIll(float duration, float startDelay, bool start) {
        if (start) {
            StartCoroutine(FadeBlack(duration, startDelay, 1f, 0f));
        }
        else StartCoroutine(FadeBlackInOut(duration, startDelay));
    }
    public void ChangeAniObjSpeed(float f) {
        foreach (GameObject obj in AnimatedObj) {
            if (obj.GetComponent<Animator>() != null) obj.GetComponent<Animator>().speed = f;
        }
    }

    public int GetMaxAdventurerCounts() {
        int sum = 0;
        for (int i = 0; i < 4; i++) {
            if (rooms[i].isActive) {
                int l = rooms[i].level;
                switch (l) {
                    case 1:
                        sum += 2;
                        break;
                    case 2:
                        sum += 4;
                        break;
                    case 3:
                        sum += 6;
                        break;
                }
            }
        }
        sum += level * 5;
        return sum;
    }

    #endregion
}
