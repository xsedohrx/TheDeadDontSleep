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

    private bool canZombieAttack = true;

    protected override void Awake()
    {
        base.Awake();
        anim = SWAT.GetComponent<Animator>();
    }

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
        if (firearm) firearm.enabled = false;

        var rotator = GetComponent<Rotator>();
        var motor = GetComponent<PlayerMotor>();
        rotator.enabled = false;
        motor.enabled = false;

        //animation
        anim.SetTrigger("dead");

        yield return new WaitForSeconds(3.5f);

        SWAT.SetActive(false);
        zombie.SetActive(true);
        gameObject.tag = "Zombie";
        anim = zombie.GetComponent<Animator>();
        humanState = HumanState.ZOMBIE;
        anim.SetTrigger("revive");
        yield return new WaitForSeconds(3f);

        rotator.enabled = true;
        motor.enabled = true;
        health = zombieHealth;

        //change to final phase and spawn some zombies..
        GameManager.instance.ZombieSpawnPhase = false;
    }

    Transform GetClosestZombie(Zombie[] zombies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        for (int i = 0; i < zombies.Length; i++)
        {
            if (!zombies[i].gameObject.activeSelf) continue;

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
        yield return new WaitForSeconds(.1f);
        Zombie[] zombies = FindObjectsOfType<Zombie>();

        Transform closestZombie = GetClosestZombie(zombies);

        if (closestZombie == null)
        {
            //GAME OVER ? TODO
            GameObject.Destroy(gameObject, 0.1f);
        }
        else
        {

            transform.position = closestZombie.transform.position;
            transform.rotation = closestZombie.transform.rotation;

            health = zombieHealth;

            closestZombie.gameObject.SetActive(false);
        }
    }

    public void Attack()
    {
        if (humanState == HumanState.ALIVE)
        {
            //do we have a gun.. 
            FireArm gun = GetComponent<FireArm>();
            if (gun && gun.enabled && gun.canFire)
            {
                anim.SetTrigger("attack");
                gun.Fire();
            }
        }
        else if(canZombieAttack)
        {
            //attacking as a zombie..
            anim.SetTrigger("attack");
            //anything in front of us ?
            RaycastHit hit;
            // Does the ray intersect any walls
            if (Physics.Raycast(transform.position + Vector3.up * 1.5f, transform.forward, out hit, 1.4f))
            {
                //hit something..
                if (hit.transform.gameObject.tag == "Zombie" || hit.transform.gameObject.tag == "Human" || hit.transform.gameObject.tag == "Soldier")
                {
                    NPC npc = hit.transform.gameObject.GetComponent<NPC>();
                    npc?.TakeDamage(10); //player does more damage on hit
                }

            }
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        canZombieAttack = false;

        yield return new WaitForSeconds(1f);

        canZombieAttack = true;
    }

}
