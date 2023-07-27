using CustomEventBus;
using CustomEventBus.Signals;
using System;
using TMPro;
using UnityEngine;

public class SwitchBoxSelector : MonoBehaviour {
    public SwitchBox SwitchBox => _switchBox;
    public Action<SwitchBoxSelector> ActiveSwitchBoxSelectorChanged;
    
    [SerializeField] private BacklitButton _backlitButton;
    [SerializeField] private TextMeshProUGUI _selectorNumberText;

    private SwitchBox _switchBox;

    public void Init(SwitchBox switchBox) {
        _switchBox = switchBox;
        _selectorNumberText.text = $"PK{switchBox.SwitchBoxData.PartNumber}";
        _backlitButton.Button.onClick.AddListener(Activation);
    }

    public void Activation() {
        ActiveSwitchBoxSelectorChanged?.Invoke(this);
        _backlitButton.Status = true;
        _backlitButton.ShowOutline();
    }

    public void Deactivation() {
        _backlitButton.Status = false;
        _backlitButton.HideOutline();
    }
}
