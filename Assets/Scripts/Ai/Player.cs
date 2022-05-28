using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Player : NPC
{

    [SerializeField] private GameObject SWAT;
    [SerializeField] private GameObject zombie;

    protected override void OnEnable(){ 
        PlayerInput.OnMouseButtonPressed += Attack;
        base.OnEnable();
    }
    protected override void OnDisable(){ 
        PlayerInput.OnMouseButtonPressed -= Attack;
        base.OnDisable();
    }

    protected override void CheckHealth()
    {
        if (Health <= 0)
        {
            if (humanState == HumanState.ALIVE)
            {
                //change to zombie!
                //probably want to do some effects and stuff here so lets stop them moving for a moment.. and make them "invincible" until
                StartCoroutine(TransformToZombie());
                return;
            }
            else
            {
                StartCoroutine(ChangeToNewZombie());
                return;
            }
        }
        base.CheckHealth();
    }

    IEnumerator TransformToZombie()
    {
        health = 1000; //invincible while we change
        canChange = false;
        FireArm firearm = GetComponent<FireArm>();
        if(firearm) firearm.enabled = false;

        //todo:animation
        yield return new WaitForSeconds(.5f);
        SWAT.SetActive(false);
        zombie.SetActive( true );
        yield return new WaitForSeconds(.5f);
        anim = zombie.GetComponent<Animator>();
        humanState = HumanState.ZOMBIE;
        health = zombieHealth;
    }

    Transform GetClosestZombie(Zombie[] zombies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        for (int i = 0; i < zombies.Length; i++)
        {
            if (!zombies[i].gameObject.activeInHierarchy) continue;

            Vector3 directionToTarget = zombies[i].transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = zombies[i].transform;
            }
        }

        return bestTarget;
    }


    IEnumerator ChangeToNewZombie()
    {
        health = 1000; //invincible while we change
        //animation here - TODO ? maybe not needed
        yield return new WaitForSeconds(.5f);
        Zombie[] zombies = FindObjectsOfType<Zombie>();

        Transform closestZombie = GetClosestZombie(zombies);

        if (closestZombie == null)
        {
            health = 0;
            //GAME OVER ? TODO
        }
        else
        {

            transform.position = closestZombie.transform.position;
            transform.rotation = closestZombie.transform.rotation;

            health = zombieHealth;
            
            Destroy(closestZombie.gameObject);
        }
    }

    public void Attack()
    {
        //do we have a gun.. 
        FireArm gun = GetComponent<FireArm>();
        if(gun)
        {
            gun.Fire();
        }
    }
}
