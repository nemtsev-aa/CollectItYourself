using CustomEventBus;
using CustomEventBus.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainingTaskDescriptionView : MonoBehaviour {
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _playButton;
    [SerializeField] private SchemeView _principialSchemeView;
    [SerializeField] private SchemeView _wiringSchemeView;
    [SerializeField] private TextMeshProUGUI _variantNumberValue;
    [SerializeField] private TextMeshProUGUI _correctSwitchingCountValue;
    [SerializeField] private TextMeshProUGUI _bestTimeValue;

    private TaskDescriptionDialog _taskDescriptionDialog;
    private TaskData _taskData;
    private EventBus _eventBus;
    private AttemptsLog _attemptsLog;

    public void Init(TaskDescriptionDialog taskDescriptionDialog, TaskData taskData) {
        _taskData = taskData;
        _taskDescriptionDialog = taskDescriptionDialog;
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _attemptsLog = ServiceLocator.Current.Get<AttemptsLog>();

        _closeButton.onClick.AddListener(HideDialog);
        _playButton.onClick.AddListener(PlayTask);

        _principialSchemeView.Init(_taskData.PrincipalShemas[0]);
        _wiringSchemeView.Init(_taskData.WiringShema);
        _variantNumberValue.text = _taskData.ID;

        _correctSwitchingCountValue.text = _attemptsLog.GetCorrectSwitchingCount(_taskData.ID);
        _bestTimeValue.text = _attemptsLog.GetBestTime(_taskData.ID);
    }

    private void HideDialog() {
        _taskDescriptionDialog.Hide();
    }

    private void PlayTask() {
        _taskDescriptionDialog.Hide();
        _eventBus.Invoke(new TaskSelectSignal(_taskData));
    }
}
