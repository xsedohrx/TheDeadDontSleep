using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private FieldOfView fov;
    private Vector3 fovOffset = new Vector3(0, 1, 0);

    private void Start()
    {
        fov = FindObjectOfType<FieldOfView>();
        if (fov)
        {
            fov.SetOrigin(transform.position + fovOffset);
        }
    }

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
        var angleLooking = transform.rotation.eulerAngles.y;
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
        var speedDirectionMultiplier = Mathf.Clamp(0.5f + (0.7f * (180 - angleDifference)/180), 0.3f, 1); ;

        transform.position += new Vector3( direction.x, 0, direction.y )  * movementSpeed * speedDirectionMultiplier * Time.deltaTime;

        if (fov)
        {
            fov.SetOrigin(transform.position + new Vector3(0,1,0));
        }
    }
}
