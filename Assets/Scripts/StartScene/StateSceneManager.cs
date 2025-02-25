using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSceneManager : MonoBehaviour
{

    void Start()
    {
        if (!PlayerPrefs.HasKey("고블린"))
            PlayerPrefs.SetInt("고블린", 0);

        if (!PlayerPrefs.HasKey("슬라임"))
            PlayerPrefs.SetInt("슬라임", 0);

        if (!PlayerPrefs.HasKey("픽시"))
            PlayerPrefs.SetInt("픽시", 0);

        if (!PlayerPrefs.HasKey("정령"))
            PlayerPrefs.SetInt("정령", 0);

        if (!PlayerPrefs.HasKey("오크"))
            PlayerPrefs.SetInt("오크", 0);

        if (!PlayerPrefs.HasKey("언데드"))
            PlayerPrefs.SetInt("언데드", 0);

        if (!PlayerPrefs.HasKey("골렘"))
            PlayerPrefs.SetInt("골렘", 0);

        if (!PlayerPrefs.HasKey("큐피트"))
            PlayerPrefs.SetInt("큐피트", 0);

        if (!PlayerPrefs.HasKey("가고일"))
            PlayerPrefs.SetInt("가고일", 0);

        if (!PlayerPrefs.HasKey("오우거"))
            PlayerPrefs.SetInt("오우거", 0);

        if (!PlayerPrefs.HasKey("악마"))
            PlayerPrefs.SetInt("악마", 0);

        if (!PlayerPrefs.HasKey("천사"))
            PlayerPrefs.SetInt("천사", 0);
    }
}
