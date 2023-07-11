using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseWindow : MonoBehaviour
{
    [Tooltip("������������ �����������")]
    [SerializeField] private ResultsView _resultsView;

    [Tooltip("������ ��� ������")]
    [SerializeField] private Button _exitButton;
    [Tooltip("������ ��� ��������")]
    [SerializeField] private Button _returnButton;

    private void Awake() {
        _exitButton.onClick.AddListener(Exit);
        _returnButton.onClick.AddListener(Return);
        //_resultsView.Initialize();
    }

    public void Show() {
        gameObject.SetActive(true);
        ShowResult();
    }

    public void Exit() {
        GameStateManager.Instance.SetMenu();
    }

    public void Return() {
        GameStateManager.Instance.SetAction();
    }

    public void ShowResult() {
        _resultsView.UploadResultToGeneralPanel();
        _resultsView.UploadResultsToSinglePanel();
    }
}
