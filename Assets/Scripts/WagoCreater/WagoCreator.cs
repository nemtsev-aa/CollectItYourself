using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagoCreator : MonoBehaviour
{
    public SwitchBox SwitchBox;

    [SerializeField] private WagoClip _wagoUp;
    [SerializeField] private WagoClip _wagoDown;
    [SerializeField] private Pointer _pointer;

    [Header("WagoPosition")]
    public GameObject WagoPosition;
    private Vector3 _wagoClipCreatePosition;

    private void LateUpdate() {
        if (Input.GetMouseButtonDown(1)) { // Вызываем меню выбора Wago-зажима нажатием на RightMouse
            ShowWagoPosition();
            _wagoClipCreatePosition = _pointer.Aim.transform.position;
            _pointer.gameObject.SetActive(false);
        }
    }

    void ShowWagoPosition() {
        WagoPosition.transform.position = Input.mousePosition; // Фиксируем начальное положение мыши
        WagoPosition.SetActive(true);
    }

    public void CreateWago(WagoType wagoType) {
        WagoClip newClip = (wagoType == WagoType.WagoU) ? _wagoUp : _wagoDown; // Определяем тип зажима
        WagoClip wago = Instantiate(newClip, _wagoClipCreatePosition, Quaternion.identity); // Создаём новый зажим
        wago.SwitchBox = SwitchBox;
        wago.Name = (SwitchBox.Wagos.Count + 1).ToString(); // Присваиваем новому зажиму имя
        wago.transform.position = _wagoClipCreatePosition;
        wago.transform.parent = SwitchBox.transform; // Прикрепляем новый зажим к РК
        wago.ShowName();

        SwitchBox.AddNewWagoClipToList(wago); // Добавляем новый зажим в список активной РК

        WagoPosition.gameObject.SetActive(false); // Скрываем меню выбора зажимов
        _pointer.gameObject.SetActive(true); // Отображаем указатель
    }
}
