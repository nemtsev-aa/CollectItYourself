using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionState : GameState
{
    [Tooltip("Окно")]
    [SerializeField] private ActiveWindow _activeWindow;

    [SerializeField] private Management _management;
    [SerializeField] private Stopwatch _stopwatch;
    [SerializeField] private PrincipalSchemaView _principalSchemaView;
    [SerializeField] private SwitchBoxsSelectorView _switchBoxsSelectorView;

    private void Awake() {
        _switchBoxsSelectorView.ActiveSwitchBoxChanged += SetActiveSwitchBoxNumber;
    }

    public override void EnterFirstTime() {
        base.EnterFirstTime();
    }

    public override void Enter()
    {
        base.Enter();
        _activeWindow.Show();
        _stopwatch.SetStatus(true);
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
        //SwitchBox box = _management.SwitchBoxManager.GetSwitchBoxByNumber(number);
        //_principalSchemaView.Show(box);
    }

    private void OnDisable() {
        _switchBoxsSelectorView.ActiveSwitchBoxChanged += SetActiveSwitchBoxNumber;
    }
}
