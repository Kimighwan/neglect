using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Request : MonoBehaviour
{
    public List<GameObject> requests;
    private List<TextMeshProUGUI> states = new List<TextMeshProUGUI>();
    private List<Button> quests = new List<Button>();
    private List<Button> adventures = new List<Button>();
    private List<Button> questStarts = new List<Button>();
    private List<Button> awakes = new List<Button>();

    private List<bool> isActivated = new List<bool>{ true, true, false, false, false };
    private List<bool> isAllocated = new List<bool>{ false, false, false, false, false };
    private void Start() {
        PopulateLists();
        Initiate();
        SetState();
    }
    private void Update() {
        
    }
    private void Initiate() {
        requests[0].GetComponent<Image>().color = new Color(1f, 1f, 1f);
        requests[1].GetComponent<Image>().color = new Color(1f, 1f, 1f);
    }
    private void PopulateLists()
    {
        foreach (var request in requests)
        {
            if (request == null) continue;

            // 자식 오브젝트 탐색
            foreach (Transform child in request.transform)
            {
                switch (child.name)
                {
                    case "Quest":
                        Button button1 = child.GetComponent<Button>();
                        if (button1 != null) quests.Add(button1);
                        break;
                    
                    case "Adventure":
                        Button button2 = child.GetComponent<Button>();
                        if (button2 != null) adventures.Add(button2);
                        break;

                    case "State":
                        TextMeshProUGUI stateComponent = child.GetComponent<TextMeshProUGUI>();
                        if (stateComponent != null) states.Add(stateComponent);
                        break;

                    case "QuestStart":
                        Button button3 = child.GetComponent<Button>();
                        if (button3 != null) questStarts.Add(button3);
                        break;
                    
                    case "Awake":
                        Button button4 = child.GetComponent<Button>();
                        if (button4 != null) awakes.Add(button4);
                        break;
                }
            }
        }
    }
    public void ActiveRequest() {
        if (!isActivated[4]) {
            int i = 1;
            if (isActivated[i] && !isActivated[i + 1]) isActivated[i++] = true;
            else if (isActivated[++i] && !isActivated[i + 1]) isActivated[i++] = true;
            else if (isActivated[++i] && !isActivated[i + 1]) isActivated[i++] = true;
            else return;
            requests[i].GetComponent<Image>().color = new Color(1f, 1f, 1f);
            quests[i].interactable = true;
            adventures[i].interactable = true;
            awakes[i].interactable = true;
            states[i].text = "";
            GameInfo.gameInfo.Requests++;
        }
    }
    public void SetState() { // 모든 버튼의 상태를 정하는 메소드로 할당이 다 되면 호출, 초기화 시 호출
        for (int i = 0; i < 5; i++) {
            if (!isActivated[i]) {
                quests[i].interactable = false;
                adventures[i].interactable = false;
                questStarts[i].interactable = false;
                awakes[i].interactable = false;
                states[i].text = "길드 레벨 ??에 개방";
            }
            else {
                states[i].text = "";
                if (!isAllocated[i]) {
                    questStarts[i].interactable = false;
                }
                else questStarts[i].interactable = true;
            }
        }
    }
}
