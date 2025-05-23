using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject roomUI;
    public GameObject whiteLine;
    public GameObject locker;
    public int index = 0;
    public int level = 1;
    public bool isActive = false;
    public int Level { get { return level; } set { level = value; } }
    void Start()
    {
        GameInfo.gameInfo.AllocateRoom(index, this);
    }
    public void ActiveRoom() {
        this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        Destroy(locker);
        isActive = true;
    }
    void OnMouseOver()
    {
        if (UIManager.Instance.ExistOpenUI() || GameInfo.gameInfo.CheckInRequest()) return;
        whiteLine.SetActive(true);
    }
    void OnMouseDown() {
        if (PoolManager.Instance.isNotTouch) return;
        if (PoolManager.Instance.isNotTouchUI) return;
        if (PoolManager.Instance.isNotTutorialTouch) return;

        // if (!GameInfo.gameInfo.roomTutorial) {
        //     GameInfo.gameInfo.roomTutorial = true;
        //     GameManager.gameManager.OpenTutorial(590007);
        // }
        if (!roomUI.GetComponent<RoomUI>().isUINow) {
            roomUI.gameObject.SetActive(true);
            roomUI.GetComponent<RoomUI>().SetInfo(index, isActive);
        }
    }
    void OnMouseExit()
    {
        whiteLine.SetActive(false);
    }

    public void ActiveBoxCollider(bool b) {
        this.GetComponent<BoxCollider2D>().enabled = b;
    }
}
