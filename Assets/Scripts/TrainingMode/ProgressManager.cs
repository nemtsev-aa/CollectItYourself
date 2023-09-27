using CustomEventBus;
using CustomEventBus.Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IDisposable = CustomEventBus.IDisposable;

public class ProgressManager : IService, IDisposable {
    public int CurrentExpValue => _currentExpValue;
    public int FullExpAmount => _fullExpAmount;

    protected int _currentExpValue;
    protected int _fullExpAmount;
    protected EventBus _eventBus;

    public virtual void Init() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _eventBus.Subscribe<TaskListCreatedSignal>(ShowProgressValue);
        _eventBus.Subscribe<TaskFinishedSignal>(Finished);
    }

    public virtual void ShowProgressValue(TaskListCreatedSignal signal) {

    }

    public virtual void Finished(TaskFinishedSignal signal) {

    }

    
    public virtual void Dispose() {
        _eventBus.Unsubscribe<TaskListCreatedSignal>(ShowProgressValue);
    }
}
