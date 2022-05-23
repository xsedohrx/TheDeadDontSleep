using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : NPC
{
    public enum IdleState { 
        IDLE,
        WANDERING,
        SEARCHING,
        CHASING,
        ATTACKING
    }

    public IdleState idleState;

}
