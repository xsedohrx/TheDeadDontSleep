using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private Camera cam;
    private FieldOfView fov;

    public float rotationSpeed = 5f;

    private void Start()
    {
        cam = Camera.main;
        fov = FindObjectOfType<FieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateObject();
    }

    void RotateObject() {
        Vector3 mousePos = Input.mousePosition;
        Ray mouseCast = Camera.main.ScreenPointToRay(mousePos);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        RaycastHit hit;
        float rayLength;
        if (Physics.Raycast(mouseCast, out hit, 100))
        {
            Vector3 targetPos = new Vector3(hit.point.x, 0f, hit.point.z);
            Debug.DrawLine(mousePos, targetPos, Color.blue);

            // We need some distance margin with our movement
            // or else the character could twitch back and forth with slight movement
            if (Vector3.Distance(targetPos, transform.position) >= 0.5f)
            {
                Quaternion lookOnLook = Quaternion.LookRotation(targetPos - transform.position);

                //transform.LookAt(targetPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookOnLook, rotationSpeed * Time.deltaTime);
            }
        }
        /*

        var direction = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane)) - transform.position;

        //Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle-90, Vector3.down);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        */
        if (fov)
        {
            fov.SetAimDirection(90 - transform.rotation.eulerAngles.y);
        }
    }
}
