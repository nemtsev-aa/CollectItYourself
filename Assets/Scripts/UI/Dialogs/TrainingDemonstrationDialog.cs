using CustomEventBus;
using CustomEventBus.Signals;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class TrainingDemonstrationDialog : Dialog {
    [SerializeField] private ErrorCardView _errorCardView;
    [SerializeField] private SwitchBoxsSelectorView _switchBoxsSelectorView;
    [SerializeField] private ÑurrentPathManager _ñurrentPathManager;
    [SerializeField] private Button _startDemonstration;
    [SerializeField] private Button _returnToSwitching;
    [SerializeField] private Button _finishSwitching;
    [SerializeField] private Button _pauseDemonstration;

    private GeneralSwitchingResult _generalSwitchingResult;
    private EventBus _eventBus;

    public void Init(GeneralSwitchingResult generalSwitchingResult, EventBus eventBus) {
        _generalSwitchingResult = generalSwitchingResult;
        _eventBus = eventBus;
        _errorCardView.Init(_generalSwitchingResult.ErrorsList, _eventBus);
        
        SwitchBoxesManager switchBoxesManager = ServiceLocator.Current.Get<SwitchBoxesManager>();
        _switchBoxsSelectorView.Init(switchBoxesManager);
        _ñurrentPathManager.Init(switchBoxesManager, _eventBus);

        _startDemonstration.onClick.AddListener(StartDemonstration);
        _returnToSwitching.onClick.AddListener(ReturnToSwitching);
        _finishSwitching.onClick.AddListener(FinishSwitching);
        _pauseDemonstration.onClick.AddListener(PauseDemonstration);
    }

    private void StartDemonstration() {
        _ñurrentPathManager.Demonstration();
    }

    private void ReturnToSwitching() {
        _eventBus.Invoke(new TaskResumeSignal());
    }

    private void PauseDemonstration() {
        _eventBus.Invoke(new TaskPauseSignal());
    }

    private void FinishSwitching() {
        _eventBus.Invoke(new TaskFinishedSignal(_generalSwitchingResult));
    }
}
