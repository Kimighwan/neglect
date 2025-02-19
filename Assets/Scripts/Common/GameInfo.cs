using System.Collections.Generic;
using UnityEngine;

/*
게임 진행에 필요한 모든 정보(데이터 베이스 제외)를 사용하려는 목적
날짜 정보 / 골드 정보 / 시간 정보 / 레벨 정보
*/

public class GameInfo : MonoBehaviour
{
    public static GameInfo gameInfo;
    public GameObject roomList;
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
    public void EndToday() {
        if (gameInfo)
        GameManager.gameManager.PauseGame();

    }
}
