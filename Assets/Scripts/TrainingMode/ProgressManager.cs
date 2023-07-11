using CustomEventBus;
using CustomEventBus.Signals;
using System;
using System.Collections.Generic;
using UnityEngine;
using IDisposable = CustomEventBus.IDisposable;

public class ProgressManager : IService, IDisposable {
    [field: SerializeField] public int CurrentExpValue { get; private set; }
    protected ProgressView _progressView;
    protected int _fullExpAmount;
    protected EventBus _eventBus;

    protected Action<int> OnProgressChanged;
    
    public virtual void Init(ProgressView progressView) {
        _progressView = progressView;
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<TaskListCreatedSignal>(GetFullExpAmount);
        _eventBus.Subscribe<TaskFinishedSignal>(TaskFinished);

        OnProgressChanged += _progressView.ShowProgress;
    }
    private void TaskFinished(TaskFinishedSignal signal) {
        var task = signal.GeneralSwitchingResult.TaskData;
        var expValue = task.ExpValue;
        CurrentExpValue += expValue;

        int newProgressValue = (CurrentExpValue + expValue) / 100;
        OnProgressChanged?.Invoke(expValue);
    }

    private void GetFullExpAmount(TaskListCreatedSignal signal) {
        List<TaskData> taskList = signal.TaskDataList;
        if (taskList.Count > 0) {
            foreach (var iTask in taskList) {
                _fullExpAmount += iTask.ExpValue;
            }
        }
    }

    public void Dispose() {
        _eventBus.Unsubscribe<TaskListCreatedSignal>(GetFullExpAmount);
        OnProgressChanged -= _progressView.ShowProgress;
    }
}
