using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController
{
    public IGameState State { get { return state; } }
    private IGameState state;

    private bool runBegin = false;

    // Constructor
    public GameStateController()
    {

    }

    public void SetState(IGameState _newState)
    {
        Debug.Log("SetState: " + _newState.ToString());

        runBegin = false;

        // Load Scene


        // Ask the last gameState to end
        if (state != null)
        {
            state.StateEnd();
        }

        state = _newState;
    }

    public void StateUpdate()
    {
        // if still loading or something
        // return

        // Ask the new gameState to begin
        if (state != null && runBegin == false)
        {
            state.StateBegin();
            runBegin = true;
        }

        if (state != null)
        {
            state.StateUpdate();
        }
    }


}
