using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class PracticalPartDescriptionDialog : Dialog {
    [SerializeField] private Button _hideWindowButton;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private MyVideoPlayer _videoPlayer;

    private PracticalPartDescription _currentDescription;

    public void Init(PracticalPartDescription currentDescription) {
        _currentDescription = currentDescription;

        _hideWindowButton.onClick.AddListener(HideDescription);
        _descriptionText.text = "";
    }

    private void HideDescription() {
        Hide();
    }
}
