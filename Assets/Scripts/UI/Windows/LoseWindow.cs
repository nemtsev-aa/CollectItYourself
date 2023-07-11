using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseWindow : MonoBehaviour
{
    [Tooltip("Визуализатор результатов")]
    [SerializeField] private ResultsView _resultsView;

    [Tooltip("Кнопка для выхода")]
    [SerializeField] private Button _exitButton;
    [Tooltip("Кнопка для возврата")]
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
