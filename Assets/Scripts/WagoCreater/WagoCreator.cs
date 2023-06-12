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
        if (Input.GetMouseButtonDown(1)) { // �������� ���� ������ Wago-������ �������� �� RightMouse
            ShowWagoPosition(true);
            _wagoClipCreatePosition = _pointer.Aim.transform.position;
            _pointer.gameObject.SetActive(false);
        } else if (Input.GetMouseButtonDown(0) && !_isOverUI) {
            ShowWagoPosition(false);
        }
    }

    void ShowWagoPosition(bool status) {
        WagoPosition.transform.position = Input.mousePosition; // ��������� ��������� ��������� ����
        WagoPosition.SetActive(status);
    }

    public void CreateWago(WagoType wagoType) {
        WagoClip newClip = (wagoType == WagoType.WagoU) ? _wagoUp : _wagoDown; // ���������� ��� ������
        WagoClip wago = Instantiate(newClip, _wagoClipCreatePosition, Quaternion.identity); // ������ ����� �����
        wago.SwitchBox = _switchBoxManager.ActiveSwichBox;
        wago.Name = (_switchBoxManager.ActiveSwichBox.WagoClips.Count + 1).ToString(); // ����������� ������ ������ ���
        wago.transform.position = _wagoClipCreatePosition;
        wago.transform.parent = _switchBoxManager.ActiveSwichBox.WagoClipsTransform.transform; // ����������� ����� ����� � ��
        wago.ShowName();

        _switchBoxManager.ActiveSwichBox.AddNewWagoClipToList(wago); // ��������� ����� ����� � ������ �������� ��

        WagoPosition.gameObject.SetActive(false); // �������� ���� ������ �������
        _pointer.gameObject.SetActive(true); // ���������� ���������
    }
}
