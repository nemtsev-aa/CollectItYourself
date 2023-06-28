using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsView : MonoBehaviour
{
    [Tooltip("Префаб - общие результаты")]
    [SerializeField] private ResultPanel _genetalResultView;
    [Tooltip("Префаб - отдельные результаты")]
    [SerializeField] private ResultPanel _singleResultView;
    [Tooltip("Контейнер")]
    [SerializeField] private Transform _content;

    [Tooltip("Список панелей с результатами")]
    [SerializeField] private List<ResultPanel> _resultPanels = new List<ResultPanel>();
    [Tooltip("Данные для отображения")]
    [SerializeField] private List<SwitchingResult> _switchingResults = new List<SwitchingResult>();

    public void Initialize() {
        EventBus.Instance.CorrectChecked += AddResultToList;
        EventBus.Instance.IncorrectChecked += AddResultToList;
    }

    public void AddResultToList(SwitchingResult result) {
        if (!_switchingResults.Contains(result)) {
            _switchingResults.Add(result);
        }
    }
    public void AddResultToList(SingleSwitchingResult result) {
        if (!_switchingResults.Contains(result)) {
            _switchingResults.Add(result);
        }
    }

    public void UploadResultsToPanel() {
        foreach (SwitchingResult iResult in _switchingResults) {
            ResultPanel newPanel = new ResultPanel();
            if (iResult is SwitchingResult) {
                newPanel = Instantiate(_genetalResultView);
                newPanel.transform.parent = _content.transform;
                newPanel.transform.SetSiblingIndex(0);
                newPanel.Initialization(iResult);
                newPanel.ShowGeneralResult();
            }
            else if (iResult is SingleSwitchingResult) {
                newPanel = Instantiate(_singleResultView);
                newPanel.transform.parent = _content.transform;
                newPanel.transform.SetSiblingIndex(_content.transform.childCount-1);
                newPanel.Initialization(iResult);
                newPanel.ShowSingleResult();
            }  
        }
    }
}
