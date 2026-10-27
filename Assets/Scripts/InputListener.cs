using System;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    public event Action<KeyCode> OnArrowKeyDown;
    [SerializeField] private float coolDownTime = 0.2f;
    private float _lastClickTime = 0;


    private void Update()
    {
        if (Time.time - _lastClickTime < coolDownTime)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnArrowKeyDown?.Invoke(KeyCode.UpArrow);
            _lastClickTime = Time.time;
            return;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnArrowKeyDown?.Invoke(KeyCode.DownArrow);
            _lastClickTime = Time.time;
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnArrowKeyDown?.Invoke(KeyCode.LeftArrow);
            _lastClickTime = Time.time;
            return;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnArrowKeyDown?.Invoke(KeyCode.RightArrow);
            _lastClickTime = Time.time;
            return;
        }
    }
}