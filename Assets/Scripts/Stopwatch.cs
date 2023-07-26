using CustomEventBus;
using CustomEventBus.Signals;
using System;
using UnityEngine;

public class Stopwatch : MonoBehaviour, IService {
    [field: SerializeField] public bool Status { get; private set; }

    [SerializeField] private TimeView _timeView;
    [SerializeField] private float _timeValue;

    public event Action<string> TimeChanged;
    public event Action<bool> StatusChanged;
    
    private EventBus _eventBus;

    public void Init(TimeView timeView, EventBus eventBus) {
        _eventBus = eventBus;
        _timeView = timeView;
        _timeView.Init(this);
        _eventBus.Subscribe((TrainingModeStopSignal signal) => SetStatus(false));
        _eventBus.Subscribe((TrainingModeStartSignal signal) => SetStatus(true));
    }

    private void Update() {
        if (Status) {
            _timeValue += Time.deltaTime;
            TimeChanged?.Invoke(GetFormattedTime());
        }
    }

    public void SetStatus(bool status) {
        Status = status;
        StatusChanged?.Invoke(Status);
    }

    public void Stop() {
        _timeValue = 0;
        StatusChanged?.Invoke(Status);
        TimeChanged?.Invoke(GetFormattedTime());
    }

    private string GetFormattedTime() {
        int minutes = Mathf.FloorToInt(_timeValue / 60f);
        int seconds = Mathf.FloorToInt(_timeValue % 60f);
        int milliseconds = Mathf.FloorToInt((_timeValue * 10f) % 10f);
        if (minutes < 1) {
            return $"{seconds}.{milliseconds}";
        } else {
            return $"{minutes}:{seconds}.{milliseconds}";
        }
        
        //return string.Format("{0:00}:{1:00}.{2:0}", minutes, seconds, milliseconds);
    }

    public void SetTimeValue(float timeValue) {
        _timeValue = timeValue;

    }

    public float GetTimeValue() {
        return _timeValue;
    }

    public string GetTimeText() {
        return GetFormattedTime();
    }
}
