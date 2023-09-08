using CustomEventBus;
using CustomEventBus.Signals;
using TMPro;
using UnityEngine;

public class GoldCountView : MonoBehaviour, IService {
    [SerializeField] private TextMeshProUGUI _goldValueText;
    private EventBus _eventBus;
    private GoldController _goldController;

    public void Init(EventBus eventBus, GoldController goldController) {
        _eventBus = eventBus;
        _goldController = goldController;
        _eventBus.Subscribe<GoldChangedSignal>(ShowGoldCount);
        _goldValueText.text = $"{_goldController.Gold}";
    }

    public void ShowGoldCount() {
        _goldValueText.text = $"{_goldController.Gold}";
    }

    private void ShowGoldCount(GoldChangedSignal signal) {
        _goldValueText.text = signal.Gold.ToString();
    }

    private void OnDisable() {
        _eventBus.Unsubscribe<GoldChangedSignal>(ShowGoldCount);
    }
}
