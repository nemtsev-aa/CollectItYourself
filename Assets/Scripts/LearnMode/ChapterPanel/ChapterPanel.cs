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
    
    public event Action<ChapterData> OnChapterComplite;
    
    private int _currentExpAmount;
    private LearningProgressManager _learningProgressManager;
    

    [ContextMenu("Initialization")]
    public void Initialization(LearningProgressManager learningProgressManager) {
        _learningProgressManager = learningProgressManager;
        _chepterTitle.text = ChapterData.ChepterTitle;
        _chepterIcon.sprite = ChapterData.ChepterIcon;
        _chepterDescription.text = ChapterData.ChepterDescription;
        _expAmount.text = ChapterData.ExpAmountToComplete.ToString();

        _progressValueText.text = ChapterData.ProgressValue.ToString() + " %";
        _progressValueImage.fillAmount = ChapterData.ProgressValue / 100;
    }



    public void AddExperience(int expAmount) {
        _currentExpAmount += expAmount;
        if (_currentExpAmount >= ChapterData.ExpAmountToComplete) {
            _progressValueText.text = "100 %";
            _progressValueImage.fillAmount = 1;
            
        }
        else {
            _progressValueText.text = ChapterData.ProgressValue.ToString() + " %";
            _progressValueImage.fillAmount = ChapterData.ProgressValue / 100;
        }
    }
}
