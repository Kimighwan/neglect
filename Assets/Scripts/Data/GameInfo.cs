using UnityEngine;

/*
게임 진행에 필요한 모든 정보(데이터 베이스 제외)를 사용하려는 목적
날짜 정보 / 골드 정보 / 시간 정보
*/

public class GameInfo : MonoBehaviour
{
    public static GameInfo gameInfo;
    // 게임 진행 속도 조절
    public float gameSpeed = 1f;
    // 골드 정보
    private int gold;
    public int Gold { get { return gold; } set { gold = value; } }
    // 몇 일차인지
    private int day;
    public int Day { get { return day; } set { day = value; } }
    // 시간 정보
    private float timer;
    public float Timer { get { return timer; } set { timer = value; } }
    // 개방된 객실 개수
    private int rooms;
    public int Rooms { get { return rooms; } set { rooms = value; } }
    private int level;
    public int Level { get { return level; } set { level = value; } }

    private void Awake() {
        gameInfo = this;
    }
    public void StartGameInfo() {
        gold = 10;
        day = 1;
        timer = 8.0f;
        rooms = 1;
        level = 1;
    }
    public void UpdateGameInfo() {
        timer += Time.deltaTime * gameSpeed;
        if (timer >= 24f) {
            timer %= 24f;
            day += 1;
            gold += 100 * rooms;
        }
    }
}
