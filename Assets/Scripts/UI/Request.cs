using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Request : MonoBehaviour
{
    public List<GameObject> requests;
    public List<TextMeshProUGUI> texts;
    public List<GameObject> bars;
    private bool rq1 = true;
    private bool rq2 = true;
    private bool rq3 = false;
    private bool rq4 = false;
    private bool rq5 = false;
    private void Start() {
        requests[0].GetComponent<Image>().color = new Color(1f, 0.5f, 0.5f);
        requests[1].GetComponent<Image>().color = new Color(1f, 0.5f, 0.5f);
        texts[0].color = new Color(1f, 1f, 1f);
        texts[1].color = new Color(1f, 1f, 1f);
        bars[0].GetComponent<Image>().color = new Color(1f, 1f, 0f);
        bars[1].GetComponent<Image>().color = new Color(1f, 1f, 0f);
    }
    public void ActiveRequest() {
        if (!rq5) {
            int i = 0;
            if (rq2 && !rq3) {
                rq3 = true;
                i = 2;
            }
            else if (rq3 && !rq4) {
                rq4 = true;
                i = 3;
            }
            else if (rq4 && !rq5) {
                rq5 = true;
                i = 4;
            }
            requests[i].GetComponent<Image>().color = new Color(1f, 0.5f, 0.5f);
            texts[i].color = new Color(1f, 1f, 1f);
            bars[i].GetComponent<Image>().color = new Color(1f, 1f, 0f);
            GameInfo.gameInfo.Requests++;
        }
    }
}
