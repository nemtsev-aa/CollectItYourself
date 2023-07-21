using CustomEventBus;
using CustomEventBus.Signals;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchBoxsSelectorView : MonoBehaviour {
    public SwitchBox CurrentSwitchBox => _currentSwitchBox;

    [SerializeField] private SwitchBoxSelector _switchBoxSelectorPrefab;
    private List<SwitchBoxSelector> _switchBoxSelectors;
    private SwitchBox _currentSwitchBox;
    private SwitchBoxSelector _currentSelector;
    private SwitchBoxManager _switchBoxManager;

    public void Init(SwitchBoxManager switchBoxManager) {
        _switchBoxSelectors = new List<SwitchBoxSelector>();
        _switchBoxManager = switchBoxManager;
    }
 
    public void CreateSwitchBoxsSelector(SwitchBox switchBox) {
        SwitchBoxSelector newSelector = Instantiate(_switchBoxSelectorPrefab, transform);
        newSelector.Init(switchBox);
        newSelector.ActiveSwitchBoxSelectorChanged += SetActiveSwitchBoxSelector;
        _switchBoxSelectors.Add(newSelector);
    }

    public void SetActiveSwitchBoxSelector(SwitchBoxSelector switchBoxSelector) {
        _currentSelector.Deactivation();
        _currentSelector = switchBoxSelector;
        _switchBoxManager.SetActiveSwichBox(_currentSelector.SwitchBox);
    }

    public void SetActiveSwitchBoxSelector(int index) {
        if (_currentSelector != null) {
            _currentSelector.Deactivation();
        } else {
            SwitchBoxSelector switchBoxSelector = _switchBoxSelectors[index];
            if (switchBoxSelector != null) {
                _currentSelector = switchBoxSelector;
                _currentSelector.Activation();
            }
        } 
    }
}

