using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    [Tooltip("Состояние - Основное меню")]
    [SerializeField] private GameState _startMenuState;
    [Tooltip("Состояние - Активное состояние игры")]
    [SerializeField] private GameState _actionState;    
    [Tooltip("Состояние - Пауза")]
    [SerializeField] private GameState _pauseState;    
    [Tooltip("Состояние - Победа")]
    [SerializeField] private GameState _winState;       
    [Tooltip("Состояние - Поражение")]
    [SerializeField] private GameState _loseState;

    private GameState _currentGameState; // Текущее игровое состояние
    

    public void Init()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this)
            Destroy(gameObject);

        _startMenuState?.Init(this);
        _actionState?.Init(this);
        _pauseState?.Init(this);
        _winState?.Init(this);
        _loseState?.Init(this);

        SetGameState(_startMenuState);
    }

    private void SetGameState(GameState gameState)
    {
        if (_currentGameState)
        {
            _currentGameState.Exit(); //Выходим из текущего состояния
        }
        _currentGameState = gameState; // Изменяем текущее состояние
        gameState.Enter();  //Входим в новое состояние
    }

    public void SetMenu()
    {
        SetGameState(_startMenuState);
    }

    public void SetAction()
    {
        SetGameState(_actionState);
    }

    public void SetPause()
    {
        SetGameState(_pauseState);
    }

    public void SetWin()
    {
        SetGameState(_winState);
    }

    public void SetLose()
    {
        SetGameState(_loseState);
    }
}
