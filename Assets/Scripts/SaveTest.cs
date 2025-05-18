using System;
using TMPro;
using UnityEngine;

public class SaveTest : MonoBehaviour
{
    public TextMeshProUGUI SaveTestText;

    int testInt;
    int A;
    float B;
    int C;

    public void SetInt(int d)
    {
        testInt = d;
        A = testInt;
        B = MathF.Pow(testInt, 2.0f);
        C = (int)B % A;
    }

    public int GetA()
    {
        return A;
    }
    public float GetB()
    {
        return B;
    }
    public int GetC()
    {
        return C;
    }
    public void PrintABC()
    {
        SaveTestText.text = $"{A}, {B}, {C}";
    }
}
