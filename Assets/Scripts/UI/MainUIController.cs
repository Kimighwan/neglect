using UnityEngine;

public class MainUIController : MonoBehaviour
{
    void Start()
    {
        Fade.Instance.DoFade(Color.black, 1f, 0f, 1f, 0f, true, false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}