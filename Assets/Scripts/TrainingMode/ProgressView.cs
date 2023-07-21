using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CustomEventBus;

[System.Serializable]
public class ProgressView : MonoBehaviour, IService, IDisposable {
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private Image _valueImage;

    protected EventBus _eventBus;
    public virtual void Init() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        //_eventBus.Subscribe<>
    }

    public virtual void ShowProgress(int value) {
        _valueText.text = (value * 100).ToString();
        _valueImage.fillAmount = value;
    }

    public void Dispose() {
        
    }
}
