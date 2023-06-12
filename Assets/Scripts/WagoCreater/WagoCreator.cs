using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WagoCreator : MonoBehaviour
{
    [SerializeField] private SwitchBoxManager _switchBoxManager;
    [SerializeField] private Pointer _pointer;

    [Header("Prefabs")]
    [SerializeField] private WagoClip _wagoUp;
    [SerializeField] private WagoClip _wagoDown;
    
    [Header("WagoPosition")]
    public GameObject WagoPosition;
    private Vector3 _wagoClipCreatePosition;
    private bool _isOverUI;

    private void LateUpdate() {
        _isOverUI = EventSystem.current.IsPointerOverGameObject();
        if (Input.GetMouseButtonDown(1)) { // Вызываем меню выбора Wago-зажима нажатием на RightMouse
            ShowWagoPosition(true);
            _wagoClipCreatePosition = _pointer.Aim.transform.position;
            _pointer.gameObject.SetActive(false);
        } else if (Input.GetMouseButtonDown(0) && !_isOverUI) {
            ShowWagoPosition(false);
        }
    }

    void ShowWagoPosition(bool status) {
        WagoPosition.transform.position = Input.mousePosition; // Фиксируем начальное положение мыши
        WagoPosition.SetActive(status);
    }

    public void CreateWago(WagoType wagoType) {
        WagoClip newClip = (wagoType == WagoType.WagoU) ? _wagoUp : _wagoDown; // Определяем тип зажима
        WagoClip wago = Instantiate(newClip, _wagoClipCreatePosition, Quaternion.identity); // Создаём новый зажим
        wago.SwitchBox = _switchBoxManager.ActiveSwichBox;
        wago.Name = (_switchBoxManager.ActiveSwichBox.WagoClips.Count + 1).ToString(); // Присваиваем новому зажиму имя
        wago.transform.position = _wagoClipCreatePosition;
        wago.transform.parent = _switchBoxManager.ActiveSwichBox.WagoClipsTransform.transform; // Прикрепляем новый зажим к РК
        wago.ShowName();

        _switchBoxManager.ActiveSwichBox.AddNewWagoClipToList(wago); // Добавляем новый зажим в список активной РК

        WagoPosition.gameObject.SetActive(false); // Скрываем меню выбора зажимов
        _pointer.gameObject.SetActive(true); // Отображаем указатель
    }
}
