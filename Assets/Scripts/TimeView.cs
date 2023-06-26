using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText;
    private Stopwatch _stopwatch;

    public void Initialization(Stopwatch stopwatch) {
        _stopwatch = stopwatch;
        _stopwatch.TimeChanged += ShowTimeValue;
    }

    private void ShowTimeValue(string time) {
        _timeText.text = time;
    }

    private void OnDisable() {
        _stopwatch.TimeChanged -= ShowTimeValue;
    }
    //private void Update() {
    //    _timeText.text = _stopwatch.GetFormattedTime();
    //}
}
