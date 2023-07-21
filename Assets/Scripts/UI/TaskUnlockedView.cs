using CustomEventBus;
using CustomEventBus.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskUnlockedView : MonoBehaviour {
    [SerializeField] private Button _exitButton;

    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _buyTrueButton;
    [SerializeField] private Button _buyFalseButton;

    [SerializeField] private TextMeshProUGUI _PriceValueText;

    private TaskUnlockedDialog _taskUnlockedDialog;
    private TaskData _taskData;
    private EventBus _eventBus;

    public void Init(TaskUnlockedDialog dialog, TaskData taskData) {
        _taskUnlockedDialog = dialog;
        _taskData = taskData;

        _exitButton.onClick.AddListener(HideDialog);
        _buyButton.onClick.AddListener(TryBuy);
        _buyButton.gameObject.SetActive(true);
        _buyTrueButton.onClick.AddListener(HideDialog);
        _buyTrueButton.gameObject.SetActive(false);
        _buyFalseButton.onClick.AddListener(HideDialog);
        _buyFalseButton.gameObject.SetActive(false);

        _PriceValueText.text = _taskData.CoinsCount.ToString();
        _eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    private void TryBuy() {
        GoldController gold = ServiceLocator.Current.Get<GoldController>();
        if (gold.HaveEnoughGold(_taskData.CoinsCount)) {
            _eventBus.Invoke(new SpendGoldSignal(_taskData.CoinsCount));
            _taskData.SetStatus(TaskStatus.Unlock);
            _taskUnlockedDialog.OnTaskUnlocked?.Invoke();
            _buyTrueButton.gameObject.SetActive(true);
            _buyButton.gameObject.SetActive(false);
            Invoke(nameof(HideDialog), 0.5f);
        } else {
            _buyFalseButton.gameObject.SetActive(true);
            _buyButton.gameObject.SetActive(false);
            Invoke(nameof(HideDialog), 0.5f);
        }
    }

    private void HideDialog() {
        _taskUnlockedDialog.Hide();
    }

    private void TransitionToTaskDescription() {

    }
}
