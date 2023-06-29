using System;
using UnityEngine;

public class Stopwatch : MonoBehaviour
{
    [field: SerializeField] public bool Status { get; private set; }

    [SerializeField] private TimeView _timeView;
    [SerializeField] private float _timeValue;

    public event Action<string> TimeChanged;
    public event Action<bool> StatusChanged;
 
    private void Start() {
        _timeView.Initialization(this);
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

    public void Reset() {
        _timeValue = 0;
        StatusChanged?.Invoke(Status);
    }

    private string GetFormattedTime() {
        int minutes = Mathf.FloorToInt(_timeValue / 60f);
        int seconds = Mathf.FloorToInt(_timeValue % 60f);
        int milliseconds = Mathf.FloorToInt((_timeValue * 1000f) % 1000f);

        return string.Format("{0:00}:{1:00}:{2:0}", minutes, seconds, milliseconds);
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
