using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class VerificationPartDialog : Dialog {
    private VerificationPartDescription _currentDescription;
    private LearningModeManager _learningModeManager;

    public void Init(VerificationPartDescription currentDescription, LearningModeManager learningModeManager) {
        _currentDescription = currentDescription;
        _learningModeManager = learningModeManager;
    }
}
