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
        var direction = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane)) - transform.position;

        //Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle-90, Vector3.down);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        if (fov)
        {
            fov.SetAimDirection(90-transform.rotation.eulerAngles.y);
        }
    }
}
