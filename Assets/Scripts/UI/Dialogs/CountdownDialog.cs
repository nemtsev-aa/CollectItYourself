using System;
using UI;
using UnityEngine;

public class CountdownDialog : Dialog {
    [SerializeField] private CountdownView _countdownView;
    public Action<bool> OnCountdownFinish;

    public void Init() {
        _countdownView.Init(this);
    }
}
