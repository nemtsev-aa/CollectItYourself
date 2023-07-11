using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ResultPanelType {
    General,
    Single
} 

public class ResultPanel : MonoBehaviour
{
    public ResultPanelType _currentType;

    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _errorsText;

    private GeneralSwitchingResult _generalResult;
    private SingleSwitchingResult _singleResult;

    public void Initialization(GeneralSwitchingResult result) {
        _generalResult = result;
    }

    public void Initialization(SingleSwitchingResult result) {
        _singleResult = result;
    }

    public void ShowGeneralResult() {
        if (_generalResult != null) {
            _nameText.text = $"Часть {_generalResult.TaskData.Type}, Вариант {_generalResult.TaskData.Variant}";
            _timeText.text = _generalResult.GetSwitchingTimesText();
            _errorsText.text = _generalResult.GetErrorsCountText();
        }   
    }
    public void ShowSingleResult() {
        if (_singleResult != null) {
            _nameText.text = _singleResult.SwitchBoxNumer.ToString();
            _timeText.text = _singleResult.SwitchingTimeText;
            _errorsText.text = _singleResult.ErrorCountText;
        }
    }
}
