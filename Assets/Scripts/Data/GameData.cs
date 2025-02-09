
[System.Serializable]
public class GameData {
    public int gold;  // 보유 금화
    //public List<AdventurerData> adventurers;  // 고용한 모험가 리스트
    //public List<QuestData> activeQuests;  // 진행 중인 의뢰 리스트

    public GameData() {
        gold = 1000;
        //adventurers = new List<AdventurerData>();
        //activeQuests = new List<QuestData>();
    }
}