using System.Collections.Generic;
using UnityEngine;

public class Request : MonoBehaviour
{
    public List<GameObject> requests;
    private List<bool> isActivated = new List<bool>{ true, true, false, false, false };
    public List<GameObject> Blocks;
    

    public void ActiveRequest() {
        if (!isActivated[4]) {
            for (int i = 2; i < 5; i++) {
                if (!isActivated[i]) {
                    isActivated[i] = true;
                    Blocks[i - 2].SetActive(false);
                    GameInfo.gameInfo.Requests++;
                    break;
                }
            }
        }
    }
    public void ClearAdTutorial() {
        foreach (GameObject obj in requests) {
            obj.GetComponent<Test>().SetAdTutorialOnceTrue();
        }
    }
}
