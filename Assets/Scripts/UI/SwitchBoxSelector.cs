using CustomEventBus;
using CustomEventBus.Signals;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwitchBoxSelector : MonoBehaviour {
    public SwitchBox SwitchBox => _switchBox;
    [SerializeField] private BacklitButton _backlitButton;
    [SerializeField] private TextMeshProUGUI _selectorNumberText;
    public Action<SwitchBoxSelector> ActiveSwitchBoxSelectorChanged;

    private SwitchBox _switchBox;

    public void Init(SwitchBox switchBox) {
        _switchBox = switchBox;
        _selectorNumberText.text = switchBox.SwitchBoxData.PartNumber.ToString();
        _backlitButton.Button.onClick.AddListener(Activation);
    }

    public void Activation() {
        ActiveSwitchBoxSelectorChanged?.Invoke(this);
        _backlitButton.ShowOutline();
    }

    public void Deactivation() {
        _backlitButton.HideOutline();
    }
}
