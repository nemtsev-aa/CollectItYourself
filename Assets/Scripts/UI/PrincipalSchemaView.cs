using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrincipalSchemaView : MonoBehaviour, IDisposable {
    public TimeView TimeView => _timeView;

    [Tooltip("Надпись - название задания")]
    [SerializeField] private TextMeshProUGUI _name;
    [Tooltip("Изображение - схема")]
    [SerializeField] private Image _image;
    [Tooltip("Панель секундомера")]
    [SerializeField] private TimeView _timeView;
    
    private EventBus _eventBus;

    public void Init(Stopwatch stopwatch, EventBus eventBus) {
        _eventBus = eventBus;
        _eventBus.Subscribe<ActiveSwitchBoxChangedSignal>(UpdateView);
        _timeView.Init(stopwatch);
    }

    public void Show(string name, Sprite sprite) {
        _name.text = name;
        _image.sprite = sprite;
    }

    public void UpdateView(ActiveSwitchBoxChangedSignal signal) {
        int currentSwitchBoxNumber = signal.SwitchBox.SwitchBoxData.PartNumber;
        TaskData currentTaskData = ServiceLocator.Current.Get<TaskController>().CurrentTaskData;
        List<Sprite> list = currentTaskData.PrincipalShemas;
        Sprite principalSchema = null; 

        string currentSwitchBoxName = "";
        if (currentTaskData.Type == TaskType.Full) {
            currentSwitchBoxName = $"{currentTaskData.ID}_{currentSwitchBoxNumber}";
            principalSchema = list[currentSwitchBoxNumber];
        } else {
            currentSwitchBoxName = currentTaskData.ID.ToString();
            principalSchema = list[0];
        }

        Show(currentSwitchBoxName, principalSchema);
    }

    public void Dispose() {
        _name.text = "";
        _image.sprite = null;
        _timeView = null;
        _eventBus.Unsubscribe<ActiveSwitchBoxChangedSignal>(UpdateView);
    }
}
