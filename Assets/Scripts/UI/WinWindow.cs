using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinWindow : MonoBehaviour
{
    [Tooltip("Надпись - время сборки")]
    [SerializeField] private TextMeshProUGUI _timeCountText;
    [Tooltip("Кнопка для продолжения")]
    [SerializeField] private Button _continueButton;

    private void Awake()
    {
        _continueButton.onClick.AddListener(Hide);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        GameStateManager.Instance.SetMenu();
    }

    public void ShowResult(int errorCount, string timeValue) {
        _timeCountText.text = timeValue;
    }
}
