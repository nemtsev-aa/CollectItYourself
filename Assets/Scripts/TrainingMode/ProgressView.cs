using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CustomEventBus;

[System.Serializable]
public class ProgressView : MonoBehaviour, IService {
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private Image _valueImage;

    protected EventBus _eventBus;
    protected ProgressManager _progressManager;

    public virtual void Init() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _progressManager = ServiceLocator.Current.Get<ProgressManager>();
    }

    public virtual void ShowProgress(int currentValue, int maxValue) {
        float xScale = (float)currentValue / maxValue;
        xScale = Mathf.Clamp01(xScale);

        _valueText.text = $"{Mathf.RoundToInt(xScale * 100)}%";
        _valueImage.fillAmount = xScale;
    }
}
