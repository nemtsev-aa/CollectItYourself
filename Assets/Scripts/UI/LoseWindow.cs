using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseWindow : MonoBehaviour
{
    [Tooltip("������� - ���������� ������")]
    [SerializeField] private TextMeshProUGUI _errorsCountText;
    [Tooltip("������� - ����� ������")]
    [SerializeField] private TextMeshProUGUI _timeCountText;

    [Tooltip("������ ��� ������")]
    [SerializeField] private Button _exitButton;
    [Tooltip("������ ��� ��������")]
    [SerializeField] private Button _returnButton;
    
    private void Awake() {
        _exitButton.onClick.AddListener(Exit);
        _returnButton.onClick.AddListener(Exit);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Exit() {
        GameStateManager.Instance.SetMenu();
    }

    public void Return() {
        GameStateManager.Instance.SetAction();
    }

    public void ShowResult(int errorCount, string timeValue) {
        _errorsCountText.text = "������: " + errorCount.ToString();
        _timeCountText.text = timeValue;
    }
}
