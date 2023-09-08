using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

/// <summary>
/// Загрузчик сервисов для основного меню
/// </summary>
public class ServiceLoader_MainMenu : ServicesLoader {
    [SerializeField] private GUIHolder _guiHolder;                          // Контейнер UI 
    [SerializeField] private MainMenuController _mainMenuController;        // Менеджер основного меню
    private GoldController _goldController;                                 // Менеджер золота

    private void Start() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _savesManager = ServiceLocator.Current.Get<SavesManager>();
        _goldController = new GoldController();
        Init();

        _eventBus.Invoke(new ApplicationStateChangedSignal(ApplicationState.StartMenu));
    }

    public override void Init() {
        _goldController.Init();

        RegisterServices();
        AddDisposables();

        _mainMenuController.Init(_eventBus);
    }

    public override void RegisterServices() {
        ServiceLocator.Current.RegisterWithReplacement(_guiHolder);
        ServiceLocator.Current.RegisterWithReplacement(_goldController);
        ServiceLocator.Current.Register(_mainMenuController);

        //ServiceLocator.Current.Register(_progressView);
        //ServiceLocator.Current.Register(_savesManager);

        //Debug.Log("RegisterServices complite");
    }

    public override void AddDisposables() {
        _disposables.Add(_mainMenuController);
        _disposables.Add(_goldController);
        _disposables.Add(_mainMenuController);
    }

    public override void OnDestroy() {
        foreach (var disposable in _disposables) {
            disposable.Dispose();
        }
    }
}

