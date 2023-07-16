using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeView : MonoBehaviour
{
    [SerializeField] private Button _start;
    [SerializeField] private Button _pause;
    [SerializeField] private Button _stop;
    [SerializeField] private Button _reset;

    [SerializeField] private TextMeshProUGUI _timeText;
    private Stopwatch _stopwatch;

    public void Init(Stopwatch stopwatch) {
        _stopwatch = stopwatch;
        _stopwatch.TimeChanged += ShowTimeValue;
    }

    private void ShowTimeValue(string time) {
        _timeText.text = time;
    }

    private void OnDisable() {
        _stopwatch.TimeChanged -= ShowTimeValue;
    }
}
