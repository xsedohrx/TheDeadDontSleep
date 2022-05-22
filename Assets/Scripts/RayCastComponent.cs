using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastComponent : MonoBehaviour
{
    [SerializeField] Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DirectionToTarget();
    }

    private void DirectionToTarget()
    {
        Vector2 direction = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);
        Debug.DrawRay(transform.position, direction, Color.red, .1f);
    }
}
