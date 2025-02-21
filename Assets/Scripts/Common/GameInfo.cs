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
    public GameObject roomList;
    public Image fadeInOut;
    
    // 게임 진행 속도 조절
    public float gameSpeed = 1f;
    // 골드 정보
    private int gold;
    public int Gold { get { return gold; } set { gold = value; } }
    public int plusGold;
    // 몇 일차인지
    private int day;
    public int Day { get { return day; } set { day = value; } }
    // 시간 정보
    private float timer;
    public float Timer { get { return timer; } set { timer = value; } }
    // 개방된 객실 개수
    private int rooms;
    public int Rooms { get { return rooms; } set { rooms = value; } }
    public List<int> roomCounts = new List<int> { 1, 0, 0, 0 };
    public List<int> roomLevels = new List<int> { 1, 1, 1, 1 };
    // 길드 레벨
    private int level;
    public int Level { get { return level; } set { level = value; } }
    private List<int> neededGold = new List<int> { 200, 500, 1500, 5000 };
    // 개방된 파견창 개수
    public Request request;
    private int requests;
    public int Requests { get { return requests; } set { requests = value; } }
    public bool malpungseonOnce = false;


    private void Awake() {
        gameInfo = this;
    }
    public void StartGameInfo() {
        gold = 10;
        day = 1;
        timer = 80.0f;
        rooms = 1;
        level = 1;
        requests = 2;
        plusGold = rooms * 100;
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
            roomList.GetComponent<Room>().ActiveRoom();
            request.ActiveRequest();
            gameInfo.Level++;
            plusGold = rooms * 100;
            return true;
        }
        return false;
    }
    public int GetNeededGold() {
        if (level == 5) return 0;
        return neededGold[level - 1];
    }
    public void EndToday()
    {
        GameManager.gameManager.PauseGame();
        fadeInOut.gameObject.SetActive(true);
        StartCoroutine(DoFadeInBlack(2f, 0f, 0f, 1f));
    }

    private IEnumerator DoFadeInBlack(float duration, float startDelay, float startAlpha, float endAlpha)
    {
        yield return StartCoroutine(FadeBlack(duration, startDelay, startAlpha, endAlpha)); // FadeIn이 끝날 때까지 대기
        UIManager.Instance.OnClickEndToday();
    }

    private IEnumerator FadeBlack(float duration, float startDelay, float startAlpha, float endAlpha)
    {
        yield return new WaitForSeconds(startDelay);    // Delay...
        fadeInOut.color = new Color(0f, 0f, 0f, startAlpha);

        var startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < duration)
        {
            fadeInOut.color = new Color(0f, 0f, 0f, Mathf.Lerp(startAlpha, endAlpha, (Time.realtimeSinceStartup - startTime) / duration));
            yield return null;
        }

        fadeInOut.color = new Color(0f, 0f, 0f, endAlpha);
    }
    public void ComeMorning()
    {
        GameManager.gameManager.PauseGame();
        timer = 320f;
        StartCoroutine(FadeBlack(2f, 0f, 1f, 0f));
        Invoke("UnActiveFade", 2f);
    }
    private void UnActiveFade() {
        fadeInOut.gameObject.SetActive(false);
    }
}
