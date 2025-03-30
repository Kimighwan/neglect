using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmUIVerson2 : ConfirmUI
{
    public GameObject OnCheckBox;
    public GameObject OffCheckBox;

    public void OnClickCheckOffBox()
    {
        PlayerPrefs.SetInt("ReSelectConfirm", 1);
        OnCheckBox.SetActive(true);
        OffCheckBox.SetActive(false);
    }

    public void OnClickCheckOnBox()
    {
        PlayerPrefs.SetInt("ReSelectConfirm", 0);
        OnCheckBox.SetActive(false);
        OffCheckBox.SetActive(true);
    }
}
