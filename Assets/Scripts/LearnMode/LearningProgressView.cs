using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LearningProgressView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private Image _valueImage;
    private LearningProgressManager _learningProgressManager;
    
    public void Initialization(LearningProgressManager learningProgressManager) {
        _learningProgressManager = learningProgressManager;
        EventBus.Instance.ProgressValueChanget += ShowProgress;
    }

    public void ShowProgress(int value) {
        _valueText.text = (value * 100).ToString();
        _valueImage.fillAmount = value;
    }

    private void OnDisable() {
        EventBus.Instance.ProgressValueChanget += ShowProgress;
    }
}
