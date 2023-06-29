using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinState : GameState
{
    [SerializeField] private WinWindow _winWindow;
    
    private void Awake() {
       EventBus.Instance.CorrectChecked += ShowCorrectResult;
        _winWindow.ResultsView.Initialize();
    }

    private void ShowCorrectResult(SwitchingResult switchingResult) {
        GameStateManager.Instance.SetWin();
    }

    public override void Enter()
    {
        base.Enter();
        _winWindow.Show();
    }

    public override void Exit()
    {
        base.Exit();
        _winWindow.Hide();
    }
}
