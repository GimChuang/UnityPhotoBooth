using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultState : IGameState {

    // Constructor
    public ResultState(GameStateController _controller, float _timeToGoBack) : base(_controller, _timeToGoBack)
    {
        GameState = Extension.GameState.ResultState;
        timeToGoBackToMainMenu = _timeToGoBack;
    }

    public override void StateBegin()
    {
        // Show ResultState UI panel
        Test_PhotoBooth_Web.Instance.uiController.HandleStateBegin(this.GameState);
    }

    public override void StateUpdate()
    {
        // Wait or click anywhere to go back to the first GameState
        if (Input.anyKeyDown)
        {
            timer_goBackToMainMenu = 0f;
            Test_PhotoBooth_Web.Instance.GoToState(Extension.GameState.ChooseBGState);
        }

        timer_goBackToMainMenu += Time.deltaTime;
        if (timer_goBackToMainMenu >= timeToGoBackToMainMenu)
        {
            timer_goBackToMainMenu = 0f;
            Test_PhotoBooth_Web.Instance.GoToState(Extension.GameState.ChooseBGState);
            Debug.LogWarning("No players. Go back to Main Menu.");
        }
    }

    public override void StateEnd()
    {
        // Hide ResultState UI panel
        Test_PhotoBooth_Web.Instance.uiController.HandleStateEnd(this.GameState);

        // Reset Game
        Test_PhotoBooth_Web.Instance.ResetGame();
    }
}
