using UnityEngine;

public class WagoCreator : MonoBehaviour, IService{
    private Pointer _pointer;
    private SwitchBoxesManager _switchBoxManager;
    private bool _isOverUI;

    public void Init(SwitchBoxesManager switchBox, Pointer pointer) {
        _switchBoxManager = switchBox;
        _pointer = pointer;
    }

    public WagoClip CreateWago(WagoClipData wagoClipData) {
        GameObject newClip = Instantiate(wagoClipData.Prefab);
        WagoClip wago = newClip.GetComponent<WagoClip>(); // ������ ����� �����
        if (_switchBoxManager.ActiveSwichBox != null) {
            wago.ParentSwitchBox = _switchBoxManager.ActiveSwichBox;
            wago.Name = (_switchBoxManager.ActiveSwichBox.WagoClips.Count + 1).ToString(); // ����������� ������ ������ ���
            wago.transform.parent = _switchBoxManager.ActiveSwichBox.WagoClipsTransform.transform; // ����������� ����� ����� � ��
            wago.transform.position = _pointer.GetPosition();
            wago.Initialization();
            wago.ObjectViews[0].ShowName();
            _switchBoxManager.ActiveSwichBox.AddNewWagoClipToList(wago); // ��������� ����� ����� � ������ �������� ��
        } else {
            wago.transform.parent = transform; // ����������� ����� ����� � ���������� �������
        }
        
        _pointer.gameObject.SetActive(true); // ���������� ���������
        _switchBoxManager.ActiveSwichBox.ActiveWagoClip = wago;
        return wago;
    }
}
