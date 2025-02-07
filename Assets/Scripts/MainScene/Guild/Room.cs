using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<GameObject> rooms = new List<GameObject>(4);
    private bool room1 = true;
    private bool room2 = false;
    private bool room3 = false;
    private bool room4 = false;
    private void Start() {
        rooms[0].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
    }
    public void ActiveRoom() {
        if (!room4) {
            if (room1 && !room2) rooms[1].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
            if (room2 && !room3) rooms[2].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
            if (room3 && !room4) rooms[3].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
            GameInfo.gameInfo.AddRoom();
        }
    }
}
