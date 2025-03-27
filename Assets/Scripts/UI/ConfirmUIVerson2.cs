using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmUIVerson2 : ConfirmUI
{
    public void OnClickCheckBox()
    {
        PlayerPrefs.SetInt("ReSelectConfirm", 1);
    }
}
