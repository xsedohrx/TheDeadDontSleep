using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : AutoDestroyPoolableObject
{
    public float speed = 10f;
    public float projectileDamage = 4;

    private void Awake()
    {
    }


    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable() 
    {
        base.OnDisable();
    }

    private void Update()
    {
        //cast a ray - see if it hits something, else move forward
        RaycastHit hit;
        // Does the ray intersect any walls
        if (Physics.Raycast(transform.position, transform.forward, out hit, speed))
        {
            Debug.LogWarning("Hit");
            Debug.LogWarning(hit);
            Debug.LogWarning(hit.transform.gameObject.tag);
            if (hit.transform.gameObject.tag == "Zombie" || hit.transform.gameObject.tag == "Human" || hit.transform.gameObject.tag == "Soldier")
            {
                NPC npc = hit.transform.gameObject.GetComponent<NPC>();
                npc?.TakeDamage(projectileDamage);
                gameObject.SetActive(false);
                return;
            }
            else
            {
                //dont allow bounce off other objects
                gameObject.SetActive(false);
                return;
            }
        }

        transform.position += transform.forward * speed;
    }


}
