using UnityEngine;
using UnityEngine.UI;

public class PauseState : GameState
{
    [Tooltip("Кнопка для продолжения")]
    [SerializeField] private Button _resumeButton;
    [Tooltip("Окно паузы")]
    [SerializeField] private PauseWindow _pauseWindow;

    private GameStateManager _gameStateManager;
    public override void Init(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
        _resumeButton.onClick.AddListener(_gameStateManager.SetAction); 
    }

    public override void Enter()
    {
        base.Enter();
        Time.timeScale = 0f;
        _pauseWindow.Show();
    }

    public override void Exit()
    {
        base.Exit();
        Time.timeScale = 1f;
        _pauseWindow.Hide();
    }
}
