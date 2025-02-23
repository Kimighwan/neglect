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
    
    // 게임 진행 속도 조절
    public float gameSpeed = 1f;
    // 골드 정보
    private int gold;
    public int Gold { get { return gold; } set { gold = value; } }
    private int plusGold;
    // 몇 일차인지
    private int day;
    public int Day { get { return day; } set { day = value; } }
    // 시간 정보
    private float timer;
    public float Timer { get { return timer; } set { timer = value; } }
    // 객실 정보
    private List<Room> rooms = new List<Room> { null, null, null, null };

    // 길드 레벨
    private int level;
    public int Level { get { return level; } set { level = value; } }
    private List<int> neededGold = new List<int> { 500, 2000, 20000, 100000 };
    // 개방된 파견창 개수
    public Request request;
    private int requests;
    public int Requests { get { return requests; } set { requests = value; } }
    public bool malpungseonOnce = false;


    private void Awake() {
        gameInfo = this;
    }
    public void StartGameInfo() {
        gold = 10000;
        day = 1;
        timer = 80.0f;
        level = 1;
        requests = 2;
        plusGold = 300;
        fadeInOut.gameObject.SetActive(true);
    }
    public void UpdateGameInfo() {
        timer += Time.deltaTime * gameSpeed;
        if (timer >= 240f) {
            timer %= 240f;
            day += 1;
            malpungseonOnce = false;
            gold += plusGold;
        }
        else if (timer >= 200f) {
            EndToday();
        }
    }
    public bool ChangeGold(int g) {
        if (gold + g > 0) {
            gold += g;
            return true;
        }
        return false;
    }

    // 레벨 업 Yes 버튼 누름
    public bool OnClickLevelUpYes() {
        if (level < 5) {
            if (level == 1 && ChangeGold(-neededGold[0])) {}
            else if (level == 2 && ChangeGold(-neededGold[1])) {}
            else if (level == 3 && ChangeGold(-neededGold[2])) {}
            else if (level == 4 && ChangeGold(-neededGold[3])) {}
            else return false;
            request.ActiveRequest();
            gameInfo.Level++;
            return true;
        }
        return false;
    }
    public int GetRoomLevel(int i) {
        if (rooms[i] != null) return rooms[i].level;
        return 0;
    }
    public void AllocateRoom(int i, Room r) {
        rooms[i] = r;
    }
    public bool isRoomActivated(int index) {
        return rooms[index].isActive;
    }
    // 객실 개방 버튼 누름
    public bool RoomActive(int index) {
        if (CheckMaxRoomActivated() && ChangeGold(-1000)) {
            rooms[index].isActive = true;
            CalculatePlusGold();
            rooms[index].ActiveRoom();
            return true;
        }
        return false;
    }
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
                return true;
            }
        }
        return false;
    }
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
        return sum;
    }

    public int GetNeededGold() {
        if (level == 5) return 0;
        return neededGold[level - 1];
    }
    public void EndToday()
    {
        GameManager.gameManager.PauseGame();
        UIManager.Instance.CloseAllOpenUI();    // 모든 UI 창 닫기
        fadeInOut.gameObject.SetActive(true);
        StartCoroutine(ComeNight(1.2f, 0f, 0f, 1f));
    }

    private IEnumerator ComeNight(float duration, float startDelay, float startAlpha, float endAlpha)
    {
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
            fadeInOut.color = new Color(0f, 0f, 0f, Mathf.Lerp(startAlpha, endAlpha, (Time.realtimeSinceStartup - startTime) / duration));
            yield return null;
        }
        fadeInOut.color = new Color(0f, 0f, 0f, endAlpha);

        if (endAlpha == 0f) fadeInOut.gameObject.SetActive(false);
    }

    public void ComeMorning()
    {
        GameManager.gameManager.PauseGame();
        timer = 320f;
        StartCoroutine(FadeBlack(1.2f, 0f, 1f, 0f));
    }

    public IEnumerator FadeBlackInOut(float duration, float startDelay) {
        yield return StartCoroutine(FadeBlack(duration, startDelay, 0f, 1f));
        yield return StartCoroutine(FadeBlack(duration, startDelay, 1f, 0f));
    }

    public void PrepareShowIll(float duration, float startDelay, bool start) {
        if (start) {
            StartCoroutine(FadeBlack(duration, startDelay, 1f, 0f));
        }
        else StartCoroutine(FadeBlackInOut(duration, startDelay));
    }
}
