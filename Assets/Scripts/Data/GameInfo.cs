using System.Collections.Generic;
using UnityEngine;

/*
게임 진행에 필요한 모든 정보(데이터 베이스 제외)를 사용하려는 목적
날짜 정보 / 골드 정보 / 시간 정보 / 레벨 정보
*/

public class GameInfo : MonoBehaviour
{
    public static GameInfo gameInfo;
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


    private void Awake() {
        gameInfo = this;
    }
    public void StartGameInfo() {
        gold = 10;
        day = 1;
        timer = 8.0f;
        rooms = 1;
        level = 1;
        requests = 2;
        plusGold = rooms * 100;
    }
    public void UpdateGameInfo() {
        timer += Time.deltaTime * gameSpeed;
        if (timer >= 24f) {
            timer %= 24f;
            day += 1;
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
    // 레벨 업 버튼 누름
    public void OnClickLevelUp(GameObject roomList) {
        if (level < 5) {
            if (level == 1 && ChangeGold(-neededGold[0])) {}
            else if (level == 2 && ChangeGold(-neededGold[1])) {}
            else if (level == 3 && ChangeGold(-neededGold[2])) {}
            else if (level == 4 && ChangeGold(-neededGold[3])) {}
            else return;
            roomList.GetComponent<Room>().ActiveRoom();
            request.ActiveRequest();
            GameInfo.gameInfo.Level++;
            plusGold = rooms * 100;
            UIManager.Instance.OnClickLevelUp();
        }
    }
    public int GetNeededGold() {
        if (level == 5) return 0;
        return neededGold[level - 1];
    }
}
