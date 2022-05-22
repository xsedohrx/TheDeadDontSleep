using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRayCast : MonoBehaviour
{
    Vector3 target;

    // Update is called once per frame
    void Update()
    {
        DirectionToTarget();

    }

    private void DirectionToTarget()
    {
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(target.x - transform.position.x, target.y - transform.position.y);
        Debug.DrawRay(transform.position, direction, Color.green, .1f);
    }
}
