using TMPro;
using UnityEngine;

public class TimeView : MonoBehaviour {
    [SerializeField] private BacklitButton _startButton;
    [SerializeField] private BacklitButton _pauseButton;
    [SerializeField] private BacklitButton _stopButton;
    [SerializeField] private TextMeshProUGUI _timeText;

    private TimeManager _stopwatch;

    public void Init(TimeManager stopwatch) {
        _stopwatch = stopwatch;
        _startButton.Button.onClick.AddListener(StartTimer);
        _pauseButton.Button.onClick.AddListener(PauseTimer);
        _stopButton.Button.onClick.AddListener(StopTimer);
    }

    public void ShowTimeValue(string time) {
        _timeText.text = time;
    }

    private void StartTimer() {
        _stopwatch.SetStatus(true);
        ShowStatus(true);
    }

    private void PauseTimer() {
        _stopwatch.SetStatus(false);
        ShowStatus(false);
    }

    private void StopTimer() {
        _stopwatch.SetStatus(false);
        _stopwatch.Stop();

        ShowStatus(false);
    }

    public void ShowStatus(bool status) {
        if (status) {
            _startButton.gameObject.SetActive(false);
            _pauseButton.gameObject.SetActive(true);
            _pauseButton.HideOutline();
        } else {
            if (_stopwatch.GetTimeValue() == 0f) {          // Нажат "Стоп"
                _startButton.gameObject.SetActive(true);
                _startButton.HideOutline();
                _pauseButton.gameObject.SetActive(false);
            } else {                                        // Нажата "Пауза"
                _startButton.gameObject.SetActive(true);
                _startButton.HideOutline();
                _pauseButton.gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable() {
        _stopwatch.TimeChanged -= ShowTimeValue;
        _stopwatch.StatusChanged -= ShowStatus;
    }
}
