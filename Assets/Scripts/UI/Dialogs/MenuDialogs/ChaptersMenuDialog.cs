using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class ChaptersMenuDialog : Dialog {

    [SerializeField] private Button _exitButton;

    protected override void Awake() {
        base.Awake();
        _exitButton.onClick.AddListener(Hide);
    }

    protected override void Hide() {
        base.Hide();
        _exitButton.onClick.RemoveAllListeners();
    }
}
