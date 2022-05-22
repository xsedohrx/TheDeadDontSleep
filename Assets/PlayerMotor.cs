using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerInput.OnKeyPressed += MoveTo;
    }

    private void OnDisable()
    {
        PlayerInput.OnKeyPressed -= MoveTo;    
    }

    private void MoveTo(Vector2 direction)
    {
        transform.Translate(direction * Time.deltaTime);
    }
}
