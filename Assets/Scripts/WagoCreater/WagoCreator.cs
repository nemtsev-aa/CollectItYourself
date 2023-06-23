using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WagoCreator : MonoBehaviour
{
    [SerializeField] private SwitchBoxManager _switchBoxManager;
    public Pointer Pointer;

    private bool _isOverUI;

    public WagoClip CreateWago(WagoClipData wagoClipData) {
        GameObject newClip = Instantiate(wagoClipData.Prefab);
        WagoClip wago = newClip.GetComponent<WagoClip>(); // Создаём новый зажим
        if (_switchBoxManager.ActiveSwichBox != null) {
            wago.SwitchBox = _switchBoxManager.ActiveSwichBox;
            wago.Name = (_switchBoxManager.ActiveSwichBox.WagoClips.Count + 1).ToString(); // Присваиваем новому зажиму имя
            wago.transform.parent = _switchBoxManager.ActiveSwichBox.WagoClipsTransform.transform; // Прикрепляем новый зажим к РК
            wago.transform.position = Pointer.Aim.transform.position;
            wago.ShowName();
            _switchBoxManager.ActiveSwichBox.AddNewWagoClipToList(wago); // Добавляем новый зажим в список активной РК
        } else {
            wago.transform.parent = transform; // Прикрепляем новый зажим к генератору зажимов
        }
        
        //WagoPosition.gameObject.SetActive(false); // Скрываем меню выбора зажимов
        Pointer.gameObject.SetActive(true); // Отображаем указатель

        return wago;
    }
}
