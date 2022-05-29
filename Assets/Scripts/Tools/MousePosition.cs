using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    Vector3 mousePosition;
    Vector3 GetMousePosition(){ return mousePosition = Input.mousePosition; }
    DrawGizmo mouseGizmo;
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
        mouseGizmo = GetComponent<DrawGizmo>();

    }

    private void Update()
    {
        OnRaycastHit();

    }

    private void OnRaycastHit()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(GetMousePosition());

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            SetGizmoPosition(hit);
            DrawRay(Camera.main.transform.position, Camera.main.ScreenPointToRay(mouseGizmo.transform.position).direction);

        }
    }

    private void SetGizmoPosition(RaycastHit hit)
    {
        mouseGizmo.SetGizmoPosition(hit.point);
    }

    void DrawRay(Vector3 position, Vector3 direction) {
        Debug.DrawRay(position, direction);
    }



    

}
