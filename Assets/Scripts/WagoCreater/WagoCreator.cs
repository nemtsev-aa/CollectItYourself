using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WagoCreator : MonoBehaviour, IService{
    private Pointer _pointer;
    private SwitchBoxManager _switchBoxManager;
    private bool _isOverUI;

    public void Init(SwitchBoxManager switchBox, Pointer pointer) {
        _switchBoxManager = switchBox;
        _pointer = pointer;
    }

    public WagoClip CreateWago(WagoClipData wagoClipData) {
        GameObject newClip = Instantiate(wagoClipData.Prefab);
        WagoClip wago = newClip.GetComponent<WagoClip>(); // Создаём новый зажим
        if (_switchBoxManager.ActiveSwichBox != null) {
            wago.ParentSwitchBox = _switchBoxManager.ActiveSwichBox;
            wago.Name = (_switchBoxManager.ActiveSwichBox.WagoClips.Count + 1).ToString(); // Присваиваем новому зажиму имя
            wago.transform.parent = _switchBoxManager.ActiveSwichBox.WagoClipsTransform.transform; // Прикрепляем новый зажим к РК
            wago.transform.position = _pointer.GetPosition();
            wago.ObjectViews[0].ShowName();
            _switchBoxManager.ActiveSwichBox.AddNewWagoClipToList(wago); // Добавляем новый зажим в список активной РК
        } else {
            wago.transform.parent = transform; // Прикрепляем новый зажим к генератору зажимов
        }
        
        //WagoPosition.gameObject.SetActive(false); // Скрываем меню выбора зажимов
        _pointer.gameObject.SetActive(true); // Отображаем указатель
        _switchBoxManager.ActiveSwichBox.ActiveWagoClip = wago;
        return wago;
    }
}
