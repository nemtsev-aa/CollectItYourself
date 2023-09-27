using CustomEventBus;
using CustomEventBus.Signals;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class PracticalPartDialog : Dialog {
    [Header("Navigations")]
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _showDescriptionWindowButton;
    [SerializeField] private Button _returnButton;
    [SerializeField] private Button _nextButton;

    [Header("Description Window")]
    private PracticalPartDescriptionDialog _descriptionWindow;

    [Header("View")]
    [SerializeField] private GoldCountView _goldCountView;

    private PracticalPartDescription _currentDescription;
    private EventBus _eventBus;
    private GoldController _goldController;
    private LearningModeManager _learningModeManager;

    public void Init(PracticalPartDescription currentDescription, LearningModeManager learningModeManager) {
        _learningModeManager = learningModeManager;
        _currentDescription = currentDescription;

        _mainMenuButton.onClick.AddListener(ShowMainMenu);
        _showDescriptionWindowButton.onClick.AddListener(ShowDescription);
        _returnButton.onClick.AddListener(ReturnToModeMenu);
        _nextButton.onClick.AddListener(NextStep);
    }

    private void ShowDescription() {
        _descriptionWindow = DialogManager.ShowDialog<PracticalPartDescriptionDialog>();
        _descriptionWindow.Init(_currentDescription);
    }

    private void NextStep() {
        // ѕодсчитать локальный прогресс главы
        int exp = _currentDescription.ExpAmountToComplete;
        _currentDescription.AddExperience(exp);
    }

    private void ReturnToModeMenu() {
        _learningModeManager.ReturnToChapterMenu(this);
    }

    private void ShowMainMenu() {
        _eventBus.Invoke(new ApplicationStateChangedSignal(ApplicationState.StartMenu));
    }
}
