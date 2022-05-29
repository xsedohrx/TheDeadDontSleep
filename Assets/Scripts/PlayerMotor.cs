using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private FieldOfView fov;
    private Vector3 fovOffset = new Vector3(0, 1, 0);
    private Player player;

    private Vector3 targetMovement = Vector3.zero;
    private Vector2 targetMovementAnimDirection = Vector2.zero;
    private Vector3 currentMovement = Vector3.zero;
    private Vector2 currentMovementAnimDirection = Vector2.zero;


    private void Start()
    {
        fov = FindObjectOfType<FieldOfView>();
        if (fov)
        {
            fov.SetOrigin(transform.position + fovOffset);
        }
        player = GetComponentInChildren<Player>();
    }

    [SerializeField] float movementSpeed = 5;
    private void OnEnable()
    {
        PlayerInput.OnKeyPressed += MoveTo;
        PlayerInput.OnNoMovement += Stop;
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

    public void Update()
    {
        if ((currentMovement != Vector3.zero || targetMovement != Vector3.zero) || (currentMovementAnimDirection != Vector2.zero || targetMovementAnimDirection != Vector2.zero))
        {
            //Debug.LogWarning(targetMovement);
            //Debug.LogError(targetMovementAnimDirection);

            currentMovement = Vector3.Lerp(currentMovement, targetMovement, 0.05f);
            currentMovementAnimDirection = Vector2.Lerp(currentMovementAnimDirection, targetMovementAnimDirection, 0.05f);

            //when numbers get small - zero them so not moving very very slightly
            if (currentMovement.magnitude < 0.0005)
            {
                //Debug.LogError("Movement Zero");
                currentMovement = Vector3.zero;
            }
            if (currentMovementAnimDirection.magnitude < 0.02)
            {
                //Debug.LogError("Anim Zero");
                currentMovementAnimDirection = Vector2.zero;
            }

            if (player && player.anim)
            {
                player.anim.SetFloat("velocityZ", currentMovementAnimDirection.x);
                player.anim.SetFloat("velocityX", currentMovementAnimDirection.y);
            }

            transform.position += currentMovement;

            if (fov)
            {
                fov.SetOrigin(transform.position + new Vector3(0, 1, 0));
            }
        }

    }

    private void Stop()
    {
        targetMovement = Vector2.zero;
        targetMovementAnimDirection = Vector2.zero;
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

        targetMovementAnimDirection = new Vector2(Mathf.Cos(angleDifference * Mathf.Deg2Rad), Mathf.Sin(angleDifference * Mathf.Deg2Rad));

        angleDifference = Math.Abs(angleDifference);
        //angleDifference now in the range 0 to 180
        //ok we slow the player down up to 50% if full backwards and 100% if forwards.. 
        var speedDirectionMultiplier = Mathf.Clamp(0.5f + (0.7f * (180 - angleDifference)/180), 0.3f, 1); ;

        targetMovement = new Vector3( direction.x, 0, direction.y )  * movementSpeed * speedDirectionMultiplier * Time.deltaTime;

    }
}
