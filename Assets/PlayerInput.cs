using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static event Action<Vector2> OnKeyPressed;

    // Update is called once per frame
    void Update()
    {
        GetUserInput();
    }

    private void GetUserInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            OnKeyPressed?.Invoke(Vector2.up);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            OnKeyPressed?.Invoke(Vector2.left);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            OnKeyPressed?.Invoke(Vector2.down);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            OnKeyPressed?.Invoke(Vector2.right);
        }
    }
}
