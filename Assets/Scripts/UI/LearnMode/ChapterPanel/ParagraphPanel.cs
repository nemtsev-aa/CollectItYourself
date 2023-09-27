using CustomEventBus;
using CustomEventBus.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParagraphPanel : MonoBehaviour {
    public ParagraphDescription Description => _paragraphDescription;

    [SerializeField] private TextMeshProUGUI _paragraphTitle;
    [SerializeField] private Image _paragraphIcon;
    [SerializeField] private TextMeshProUGUI _paragraphDescriptionText;
    [SerializeField] private StatusIconSelector _statusIconSelector;
    [SerializeField] private Button _playButton;

    private LearningModeManager _learningModeManager;
    private ParagraphDescription _paragraphDescription;
    private EventBus _eventBus;

    public void Init(LearningModeManager LearningModeManager, ParagraphDescription paragraphDescription) {
        _learningModeManager = LearningModeManager;
        _paragraphDescription = paragraphDescription;
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _playButton.onClick.AddListener(ParagraphPlayClick);

        DisplayParagraphDescription();
    }
    
    public void ShowStatus() {
        TaskStatus currentStatus = _paragraphDescription.Status;
        _statusIconSelector.SetStatus(currentStatus);

        if (currentStatus != TaskStatus.Lock) ShowPlayButton(true);
        else ShowPlayButton(false);
    }

    private void DisplayParagraphDescription() {
        SetTitleValue();
        ShowStatus();
    }

    private void SetTitleValue() {
        _paragraphIcon.sprite = _paragraphDescription.Icon;
        _paragraphDescriptionText.text = _paragraphDescription.Title;
        switch (_paragraphDescription.CurrentType) {
            case ParagraphType.Theoretical:
                _paragraphTitle.text = "Теория";
                break;
            case ParagraphType.Practical:
                _paragraphTitle.text = "Практика";
                break;
            case ParagraphType.Verification:
                _paragraphTitle.text = "Проверка";
                break;
            default:
                break;
        }
    }

    private void ShowPlayButton(bool status) {
        _playButton.gameObject.SetActive(status);
    }

    private void ParagraphPlayClick() {
        StartChapterPart(new ParagraphStartedSignal(_paragraphDescription));
    }

    /// <summary>
    /// Параграф запущен
    /// </summary>
    /// <param name="signal"></param>
    private void StartChapterPart(ParagraphStartedSignal signal) {
        _eventBus.Invoke(signal);
    }
}
