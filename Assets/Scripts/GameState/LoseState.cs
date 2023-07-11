using CustomEventBus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseState : GameState
{
    [SerializeField] private LoseWindow _loseWindow;
    private EventBus _eventBus;
    private void Awake() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    private void ShowIncorrectResult(GeneralSwitchingResult switchingResult) {
        GameStateManager.Instance.SetLose();
    }

    public override void Enter()
    {
        base.Enter();
        Time.timeScale = 0f;
        _loseWindow.Show();
    }
}
