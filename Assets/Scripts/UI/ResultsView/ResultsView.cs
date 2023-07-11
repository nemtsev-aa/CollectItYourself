using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsView : MonoBehaviour {
    [Tooltip("Префаб - общие результаты")]
    [SerializeField] private ResultPanel _genetalResultView;
    [Tooltip("Префаб - отдельные результаты")]
    [SerializeField] private ResultPanel _singleResultView;
    [Tooltip("Контейнер")]
    [SerializeField] private Transform _content;

    [Tooltip("Список панелей с результатами")]
    [SerializeField] private List<ResultPanel> _resultPanels = new List<ResultPanel>();
    [Tooltip("Общие данные для отображения")]
    [SerializeField] GeneralSwitchingResult _generalResult;
    [Tooltip("Локальные данные для отображения")]
    [SerializeField] private List<SingleSwitchingResult> _singleResults = new List<SingleSwitchingResult>();

    public void Initialize(GeneralSwitchingResult switchingResult) {
        _generalResult = switchingResult;
        // Если в результате содержатся данные о сборке отдельных РК
        if (_generalResult.SingleSwichingResults.Count != 0) {
            foreach (SingleSwitchingResult iSingleResult in _generalResult.SingleSwichingResults) {
                if (!_singleResults.Contains(iSingleResult)) {
                    _singleResults.Add(iSingleResult);
                }
            }
        }
    }

    public void UploadResultToGeneralPanel() {
        ResultPanel newPanel;
        newPanel = Instantiate(_genetalResultView, _content.transform);
        newPanel.transform.localScale = Vector3.one;
        newPanel.transform.position = new Vector3(newPanel.transform.position.x, newPanel.transform.position.y, 0f);
        newPanel.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        newPanel.transform.SetSiblingIndex(0);
        newPanel.Initialization(_generalResult);
        newPanel.ShowGeneralResult();
        Debug.Log("Панель для отображения общего результата создана!");
    }

    public void UploadResultsToSinglePanel() {
        // Если в результате содержатся данные о сборке отдельных РК
        if (_generalResult.SingleSwichingResults.Count != 0) {
            foreach (SingleSwitchingResult iResult in _singleResults) {
                ResultPanel newPanel;
                newPanel = Instantiate(_singleResultView, _content.transform);
                newPanel.transform.localScale = Vector3.one;
                newPanel.transform.position = new Vector3(newPanel.transform.position.x, newPanel.transform.position.y, 0f);
                newPanel.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                newPanel.transform.SetSiblingIndex(_content.transform.childCount - 1);
                newPanel.Initialization(iResult);
                newPanel.ShowSingleResult();
                Debug.Log("Панель для отображения локального результата создана!");
            }
        }    
    }
}
