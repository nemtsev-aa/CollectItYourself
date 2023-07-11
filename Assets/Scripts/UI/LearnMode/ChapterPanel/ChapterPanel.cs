using CustomEventBus;
using CustomEventBus.Signals;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterPanel : MonoBehaviour {

    public ChapterData ChapterData;
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

   [ContextMenu("Initialization")]
    public void Initialization(LearningProgressManager learningProgressManager) {
        _learningProgressManager = learningProgressManager;
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _chepterTitle.text = ChapterData.ChepterTitle;
        _chepterIcon.sprite = ChapterData.ChepterIcon;
        _chepterDescription.text = ChapterData.ChepterDescription;
        _expAmount.text = ChapterData.ExpAmountToComplete.ToString();

        _progressValueText.text = ChapterData.ProgressValue.ToString() + " %";
        _progressValueImage.fillAmount = ChapterData.ProgressValue / 100;

        _playButton.onClick.AddListener(PlayChapter);
        _eventBus.Subscribe<ChapterStartedSignal>(StartChapter, -1);
    }

    public void AddExperience(int expAmount) {
        _currentExpAmount += expAmount;
        if (_currentExpAmount >= ChapterData.ExpAmountToComplete) {
            _progressValueText.text = "100 %";
            _progressValueImage.fillAmount = 1;
            
        } else {
            _progressValueText.text = ChapterData.ProgressValue.ToString() + " %";
            _progressValueImage.fillAmount = ChapterData.ProgressValue / 100;
        }
    }

    private void PlayChapter() {
        StartChapter(new ChapterStartedSignal(ChapterData));
    }

    /// <summary>
    /// Глава запущена
    /// </summary>
    /// <param name="signal"></param>
    private void StartChapter(ChapterStartedSignal signal) {
        _eventBus.Invoke(signal);
    }
}
