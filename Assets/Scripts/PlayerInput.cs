using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static event Action<Vector2> OnKeyPressed;
    public static event Action<Vector2> OnMouseButtonPressed;
    public static Vector3 mousePos;
    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
        GetMouseInput();
    }

    private void GetMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            OnMouseButtonPressed?.Invoke(mousePos);
        }
    }

    private void GetMovementInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) ||
            (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) ||
            (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) ||
            (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            )
        {
            OnKeyPressed?.Invoke(new Vector2(horizontalInput, verticalInput).normalized);
        }
    }
}
