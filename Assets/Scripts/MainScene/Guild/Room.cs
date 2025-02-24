using UnityEngine;
using UnityEngine.EventSystems;

public class Room : MouseDrag
{
    public GameObject locker;
    public int index = 0;
    public int level = 1;
    public bool isActive = false;
    public int Level { get { return level; } set { level = value; } }

    void Start()
    {
        if (index == 0) {
            isActive = true;
            Destroy(locker);
        }
        GameInfo.gameInfo.AllocateRoom(index, this);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        UIManager.Instance.OnClickRoom(index, isActive);
    }
    public void ActiveRoom() {
        this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        Destroy(locker);
        isActive = true;
    }
}
