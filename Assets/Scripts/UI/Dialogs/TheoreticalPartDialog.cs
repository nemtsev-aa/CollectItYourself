using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class TheoreticalPartDialog : Dialog {
    public GoldCountView GoldCountView => _goldCountView;

    [Header("Navigations")]
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _returnButton;
    [SerializeField] private Button _nextButton;

    [Header("Description Window")]
    [SerializeField] private MyVideoPlayer _descriptionPlayer;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    [Header("View")]
    [SerializeField] private GoldCountView _goldCountView;

    private Description _currentDescription;
    private ServicesLoader_ExamMode _services;
}
