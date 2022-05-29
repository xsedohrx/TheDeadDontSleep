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

    protected override void OnEnable()
    {
        base.OnEnable();

        SetPanickState(PanickState.CALM);
    }

    protected override void OnDisable(){ base.OnDisable();}
    protected override void Awake(){ base.Awake();}
    protected override void MoveToDestination(){ base.MoveToDestination();}
    protected override void SetTarget(Transform target){ base.SetTarget(target); }
    protected override void Start(){ base.Start(); }
    protected override void Update(){ HandlePanickState(); }


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
