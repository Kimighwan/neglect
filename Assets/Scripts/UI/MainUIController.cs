using System.Buffers.Text;
using Unity.VisualScripting;
using UnityEngine;

public class MainUIController : MonoBehaviour
{
    public GameObject encyclopedia;
    public GameObject blackImage;

    public Transform UICanvasTrs;   // Canvas 위치 닫힌 UI
    public Transform OpenUITrs;     // 열린 UI


    private GameObject frontUI;

    void Start()
    {
        Fade.Instance.DoFade(Color.black, 1f, 0f, 1f, 0f, true, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }

    public void OnClickEncyclopediaBtn()
    {
        encyclopedia.SetActive(true);
        OpenUI();
    }

    private void OpenUI()
    {
        SetBlackImage(true);
        transform.SetParent(OpenUITrs);
    }

    private void ClosedUI()
    {
        SetBlackImage(false);
        transform.SetParent(UICanvasTrs);
    }

    private void SetBlackImage(bool active)
    {
        if (active)
        {
            blackImage.SetActive(true);
        }
        else
        {
            blackImage.SetActive(false);
        }
    }
}