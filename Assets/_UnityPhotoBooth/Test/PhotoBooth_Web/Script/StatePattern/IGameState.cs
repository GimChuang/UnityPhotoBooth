using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IGameState {

    private Extension.GameState gameState;
    public Extension.GameState GameState
    {
        get { return gameState; }
        set { gameState = value; }
    }

    protected GameStateController gameStateController = null;

    public float timeToGoBackToMainMenu;
    protected float timer_goBackToMainMenu;

    // Constructor
    public IGameState(GameStateController _controller, float _timeToGoBack)
    {
        gameStateController = _controller;
        timeToGoBackToMainMenu = _timeToGoBack;
    }

    public virtual void StateBegin()
    {

    }

    public virtual void StateEnd()
    {

    }

    public virtual void StateUpdate()
    {

    }
}
