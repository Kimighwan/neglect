using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<GameObject> rooms = new List<GameObject>(4);
    public bool active = false;
    public void ActiveRoom() {
        this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        GameInfo.gameInfo.Rooms++;
    }
}
