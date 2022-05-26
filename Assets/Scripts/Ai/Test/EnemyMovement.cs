using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    Transform target;
    public float updateSpeed = .1f;

    NavMeshAgent agent;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(FollowTarget());
    }

    IEnumerator FollowTarget() {

        WaitForSeconds wait = new WaitForSeconds(updateSpeed);

        while (enabled)
        {
            agent.SetDestination(target.position);
            yield return wait;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
