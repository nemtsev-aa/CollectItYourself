using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseState : GameState
{
    [SerializeField] private LoseWindow _loseWindow;
    
    private void Awake() {
        EventBus.Instance.CorrectChecked += ShowIncorrectResult;
    }

    private void ShowIncorrectResult(SwitchingResult switchingResult) {
        GameStateManager.Instance.SetLose();
    }

    public override void Enter()
    {
        base.Enter();
        Time.timeScale = 0f;
        _loseWindow.Show();
    }
}
