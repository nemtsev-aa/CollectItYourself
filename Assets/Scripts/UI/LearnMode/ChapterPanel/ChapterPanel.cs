using CustomEventBus;
using CustomEventBus.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterPanel : MonoBehaviour {

    public LearningModeDescription ChapterDescription;
    [SerializeField] private TextMeshProUGUI _chepterTitle;
    [SerializeField] private Image _chepterIcon;
    [SerializeField] private Image _progressValueImage;
    [SerializeField] private TextMeshProUGUI _progressValueText;
    [SerializeField] private TextMeshProUGUI _chepterDescription;
    [SerializeField] private TextMeshProUGUI _expAmount;
    [SerializeField] private Button _playButton;

    private int _currentExpAmount;
    private LearningProgressManager _learningProgressManager;
    private EventBus _eventBus;

   [ContextMenu("Init")]
    public void Init(LearningProgressManager learningProgressManager) {
        _learningProgressManager = learningProgressManager;
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _chepterTitle.text = ChapterDescription.Name;
        _chepterIcon.sprite = ChapterDescription.Icon;
        _chepterDescription.text = ChapterDescription.Text;
        _expAmount.text = ChapterDescription.ExpAmountToComplete.ToString();

        _progressValueText.text = ChapterDescription.ProgressValue.ToString() + " %";
        _progressValueImage.fillAmount = ChapterDescription.ProgressValue / 100;

        _playButton.onClick.AddListener(PlayChapter);
        _eventBus.Subscribe<ChapterStartedSignal>(StartChapter, -1);
    }

    public void AddExperience(int expAmount) {
        _currentExpAmount += expAmount;
        if (_currentExpAmount >= ChapterDescription.ExpAmountToComplete) {
            _progressValueText.text = "100%";
            _progressValueImage.fillAmount = 1;
            
        } else {
            _progressValueText.text = ChapterDescription.ProgressValue.ToString() + " %";
            _progressValueImage.fillAmount = ChapterDescription.ProgressValue / 100;
        }
    }

    private void PlayChapter() {
        StartChapter(new ChapterStartedSignal(ChapterDescription));
    }

    /// <summary>
    /// Глава запущена
    /// </summary>
    /// <param name="signal"></param>
    private void StartChapter(ChapterStartedSignal signal) {
        _eventBus.Invoke(signal);
    }
}
