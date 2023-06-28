using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinWindow : MonoBehaviour
{
    [Tooltip("Визуализатор результатов")]
    [SerializeField] private ResultsView _resultsView;
    [Tooltip("Кнопка для продолжения")]
    [SerializeField] private Button _continueButton;

    private void Awake() {
        _continueButton.onClick.AddListener(Hide);
        _resultsView.Initialize();
    }

    public void Show() {
        ShowResult();
        gameObject.SetActive(true);
    }

    public void Hide() {
        GameStateManager.Instance.SetMenu();
    }

    public void ShowResult() {
        _resultsView.UploadResultsToPanel();
    }
}
