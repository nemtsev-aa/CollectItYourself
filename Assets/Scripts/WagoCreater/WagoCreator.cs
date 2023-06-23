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
        WagoClip wago = newClip.GetComponent<WagoClip>(); // ������ ����� �����
        if (_switchBoxManager.ActiveSwichBox != null) {
            wago.SwitchBox = _switchBoxManager.ActiveSwichBox;
            wago.Name = (_switchBoxManager.ActiveSwichBox.WagoClips.Count + 1).ToString(); // ����������� ������ ������ ���
            wago.transform.parent = _switchBoxManager.ActiveSwichBox.WagoClipsTransform.transform; // ����������� ����� ����� � ��
            wago.transform.position = Pointer.Aim.transform.position;
            wago.ShowName();
            _switchBoxManager.ActiveSwichBox.AddNewWagoClipToList(wago); // ��������� ����� ����� � ������ �������� ��
        } else {
            wago.transform.parent = transform; // ����������� ����� ����� � ���������� �������
        }
        
        //WagoPosition.gameObject.SetActive(false); // �������� ���� ������ �������
        Pointer.gameObject.SetActive(true); // ���������� ���������

        return wago;
    }
}
