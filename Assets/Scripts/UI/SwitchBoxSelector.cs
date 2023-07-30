using System;
using TMPro;
using UnityEngine;

public class SwitchBoxSelector : MonoBehaviour {
    public SwitchBox SwitchBox => _switchBox;
    public Action<SwitchBoxSelector> ActiveSwitchBoxSelectorChanged;
    
    [SerializeField] private BacklitButton _backlitButton;
    [SerializeField] private TextMeshProUGUI _selectorNumberText;

    private SwitchBox _switchBox;
    private SwitchBoxsSelectorView _switchBoxsSelectorView;

    public void Init(SwitchBox switchBox, SwitchBoxsSelectorView switchBoxsSelectorView) {
        _switchBox = switchBox;
        _selectorNumberText.text = $"PK{switchBox.SwitchBoxData.PartNumber}";
        _backlitButton.Button.onClick.AddListener(Activation);
        _switchBoxsSelectorView = switchBoxsSelectorView;
        ActiveSwitchBoxSelectorChanged += _switchBoxsSelectorView.SetActiveSwitchBoxSelector;
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

    private void OnDisable() {
        ActiveSwitchBoxSelectorChanged += _switchBoxsSelectorView.SetActiveSwitchBoxSelector;
    }
}
