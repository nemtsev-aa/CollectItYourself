using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsView : MonoBehaviour {
    [Tooltip("������ - ����� ����������")]
    [SerializeField] private ResultPanel _genetalResultView;
    [Tooltip("������ - ��������� ����������")]
    [SerializeField] private ResultPanel _singleResultView;
    [Tooltip("���������")]
    [SerializeField] private Transform _content;

    [Tooltip("������ ������� � ������������")]
    [SerializeField] private List<ResultPanel> _resultPanels = new List<ResultPanel>();
    [Tooltip("����� ������ ��� �����������")]
    [SerializeField] private List<SwitchingResult> _generalResult = new List<SwitchingResult>();
    [Tooltip("��������� ������ ��� �����������")]
    [SerializeField] private List<SingleSwitchingResult> _singleResults = new List<SingleSwitchingResult>();

    public void Initialize() {
        EventBus.Instance.CorrectChecked += AddResultToList;
        EventBus.Instance.IncorrectChecked += AddResultToList;
        
        EventBus.Instance.SingleCorrectChecked += AddSingleResultToList;
        EventBus.Instance.SingleIncorrectChecked += AddSingleResultToList;
    }

    public void AddResultToList(SwitchingResult result) {
        if (!_generalResult.Contains(result)) {
            _generalResult.Add(result);
        }
    }

    public void AddSingleResultToList(SingleSwitchingResult result) {
        if (!_singleResults.Contains(result)) {
            _singleResults.Add(result);
        }
    }

    public void UploadResultToGeneralPanel() {
        foreach (SwitchingResult iResult in _generalResult) {
            ResultPanel newPanel;
            newPanel = Instantiate(_genetalResultView, _content.transform);
            newPanel.transform.localScale = Vector3.one;
            newPanel.transform.position = new Vector3(newPanel.transform.position.x, newPanel.transform.position.y, 0f);
            newPanel.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            newPanel.transform.SetSiblingIndex(0);
            newPanel.Initialization(iResult);
            newPanel.ShowGeneralResult();
            Debug.Log("������ �������!");
        }
    }

    public void UploadResultsToSinglePanel() {

        foreach (SingleSwitchingResult iResult in _singleResults) {
            ResultPanel newPanel;
            newPanel = Instantiate(_singleResultView,_content.transform);
            newPanel.transform.localScale = Vector3.one;
            newPanel.transform.position = new Vector3(newPanel.transform.position.x, newPanel.transform.position.y, 0f);
            newPanel.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            newPanel.transform.SetSiblingIndex(_content.transform.childCount - 1);
            newPanel.Initialization(iResult);
            newPanel.ShowSingleResult();
            Debug.Log("������ �������!");
        }
    }

    private void OnDisable() {
        EventBus.Instance.CorrectChecked -= AddResultToList;
        EventBus.Instance.IncorrectChecked -= AddResultToList;

        EventBus.Instance.SingleCorrectChecked -= AddSingleResultToList;
        EventBus.Instance.SingleIncorrectChecked -= AddSingleResultToList;
    }
}
