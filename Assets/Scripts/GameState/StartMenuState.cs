using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuState : GameState
{
    [Tooltip("���� ��������� ����")]
    [SerializeField] private GameObject _startMenuObject;
    [Tooltip("������ ��� ������")]
    [SerializeField] private Button _tabToStartButton;

    [Tooltip("�������� �������")]
    public TaskManager TaskManager;
    [Tooltip("���������� ������ �������")]
    public TMP_Dropdown TaskSelector;

    private Management _management;
    private GameStateManager _gameStateManager;

    public override void Init(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
        _tabToStartButton.onClick.AddListener(SelectTask);
        _management = _gameStateManager.Management;
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

    public void SelectTask() {
        string TaskName = TaskSelector.options[TaskSelector.value].text;
        Task selectionTask = TaskManager.FindTask(TaskName);
        _management.ShowTask(selectionTask);
        _gameStateManager.SetAction();
    }
}
