using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : NPC
{
    bool destinationReached = false;
    Vector3 newDestination;

    public enum IdleState { 
        IDLE,
        PATROLLING,
        SEARCHING,
        CHASING,
        ATTACKING
    }

    public IdleState idleState;

    void SetIdleState(IdleState state) {
        idleState = state;
    }

    private void Start()
    {
        SetIdleState(IdleState.IDLE);
    }

    protected override void Update()
    {
        base.Update();
        HandleIdleState();
    }

    private void HandleIdleState()
    {
        switch (idleState)
        {
            case IdleState.IDLE:
                //TODO Wander idlely
                break;
            case IdleState.PATROLLING:
                //TODO Walk between waypoints
                break;
            case IdleState.SEARCHING:
                //TODO Look for enemies
                break;
            case IdleState.CHASING:
                //TODO Chase target
                break;
            case IdleState.ATTACKING:
                //TODO FFire at target
                break;
        }
    }
}
