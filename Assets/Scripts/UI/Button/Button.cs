using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    static public event Action OnButtonClicked;

    public void Click()
    {
        OnButtonClicked?.Invoke();
    }
}
