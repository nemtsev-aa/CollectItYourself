using TMPro;
using UnityEngine;

public enum ResultPanelType {
    General,
    Single
} 

public class ResultPanel : MonoBehaviour {
    public ResultPanelType _currentType;

    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _checkResultText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _errorsText;

    private GeneralSwitchingResult _generalResult;
    private SingleSwitchingResult _singleResult;

    public void Init(GeneralSwitchingResult result) {
        _generalResult = result;
    }

    public void Init(SingleSwitchingResult result) {
        _singleResult = result;
    }

    public void ShowGeneralResult() {
        if (_generalResult != null) {
            _nameText.text = _generalResult.TaskData.ID;
            _checkResultText.text = _generalResult.CheckStatus ? "Успех" : "Неудача";
            _timeText.text = _generalResult.GetSwitchingTimesText();
            _errorsText.text = _generalResult.ErrorsCountText + "/" + _generalResult.TaskData.GetConnectionsCount().ToString();
        }   
    }

    public void ShowSingleResult() {
        if (_singleResult != null) {
            _nameText.text = _singleResult.SwitchBoxNumer.ToString();
            _checkResultText.text = _singleResult.CheckStatus ? "Успех" : "Неудача";
            _timeText.text = _singleResult.GetSwitchingTimesText();
            _errorsText.text = _generalResult.ErrorsCountText + "/" + _generalResult.TaskData.GetConnectionsCount().ToString();
        }
    }
}
