using CustomEventBus;
using CustomEventBus.Signals;
using System;
using UI;
using UI.Dialogs;
using UnityEngine;


public class MainMenuController : MonoBehaviour, IService, CustomEventBus.IDisposable {
    [SerializeField] private MainMenuDialog _mainMenuDialog;

    private EventBus _eventBus;
    public void Init(EventBus eventBus) {
        _eventBus = eventBus;
        _eventBus.Subscribe((StartMenuStateSignal signal) => ShowMainMenu(signal));
    }

    private void ShowMainMenu(StartMenuStateSignal signal) {
        if (ServiceLocator.Current.Get<GUIHolder>().transform.childCount == 0) {
            _mainMenuDialog = DialogManager.ShowDialog<MainMenuDialog>();
        }
    }

    public void Dispose() {
        _eventBus.Unsubscribe((StartMenuStateSignal signal) => ShowMainMenu(signal));
    }
}
