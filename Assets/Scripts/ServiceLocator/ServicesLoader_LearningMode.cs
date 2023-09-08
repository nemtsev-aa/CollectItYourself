using CustomEventBus;
using UI;
using UI.Dialogs;
using UnityEngine;

public class ServicesLoader_LearningMode : ServicesLoader {
    [SerializeField] private GUIHolder _guiHolder;                                                  // Контейнер для всплывающих окон
    [SerializeField] private LearningModeDescriptionSOLoader _learningModeDescriptionSOLoader;
    private LearningModeManager _learningModeManager;                                               // Менеджер состояния приложения в модуле "Обучение"
    private ServiceLocator _serviceLocator;

    private void Start() {
        _serviceLocator = ServiceLocator.Current;
        
        Init();
        _learningModeManager = new LearningModeManager();
        _learningModeManager.Init();
        _learningModeManager.ShowLearningModeMenu();
    }

    public override void Init() {
        RegisterServices();
        AddDisposables();
    }

    public override void RegisterServices() {
        _serviceLocator.RegisterWithReplacement(_guiHolder);
        _serviceLocator.Register(_learningModeDescriptionSOLoader);
    }

    public override void AddDisposables() {
        //_disposables.Add(_guiHolder);
        //_disposables.Add(_learningModeDescriptionSOLoader);
    }

    public override void OnDestroy() {
        
    }
}
