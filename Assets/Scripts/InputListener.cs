using System;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    public event Action<KeyCode> OnArrowKeyDown;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) OnArrowKeyDown?.Invoke(KeyCode.UpArrow);
        if (Input.GetKeyDown(KeyCode.DownArrow)) OnArrowKeyDown?.Invoke(KeyCode.DownArrow);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) OnArrowKeyDown?.Invoke(KeyCode.LeftArrow);
        if (Input.GetKeyDown(KeyCode.RightArrow)) OnArrowKeyDown?.Invoke(KeyCode.RightArrow);
    }
}