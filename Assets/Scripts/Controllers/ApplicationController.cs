using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections;
using System.Collections.Generic;
using UI;
using UI.Dialogs;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ApplicationState {
    StartMenu,
    LearningMode,
    TrainingMode,
    ExamMode,
    Pause,
    Settings,
    Quit
}

/// <summary>
/// Менеджер состояний приложения
/// </summary>
public class ApplicationController : MonoBehaviour, IService {
    
    private EventBus _eventBus;                                                             // Шина событий
    private ApplicationState _currentApplicationState = ApplicationState.StartMenu;         // Текущее игровое состояние

    public void Init(EventBus eventBus) {
        _eventBus = eventBus;
        _eventBus.Subscribe((ApplicationStateChangedSignal signal) => SetApplicationState(signal));
    }

    public void SetApplicationState(ApplicationState state) {
        _currentApplicationState = state;
    }

    private void SetApplicationState(ApplicationStateChangedSignal signal) {
        _currentApplicationState = signal.CurrentState;
        Debug.Log($"Текущее состояние приложения {_currentApplicationState}");
        
        switch (_currentApplicationState) {
            case ApplicationState.StartMenu:
                _eventBus.Invoke(new StartMenuStateSignal());
                if (SceneManager.GetActiveScene().buildIndex != 1) SceneManager.LoadScene("MainMenu");
                break;
            case ApplicationState.LearningMode:
                _eventBus.Invoke(new LearningModeStateSignal());
                SceneManager.LoadScene("Learning");
                break;
            case ApplicationState.TrainingMode:
                _eventBus.Invoke(new TrainingModeStateSignal());
                SceneManager.LoadScene("Training");
                break;
            case ApplicationState.ExamMode:
                _eventBus.Invoke(new ExamModeStateSignal());
                SceneManager.LoadScene("Exam");
                break;
            case ApplicationState.Pause:
                _eventBus.Invoke(new PauseStateSignal());
                break;
            case ApplicationState.Settings:
                _eventBus.Invoke(new SettingsStateSignal());
                break;
            case ApplicationState.Quit:
                _eventBus.Invoke(new QuitStateSignal());
                break;
            default:
                break;
        }
    }
}
