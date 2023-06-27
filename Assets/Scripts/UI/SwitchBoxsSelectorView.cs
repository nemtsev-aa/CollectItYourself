using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchBoxsSelectorView : MonoBehaviour {
    [field: SerializeField] public int CurrentNumber { get; private set; }

    [SerializeField] private Transform _buttons;
    public Action<int> ActiveSwitchBoxChanged;
    

    private void Start() {
        foreach (Button iButton in _buttons.GetComponentsInChildren<Button>()) {
            iButton.onClick.AddListener(() => OnButtonClick(iButton));
        }
    }

    private void OnButtonClick(Button iButton) {
        CurrentNumber = int.Parse(iButton.gameObject.name);
        ActiveSwitchBoxChanged.Invoke(CurrentNumber);
        Debug.Log(CurrentNumber);
    }
}

