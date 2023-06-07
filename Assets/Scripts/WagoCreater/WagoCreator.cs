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
        if (Input.GetMouseButtonDown(1)) { // �������� ���� ������ Wago-������ �������� �� RightMouse
            ShowWagoPosition();
            _wagoClipCreatePosition = _pointer.Aim.transform.position;
            _pointer.gameObject.SetActive(false);
        }
    }

    void ShowWagoPosition() {
        WagoPosition.transform.position = Input.mousePosition; // ��������� ��������� ��������� ����
        WagoPosition.SetActive(true);
    }

    public void CreateWago(WagoType wagoType) {
        WagoClip newClip = (wagoType == WagoType.WagoU) ? _wagoUp : _wagoDown; // ���������� ��� ������
        WagoClip wago = Instantiate(newClip, _wagoClipCreatePosition, Quaternion.identity); // ������ ����� �����
        wago.SwitchBox = SwitchBox;
        wago.Name = (SwitchBox.Wagos.Count + 1).ToString(); // ����������� ������ ������ ���
        wago.transform.position = _wagoClipCreatePosition;
        wago.transform.parent = SwitchBox.transform; // ����������� ����� ����� � ��
        wago.ShowName();

        SwitchBox.AddNewWagoClipToList(wago); // ��������� ����� ����� � ������ �������� ��

        WagoPosition.gameObject.SetActive(false); // �������� ���� ������ �������
        _pointer.gameObject.SetActive(true); // ���������� ���������
    }
}
