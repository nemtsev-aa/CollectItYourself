using CustomEventBus;
using CustomEventBus.Signals;
using System;
using UnityEngine;

/// <summary>
/// Система отвечающая за золото:
/// Начисление, трата, изменение золота
/// </summary>
public class GoldController : IService, CustomEventBus.IDisposable {
    private string key = StringConstants.GENERAL_SAVES;
    
    private int _gold;
    public int Gold => _gold;
    private EventBus _eventBus;
    private SavesManager _savesManager;

    public void Init() {      
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<AddGoldSignal>(OnAddGold);
        _eventBus.Subscribe<SpendGoldSignal>(SpendGold);
        _eventBus.Subscribe<GoldChangedSignal>(GoldChanged);
        _eventBus.Subscribe<TaskFinishedSignal>(TaskFinished);

        _savesManager = ServiceLocator.Current.Get<SavesManager>();
        GetGoldValueFromSaveFile();
    }

    private void GetGoldValueFromSaveFile() {
        GeneralIndicators generalSaves = new GeneralIndicators();
        _savesManager.CurrentService.Load<GeneralIndicators>(key, loadResult => {        // Получаем сохранённые данные
            if (loadResult != null) {
                _gold = loadResult.Gold;
            } else {
                _gold = generalSaves.Gold;
            }
        });
    }

    private void SetGoldValueToSaveFile() {
        GeneralIndicators generalSaves = new GeneralIndicators() {
            UserName = "@user",
            LastVisitDate = DateTime.Now,
            Gold = _gold
        };

        _savesManager.CurrentService.Save(key, generalSaves);                            // Сохраняем данные
    }

    private void OnAddGold(AddGoldSignal signal) {
        OnAddGold(signal.Value);
    }
    
    private void OnAddGold(int value) {
        _gold += value;
        _eventBus.Invoke(new GoldChangedSignal(_gold));
    }

    public bool HaveEnoughGold(int gold) {
        return _gold >= gold;
    }

    private void SpendGold(SpendGoldSignal signal) {
        if (HaveEnoughGold(signal.Value)) {
            _gold -= signal.Value;
            _eventBus.Invoke(new GoldChangedSignal(_gold));
        }
    }

    private void GoldChanged(GoldChangedSignal signal) {
        _gold = signal.Gold;
        SetGoldValueToSaveFile();
    }

    private void TaskFinished(TaskFinishedSignal signal) {
        if (signal.GeneralSwitchingResult.CheckStatus) {
            OnAddGold(signal.GeneralSwitchingResult.TaskData.CoinsCount);
        } else {
            OnAddGold(0);
        }
    }

    public void ShowCurrentGoldValue() => OnAddGold(_gold);

    public void Dispose() {
        _eventBus.Unsubscribe<AddGoldSignal>(OnAddGold);
        _eventBus.Unsubscribe<SpendGoldSignal>(SpendGold);
        _eventBus.Unsubscribe<GoldChangedSignal>(GoldChanged);
        _eventBus.Unsubscribe<TaskFinishedSignal>(TaskFinished);
    }
}