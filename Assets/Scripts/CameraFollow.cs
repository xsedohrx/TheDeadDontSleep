using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Camera mainCam;
    [SerializeField] Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget(target.position);
    }

    private void FollowTarget(Vector3 targetToFollow)
    {
        mainCam.transform.position = new Vector3(targetToFollow.x, mainCam.transform.position.y, targetToFollow.z);
    }
}
