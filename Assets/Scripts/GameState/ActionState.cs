using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionState : GameState
{
    [Tooltip("Окно")]
    [SerializeField] private ActiveWindow _activeWindow;

    [SerializeField] private Management Management;
    [SerializeField] private PrincipalSchemaView _principalSchemaView;
    [SerializeField] private SwitchBoxsSelectorView _switchBoxsSelectorView;

    private void Awake() {
        _switchBoxsSelectorView.ActiveSwitchBoxChanged += SetActiveSwitchBoxNumber;
    }

    public override void EnterFirstTime() {
        base.EnterFirstTime();
    }

    public override void Init(GameStateManager gameStateManager)
    {
        base.Init(gameStateManager);
    }

    public override void Enter()
    {
        base.Enter();
        _activeWindow.Show();
        Management.ShowTask();
    }

    public override void Exit()
    {
        base.Exit();
        _activeWindow.Hide();
    }

    public void ShowSwitchBox(SwitchBox switchBox) {
        _principalSchemaView.Show(switchBox);
    }

    public void SetActiveSwitchBoxNumber(int number) {
        SwitchBox box = Management.SwitchBoxManager.GetSwitchBoxByNumber(number);
        _principalSchemaView.Show(box);
    }

    private void OnDisable() {
        _switchBoxsSelectorView.ActiveSwitchBoxChanged += SetActiveSwitchBoxNumber;
    }
}
