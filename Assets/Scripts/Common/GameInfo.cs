using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
게임 진행에 필요한 모든 정보(데이터 베이스 제외)를 사용하려는 목적
날짜 정보 / 골드 정보 / 시간 정보 / 레벨 정보
*/

public class GameInfo : MonoBehaviour
{
    public SaveTest saver;

    public static GameInfo gameInfo;
    public Button closeButton;

    // 다음 날로 넘어갔는지 체크
    public bool nextDayQuest = false;
    public bool nextDayAdventure = false;
    public int addGold;

    private int playerScore;
    private int todayScore;
    private int todayGold;
    private int questReroll;
    private int adventureReroll;

    public int QuestReroll { get { return questReroll; } set { } }
    public int AdventureReroll { get { return adventureReroll; } set { } }
    public int PlayerScore { get { return playerScore; } set { } }
    public int TodayScore { get { return todayScore; } set { } }
    public int TodayGold { get { return todayGold; } set { } }

    private void Awake()
    {
        gameInfo = this;
    }
    public void StartGameInfo()
    {
        gold = 2000;
        day = 1;
        addGold = 0;
        level = 1;
        requests = 2;
        plusGold = 0;
        playerScore = 0;
    }
    private void UpdateNewDay() // 아침이 시작됨과 동시에 적용
    {
        day += 1;
        gold += plusGold;
        todayGold = 0;
        questReroll = 0;
        adventureReroll = 0;
        if (!ScriptHandler.scriptHandler.ScriptPlayDay(day)) AudioManager.Instance.PlayBGM(BGM.Main6);
    }

    #region GoldInfo
    private int gold;
    public int Gold { get { return gold; } set { gold = value; } }
    public int plusGold; // 객실로 얻는 골드
    public bool ChangeGold(int g)
    {
        if (gold + g >= 0)
        {
            gold += g;
            return true;
        }
        return false;
    }

    public void CalculateTodayGold(int g)
    {
        todayGold += g;
    }

    #endregion

    #region DayAndTimeInfo
    private int day;
    public int Day { get { return day; } set { day = value; } }

    public void OnClickEndTodayBtn()
    {
        // if (day == 5 || day == 10 || day == 15)
        // {
        //     var noCloseData = new ConfirmUIData();
        //     noCloseData.confirmType = ConfirmType.OK;
        //     noCloseData.descTxt = "오늘은 무슨 일이\n일어날 것만 같다...";
        //     noCloseData.okBtnTxt = "확인";
        //     UIManager.Instance.OpenUI<ConfirmUI>(noCloseData);
        // }

        var uiData = new ConfirmUIData();
        uiData.confirmType = ConfirmType.OK_CANCEL;
        uiData.descTxt = "하루를 끝내시겠습니까?";
        uiData.okBtnTxt = "네";
        uiData.cancelBtnTxt = "취소";
        uiData.onClickOKBtn = () =>
        {
            EndToday();
        };
        UIManager.Instance.OpenUI<ConfirmUI>(uiData);
    }

    private void EndToday() // 하루 끝
    {
        AudioManager.Instance.StopBGM();
        UIManager.Instance.CloseAllOpenUI();    // 모든 UI 창 닫기
        roomUI.OnClickCloseButton();
        closeButton.interactable = false;
        playerScore += CalculateTodayScore();
        Fade.Instance.DoFade(Color.black, 0f, 1f, 1f, 0f, true, () =>
            {
                UIManager.Instance.OnClickEndToday();
                nextDayQuest = true;
                nextDayAdventure = true;
            }
        );
    }

    public void ComeMorning() // 아침이 다시 옴
    {
        UpdateNewDay();
        Fade.Instance.DoFade(Color.black, 1f, 0f, 1f, 0f, true, () =>
            {
                GameManager.gameManager.cameraTransform.position = new Vector3(0f, 0f, -10f);
                closeButton.interactable = true;
            }
        );
    }

    #endregion

    #region RoomInfo

    public RoomUI roomUI;
    private List<Room> rooms = new List<Room> { null, null, null, null };
    public bool firstPurchase = true;

    public void LockRoomClick()
    {
        foreach (Room room in rooms)
        {
            room.ActiveBoxCollider(false);
        }
    }
    public void UnLockRoomClick()
    {
        foreach (Room room in rooms)
        {
            room.ActiveBoxCollider(true);
        }
    }
    public int GetRoomLevel(int i)
    {
        if (rooms[i] != null) return rooms[i].level;
        return 0;
    }
    public void AllocateRoom(int i, Room r)
    { // 룸 오브젝트 배정
        rooms[i] = r;
    }
    public bool isRoomActivated(int index)
    { // 룸 개방 되었는지
        return rooms[index].isActive;
    }
    // 객실 개방 버튼 누름
    public bool RoomActive(int index)
    {
        if (CheckMaxRoomActivated())
        {
            if (firstPurchase || ChangeGold(-1000))
            {
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
    private bool CheckMaxRoomActivated()
    {
        int k = 0;
        switch (level)
        {
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
    public bool RoomLevelUp(int index)
    {
        int l = rooms[index].level;
        if (l < 3)
        {
            if ((l == 1 && ChangeGold(-3000)) || (l == 2 && ChangeGold(-6000)))
            {
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
    private void CalculatePlusGold()
    {
        int sum = 0;
        for (int i = 0; i < 4; i++)
        {
            if (rooms[i].isActive)
            {
                int l = rooms[i].level;
                switch (l)
                {
                    case 1:
                        sum += 500;
                        break;
                    case 2:
                        sum += 1000;
                        break;
                    case 3:
                        sum += 2000;
                        break;
                }
            }
        }
        plusGold = sum;
        addGold += sum;
    }
    // 최대 모험가수 계산해줌


    #endregion

    #region GuildInfo
    private int level;
    public int Level { get { return level; } set { level = value; } }
    private List<int> neededGold = new List<int> { 500, 3000, 10000, 50000 }; // 필요 골드
    public int GetNeededGold()
    {
        if (level == 5) return 0;
        return neededGold[level - 1];
    }
    // 레벨 업 Yes 버튼 누름
    public bool OnClickLevelUpYes()
    {
        if (level < 5)
        {
            if (level == 1 && ChangeGold(-neededGold[0])) { }
            else if (level == 2 && ChangeGold(-neededGold[1])) { }
            else if (level == 3 && ChangeGold(-neededGold[2])) { }
            else if (level == 4 && ChangeGold(-neededGold[3])) { }
            else
            {
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
    public RequestButton requestButton;
    private int requests;
    public int Requests { get { return requests; } set { requests = value; } }
    private List<int> successTiers = new List<int> { 0, 0, 0, 0, 0 };
    public void CheckSuccessTier(string tier)
    {
        switch (tier)
        {
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
    private int CalculateTodayScore()
    {
        int sum = 0;
        for (int i = 0; i < 5; i++)
        {
            sum = successTiers[i] * (i + 1) * 100;
            successTiers[i] = 0;
        }
        todayScore = sum;
        return sum;
    }
    public bool CheckInRequest()
    {
        return requestButton.GetInRequest();
    }

    #endregion

    #region Other

    public bool clickedSkipBut = false;

    public int GetMaxAdventurerCounts()
    {
        int sum = 5;
        for (int i = 0; i < 4; i++)
        {
            if (rooms[i].isActive)
            {
                int k = rooms[i].level;
                sum += k * 2;
            }
        }

        sum += level * 2;
        return sum;
    }

    #endregion

    public void AddQuestReroll()
    {
        questReroll++;
    }

    public void AddAdventureReroll()
    {
        adventureReroll++;
    }
}
