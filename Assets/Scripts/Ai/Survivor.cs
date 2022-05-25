using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor : NPC
{

    #region State Variables
    public enum PanickState { 
        CALM,
        PANICK,
        RUNNING,
        HIDING
    }
    public PanickState panickState;

    #endregion
    #region Unity Functions
    protected override void Start()
    {
        base.Start(); 
        SetPanickState(PanickState.CALM);
    }

    protected override void Update()
    {
        HandlePanickState();
    }

    #endregion
    #region Panick State Switch
    private void HandlePanickState()
    {
        switch (panickState)
        {
            case PanickState.CALM:
                //TODO Implement wander functionality
                break;
            case PanickState.PANICK:
                //TODO Implement panick
                break;
            case PanickState.RUNNING:
                //TODO Implement run from zombies
                break;
            case PanickState.HIDING:
                //TODO Implement hiding mechanic
                break;

        }
    }
    
    void SetPanickState(PanickState state)
    {
        panickState = state;
    }
    #endregion

}
