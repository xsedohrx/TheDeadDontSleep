using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor : NPC
{
    public enum PanickState { 
        PANICK,
        RUNNING,
        HIDING
    }
    public PanickState panickState;


}
