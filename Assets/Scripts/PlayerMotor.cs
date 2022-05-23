using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] float movementSpeed = 3;
    private void OnEnable()
    {
        PlayerInput.OnKeyPressed += MoveTo;
    }

    private void OnDisable()
    {
        PlayerInput.OnKeyPressed -= MoveTo;    
    }

    public static float Angle(Vector2 vector2)
    {
        vector2.x *= -1;
        if (vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg;
        }
    }

    private void MoveTo(Vector2 direction)
    {
        var angleMoving = Angle(direction);
        var angleLooking = transform.rotation.eulerAngles.z;
        var angleDifference = ((angleMoving - angleLooking) % 360);
        if (angleDifference < 0)
        {
            angleDifference += 360;
        }
        if (angleDifference>160)
        {
            angleDifference -= 360;
        }
        angleDifference = Math.Abs(angleDifference);
        //angleDifference now in the range 0 to 180
        //ok we slow the player down up to 50% if full backwards and 100% if forwards.. 
        var speedDirectionMultiplier = Mathf.Clamp(0.5f + (0.5f * (180 - angleDifference)/180), 0.5f, 1); ;

        transform.position += new Vector3( direction.x, direction.y )  * movementSpeed * speedDirectionMultiplier * Time.deltaTime;

        Debug.LogWarning(angleMoving + " / " + angleLooking + " / " + angleDifference + " / " + speedDirectionMultiplier );
    }
}
