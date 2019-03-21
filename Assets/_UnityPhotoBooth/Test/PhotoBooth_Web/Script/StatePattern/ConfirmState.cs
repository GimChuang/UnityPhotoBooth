using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmState : IGameState
{
    // Constructor
    public ConfirmState(GameStateController _controller, float _timeToGoBack) : base(_controller, _timeToGoBack)
    {
        GameState = Extension.GameState.ConfirmState;
        timeToGoBackToMainMenu = _timeToGoBack;
    }

    public override void StateBegin()
    {
        // Show ConfirmState UI panel
        Test_PhotoBooth_Web.Instance.uiController.HandleStateBegin(this.GameState);
    }

    public override void StateUpdate()
    {
        if (Input.anyKeyDown)
        {
            timer_goBackToMainMenu = 0f;
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
        // Hide ConfirmState UI panel
        Test_PhotoBooth_Web.Instance.uiController.HandleStateEnd(this.GameState);
    }
}
