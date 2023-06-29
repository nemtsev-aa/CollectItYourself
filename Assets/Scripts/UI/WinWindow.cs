using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinWindow : MonoBehaviour
{
    [Tooltip("������������ �����������")]
    public ResultsView ResultsView;
    [Tooltip("������ ��� �����������")]
    [SerializeField] private Button _continueButton;

    private void Awake() {
        _continueButton.onClick.AddListener(Hide);
        ResultsView.Initialize();
    }

    public void Show() {
        ShowResult();
        gameObject.SetActive(true);
    }

    public void Hide() {
        GameStateManager.Instance.SetMenu();
    }

    public void ShowResult() {
        ResultsView.UploadResultToGeneralPanel();
        ResultsView.UploadResultsToSinglePanel();
    }
}
