using CustomEventBus;
using CustomEventBus.Signals;
using TMPro;
using UnityEngine;

public class GoldCountView : MonoBehaviour, IService {
    [SerializeField] private TextMeshProUGUI _goldValueText;
    private EventBus _eventBus;

    public void Init() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<GoldChangedSignal>(ShowGoldCount);
        _goldValueText.text = ServiceLocator.Current.Get<GoldController>().Gold.ToString();
    }

    private void ShowGoldCount(GoldChangedSignal signal) {
        _goldValueText.text = signal.Gold.ToString();
    }

    private void OnDisable() {
        _eventBus.Unsubscribe<GoldChangedSignal>(ShowGoldCount);
    }
}
