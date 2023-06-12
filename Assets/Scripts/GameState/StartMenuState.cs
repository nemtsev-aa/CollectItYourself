using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuState : GameState
{
    [Tooltip(" нопка дл€ старта")]
    [SerializeField] private Button _tabToStartButton;
    [Tooltip("ќкно основного меню")]
    [SerializeField] private GameObject _startMenuObject;

    public override void Init(GameStateManager gameStateManager)
    {
        base.Init(gameStateManager);
        _tabToStartButton.onClick.AddListener(gameStateManager.SetAction);
    }

    public override void Enter()
    {
        base.Enter();
        _startMenuObject.SetActive(true);
        Cursor.visible = false;
    }

    public override void Exit()
    {
        base.Exit();
        _startMenuObject.SetActive(false);
        Cursor.visible = true;
    }
}
