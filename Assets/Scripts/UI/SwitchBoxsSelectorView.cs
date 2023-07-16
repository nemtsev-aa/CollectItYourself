using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchBoxsSelectorView : MonoBehaviour {
    public int CurrentNumber => _currentNumber;
    [SerializeField] private Transform _buttons;
    private int _currentNumber;

    public Action<int> ActiveSwitchBoxChanged;
    
    public void Init(SwitchBoxManager switchBoxManager) {
        foreach (Button iButton in _buttons.GetComponentsInChildren<Button>()) {
            iButton.onClick.AddListener(() => OnButtonClick(iButton));
        }
        
        ActiveSwitchBoxChanged += switchBoxManager.SetActiveSwichBox;
    }

    private void OnButtonClick(Button iButton) {
        _currentNumber = int.Parse(iButton.gameObject.name);
        ActiveSwitchBoxChanged.Invoke(_currentNumber);
        Debug.Log(CurrentNumber);
    }
}

