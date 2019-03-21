using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseBGState : IGameState {

    // Constructor
    public ChooseBGState(GameStateController _controller, float _timeToGoBack):base(_controller, _timeToGoBack)
    {
        GameState = Extension.GameState.ChooseBGState;
        timeToGoBackToMainMenu = _timeToGoBack;
    }

    public override void StateBegin()
    {
        // Show ChooseBGState UI panel
        Test_PhotoBooth_Web.Instance.uiController.HandleStateBegin(this.GameState);
    }

    public override void StateUpdate()
    {
        // Choose a background
        // Click btn_nextStep to go to TakePhotoState
    }

    public override void StateEnd()
    {
        // Hide ChooseBGState UI panel
        Test_PhotoBooth_Web.Instance.uiController.HandleStateEnd(this.GameState);
    }
}
