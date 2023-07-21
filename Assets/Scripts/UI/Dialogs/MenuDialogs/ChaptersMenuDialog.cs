using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class ChaptersMenuDialog : Dialog {

    [SerializeField] private Button _exitButton;

    public override void Awake() {
        base.Awake();
        _exitButton.onClick.AddListener(Hide);
    }

    public override void Hide() {
        base.Hide();
        _exitButton.onClick.RemoveAllListeners();
    }
}
